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
            LoadBox("Wood", "wood.jpg");

            LoadUser("Nik", "ntunin.png");
            LoadUser("Cam", "cam.png");
            LoadUser("Asel", "asel.png");
            LoadUser("Nastya", "nastya.png");
            LoadUser("Alexey", "alexey.png");
            LoadUser("Slava", "slava.png");
            LoadUser("Victor", "victor.png");
            LoadUser("Nikita M", "nminin.png");
            LoadUser("Nikita Ch", "nchubarov.png");
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

        private void LoadUser(string name, string texture)
        {
            new SkinBuilder(new Dictionary<string, object>
            {
                {"File", "boxface.X"},
                {"CustomTextures", new Dictionary<string, object> {
                    { "ntunin.png", texture}
                } },
                {"Name", name}
            }).Create();
        }
    }
}
