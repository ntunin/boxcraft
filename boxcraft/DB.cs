using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    public class DB
    {
        public static List<User> Users = new List<User>
        {
            new User("Nikita Ch", "Stone"),
            new User("Nikita M", "Stone"),
            new User("Vyacheslav", "Stone"),
            new User("Aleksey", "Stone"),
            new User("Victor", "Stone"),
            new User("Asel", "Stone"),
            new User("Anastasiya", "Stone")
        };

        public class User
        {
            public string Name;
            public string Skin;

            public User(string name, string skin)
            {
                Name = name;
                Skin = skin;

            }
        }
    }
}
