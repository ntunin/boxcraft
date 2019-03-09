using D3DX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    class SkinLoader
    {

        public void Load()
        {
            new SkinBuilder(new Dictionary<string, object>
            {
                {"File", "box.X"}
            }).Create();
            new SkinBuilder(new Dictionary<string, object>
            {
                {"File", "box.X"},
                {"CustomTextures", new Dictionary<string, object> {
                    { "box.png", "ground.jpg"}
                } },
                {"Name", "Ground"}
            }).Create();
        }
    }
}
