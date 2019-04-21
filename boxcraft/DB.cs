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
            new User("Nikita Ch", "Nikita Ch"),
            new User("Nikita M", "Nikita M"),
            new User("Vyacheslav", "Vyacheslav"),
            new User("Aleksey", "Aleksey"),
            new User("Victor", "Victor"),
            new User("Asel", "Asel"),
            new User("Nastya", "Nastya"),
            new User("Nik", "Nik"),
            new User("Cam", "Cam")
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
