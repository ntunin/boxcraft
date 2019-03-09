using D3DX;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    class Box
    {
        public Prefab prefab;
        public string type;
        public Box(string type, Prefab prefab)
        {
            this.type = type;
            this.prefab = prefab;
        }
    }
}
