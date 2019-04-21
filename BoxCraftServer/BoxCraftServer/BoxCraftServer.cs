using boxcraft;
using ClientServer;
using System;
using System.Collections.Generic;

namespace BoxCraftServer
{
    public class BoxCraftServer : Server, ISessionDelegate
    {
        private World world;
        private Session session;
        private SavingService savingService = new SavingService();
        private Pipe currentPipe;
        private Dictionary<string, Pipe> pipes = new Dictionary<string, Pipe>();
        private BoxCraftScene scene;

        public BoxCraftServer()
        {
            session = new Session(this);
            var form = new Form1(); // To primary device initializtion
            scene = form.scene;
            new SkinLoader().Load();
            if (savingService.HasSave())
            {
                world = savingService.Load();
            }
            else
            {
                world = new WorldGenarator().Generate();
            }
        }

        public override void HandleRequest(string request, Pipe pipe)
        {
            lock(scene)
            {
                currentPipe = pipe;
                session.Handle(request);
            }
        }

        public void OnJoin(Join join)
        {
            pipes[join.UserName] = currentPipe;
            join.Apply(world);
            var message = join.ToString();
            var response = new CreateWorld(world).ToString();
            ForEachUserPipe((username, pipe) =>
            {
                if (username == join.UserName)
                {
                    pipe.SendMessage(response);
                }
                else
                {
                    pipe.SendMessage(message);
                }
            });
        }

        public void OnMove(Move move)
        {
            Apply(move);
        }

        public void OnRotate(Rotate rotate)
        {
            Apply(rotate);
        }

        public void OnWorldInit(CreateWorld createWorld)
        {
        }

        public void OnBoxCreate(CreateBox createBox)
        {
            Apply(createBox);
        }

        public void OnBoxRemove(RemoveBox removeBox)
        {
            Apply(removeBox);
        }

        private void Apply(Command command)
        {
            command.Apply(world);
            var message = command.ToString();
            ForEachUserPipe((username, pipe) =>
            {
                if (username != command.UserName)
                {
                    pipe.SendMessage(message);
                }
            });
        }

        private void ForEachUserPipe(Action<string, Pipe> action)
        {
            foreach (var userPipe in pipes)
            {
                action(userPipe.Key, userPipe.Value);
            }
        }
    }
}
