using D3DX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    public class SkinLoader
    {

        public void Load()
        {
            new SkinBuilder(new Dictionary<string, object>
            {
                {"File", "box.X"}
            }).Create();

            LoadBox("Ground", "ground.jpg");
            LoadBox("Stone", "stone.png");
            LoadBox("Bricks", "bricks.png");
            new SkinBuilder(new Dictionary<string, object>
            {
                {"File", "cross.X"},
                {"Name", "Cross"}
            }).Create();
        }

        private void LoadBox(string name, string texture)
        {
            new SkinBuilder(new Dictionary<string, object>
            {
                {"File", "box.X"},
                {"CustomTextures", new Dictionary<string, object> {
                    { "box.png", texture}
                } },
                {"Name", name}
            }).Create();
        }

    }
}
