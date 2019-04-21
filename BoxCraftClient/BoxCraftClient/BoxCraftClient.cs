using boxcraft;
using ClientServer;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BoxCraftClient
{
    class BoxCraftClient : Client, BoxCraftSceneDelegate, ISessionDelegate
    {
        public World world;
        BoxCraftScene scene;
        Session session;
        User user;

        public BoxCraftClient(string host, string username, BoxCraftScene scene)
        {
            session = new Session(this);
            user = UserFactyory.CreateUser(username, new Vector3(), new Vector3());
            this.scene = scene;
            scene.dlgt = this;
            Authenticate(new SocketConnection(host, 11020));
            Send(new Join(user).ToString());
        }

        public override void Handle(string message)
        {
            lock(scene)
            {
                session.Handle(message);
            }      
        }

        public void OnCreateBox(Box box)
        {
            Send(new CreateBox(Constants.username, box).ToString());
        }

        public void OnRemoveBox(Box box)
        {
            Send(new RemoveBox(Constants.username, box).ToString());
        }

        public void OnMove(Vector3 position)
        {
            Send(new Move(user.name, position).ToString());
        }

        public void OnRotate(Vector3 rotation)
        {
            Send(new Rotate(user.name, rotation).ToString());
        }

        public void OnJoin(Join join)
        {
            join.Apply(world);
        }

        public void OnMove(Move move)
        {
            move.Apply(world);
        }

        public void OnRotate(Rotate rotate)
        {
            rotate.Apply(world);
        }

        public void OnWorldInit(CreateWorld createWorld)
        {
            world = createWorld.World;
            scene.Load(world);
            var user = world.users.Find(aUser => aUser.name == Constants.username);
            if (user != null)
            {
                world.prefab.RemoveChild(user.prefab);
            }
        }

        public void OnBoxCreate(CreateBox createBox)
        {
            createBox.Apply(world);
        }

        public void OnBoxRemove(RemoveBox removeBox)
        {
            removeBox.Apply(world);
        }
    }
}
