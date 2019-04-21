using Microsoft.DirectX;
using System;

namespace boxcraft
{
    public class WorldSerializer
    {
        private static BoxParser boxParser = new BoxParser();
        private static UserParser userParser = new UserParser();

        public static string Serialize(World world)
        {
            string result = "";
            result += "boxes\n";
            world.boxes.ForEach((Box box) => { result += SerizlizeBox(box) + '\n'; });
            result += "users\n";
            world.users.ForEach((User user) => { result += SerializeUser(user) + '\n'; });
            return result;
        }

        public static World Deserialize(string str)
        {
            var world = new World();
            string[] lines = str.Split('\n');

            Parser parser = null;

            foreach(string line in lines)
            {
                if (line.Equals("boxes")) {
                    parser = boxParser;
                } else if (line.Equals("users")) {
                    parser = userParser;
                } else {
                    parser.Parse(line, world);
                }
            }
            return world;
        }

        public static User DeserializeUser(string str)
        {
            return userParser.Parse(str);
        }

        public static string SerializeUser(User user)
        {
            return userParser.Archive(user);
        }

        public static Box DeserializeBox(string str)
        {
            return boxParser.Parse(str);
        }

        public static string SerizlizeBox(Box box)
        {
            return boxParser.Archive(box);
        }

        public static string SerializeVector(Vector3 vector)
        {
            return $"{vector.X};{vector.Y};{vector.Z}";
        }

        public static Vector3 DeserializeVector(string str)
        {
            return boxParser.ParseVector3(str);
        }

        private abstract class Parser
        {
            public abstract void Parse(string line, World world);

            public Vector3 ParseVector3(string line)
            {
                string[] components = line.Split(';');
                if(components.Length != 3)
                {
                    throw new Exception($"Vrong vector3 string passed: {line}");
                }
                return new Vector3(float.Parse(components[0]),
                                   float.Parse(components[1]),
                                   float.Parse(components[2]));
            }
        }

        private class UserParser: Parser
        {
            private UserFactyory userFactyory = new UserFactyory();

            public override void Parse(string line, World world)
            {
                if (line.Length == 0)
                {
                    return;
                }
                var user = Parse(line);
                world.Add(user);
            }

            public User Parse(string line)
            {
                string[] components = line.Split('|');
                if (components.Length != 3)
                {
                    throw new Exception($"Wrong user line passed: {line}");
                }
                string name = components[0];
                Vector3 position = ParseVector3(components[1]);
                Vector3 rotation = ParseVector3(components[2]);
                var user = UserFactyory.CreateUser(name, position, rotation);
                return user;
            }

            public string Archive(User user)
            {
                var p = user.prefab.Body.Position;
                var r = user.prefab.Body.Rotation;
                return $"{user.name}|{SerializeVector(p)}|{SerializeVector(r)}";
            }
        }

        private class BoxParser : Parser
        {
            public override void Parse(string line, World world)
            {
                if (line.Length == 0)
                {
                    return;
                }
                var box = Parse(line);
                world.Add(box);
            }

            public Box Parse(string line)
            {
                string[] components = line.Split('|');
                if (components.Length != 2)
                {
                    throw new Exception($"Wrong box line passed: {line}");
                }
                string type = components[0];
                Vector3 position = ParseVector3(components[1]);
                var box = BoxFactory.CreateBox(type, position);
                return box;
            }

            public string Archive(Box box)
            {
                var p = box.prefab.Body.Position;
                return $"{box.type}|{SerializeVector(p)}";
            }
        }
    }
}
