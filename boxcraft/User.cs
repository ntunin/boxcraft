using D3DX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    public class User
    {
        public string name;
        public Prefab prefab;

        public User(string name, Prefab prefab)
        {
            this.name = name;
            this.prefab = prefab;
        }
    }
}
