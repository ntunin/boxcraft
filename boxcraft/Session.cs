using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace boxcraft
{
    public abstract class Command
    {
        public string UserName;
        public abstract void Apply(World world);
        
    }

    public class CreateWorld : Command
    {
        public World World;

        public CreateWorld(World world)
        {
            World = world;
        }

        public CreateWorld(XElement e)
        {
            World = WorldSerializer.Deserialize(e.Value);
        }

        public override void Apply(World world)
        {
        }

        public override string ToString()
        {
            return $"<world>{WorldSerializer.Serialize(World)}</world>";
        }
    }

    public class Join: Command
    {
        private User user;

        public Join(User user)
        {
            this.user = user;
            UserName = user.name;
        }

        public Join(XElement e)
        {
            user = WorldSerializer.DeserializeUser(e.Value);
            UserName = user.name;
        }


        public override string ToString()
        {
            return $"<join>{WorldSerializer.SerializeUser(user)}</join>";
        }

        public override void Apply(World world)
        {
            world.Add(user);
        }
    }

    public class Move: Command
    {
        private Vector3 position;

        public Move(string username, Vector3 position)
        {
            UserName = username;
            this.position = position;
        }

        public Move(XElement e)
        {
            var attribute = e.Attribute(XName.Get("user"));
            UserName = attribute.Value;
            position = WorldSerializer.DeserializeVector(e.Value);
        }

        public override string ToString()
        {
            return $"<move user=\"{UserName}\">{WorldSerializer.SerializeVector(position)}</move>";
        }

        public override void Apply(World world)
        {
            var user = world.users.Find(aUser => aUser.name == UserName);
            if (user != null)
            {
                user.prefab.Body.Position = position;
            }
        }
    }

    public class Rotate: Command
    {
        private Vector3 rotation;

        public Rotate(string username, Vector3 rotation)
        {
            UserName = username;
            this.rotation = rotation;
        }

        public Rotate(XElement e)
        {
            var attribute = e.Attribute(XName.Get("user"));
            UserName = attribute.Value;
            rotation = WorldSerializer.DeserializeVector(e.Value);
        }

        public override string ToString()
        {
            return $"<rotate user=\"{UserName}\">{WorldSerializer.SerializeVector(rotation)}</rotate>";
        }

        public override void Apply(World world)
        {
            var user = world.users.Find(aUser => aUser.name == UserName);
            if (user != null)
            {
                user.prefab.Body.Rotation = rotation;
            }
        }
    }

    public class CreateBox: Command
    {
        public Box Box;

        public CreateBox(string username, Box box)
        {
            UserName = username;
            Box = box;
        }

        public CreateBox(XElement e)
        {
            var attribute = e.Attribute(XName.Get("user"));
            UserName = attribute.Value;
            Box = WorldSerializer.DeserializeBox(e.Value);
        }

        public override void Apply(World world)
        {
            world.Add(Box);
        }

        public override string ToString()
        {
            return $"<create user=\"{UserName}\">{WorldSerializer.SerizlizeBox(Box)}</create>";
        }
    }

    public class RemoveBox : Command
    {
        public Box Box;

        public RemoveBox(string username, Box box)
        {
            UserName = username;
            Box = box;
        }

        public RemoveBox(XElement e)
        {
            var attribute = e.Attribute(XName.Get("user"));
            UserName = attribute.Value;
            Box = WorldSerializer.DeserializeBox(e.Value);
        }

        public override void Apply(World world)
        {
            var removingBox = Session.Find(Box, world);
            if (removingBox != null)
            {
                world.Remove(removingBox);
            }
        }

        public override string ToString()
        {
            return $"<remove user=\"{UserName}\">{WorldSerializer.SerizlizeBox(Box)}</remove>";
        }
    }

    public interface ISessionDelegate
    {
        void OnJoin(Join join);
        void OnMove(Move move);
        void OnRotate(Rotate rotate);
        void OnWorldInit(CreateWorld createWorld);
        void OnBoxCreate(CreateBox createBox);
        void OnBoxRemove(RemoveBox removeBox);
    }

    public class Session
    {
        ISessionDelegate dlgt;

        public Session(ISessionDelegate dlgt)
        {
            this.dlgt = dlgt;
        }

        public void Handle(string request)
        {
            XElement e = XElement.Parse(request);
            switch (e.Name.LocalName)
            {
                case "join":
                    dlgt.OnJoin(new Join(e));
                    break;
                case "move":
                    dlgt.OnMove(new Move(e));
                    break;
                case "rotate":
                    dlgt.OnRotate(new Rotate(e));
                    break;
                case "world":
                    dlgt.OnWorldInit(new CreateWorld(e));
                    break;
                case "create":
                    dlgt.OnBoxCreate(new CreateBox(e));
                    break;
                case "remove":
                    dlgt.OnBoxRemove(new RemoveBox(e));
                    break;
            }
        }

        public static Box Find(Box box, World world)
        {
            return world.boxes.Find(aBox =>
            {
                return box.prefab.Body.Position.Equals(aBox.prefab.Body.Position);
            });

        }
    }
}
