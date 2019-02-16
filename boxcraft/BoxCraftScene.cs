using D3DX;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace boxcraft
{
    class BoxCraftScene : Scene
    {

        private D3DX.Light light1;
        private Prefab landscape;
        private Prefab tank;
        private Prefab world;
        private float teta;
        private float fi;

        public BoxCraftScene(Control control) : base(control)
        {
        }

        protected override Camera CreateCamera(Control control)
        {
            return new PerspectiveCamera((float)Math.PI / 4, control, 0.1f, 1000f, new Vector3(0, 0, 0), new Vector3(0, 0, -1), new Vector3(0, 1, 0));
        }

        protected override void AddLight()
        {
            light1 = new D3DX.Light(new Dictionary<string, object> {
                {"Type", LightType.Point},
                {"Position", new Vector3(0, 100, 0) },
                {"Diffuse", Color.White },
                {"Attenuation", 0.6f },
                {"Range", 10000f }
            });
            light.Add(light1);
        }

        protected override void AddPrefabs()
        {
            var landscapeSkin = (Skin)new SkinBuilder(new Dictionary<string, object> {
                { "Name", "Landscape" },
                { "File", "teapot.X" },
                { "MeshFlags", "Managed" },
            }).Create();

            landscape = (Prefab)new PrefabBuilder(new Dictionary<string, object> {
                {"Name", "Landscape"},
                {"Body", new Dictionary<string, object>{
                    {"Position", new Dictionary<string, object>{
                        {"X", "0"},
                        {"Y", "0"},
                        {"Z", "-500"}
                    }}
                }},
                {"Skin", "Landscape" }
            }).Create();
            //tank = (Prefab)new PrefabBuilder(new Dictionary<string, object> { }).Create();

            world = (Prefab)new PrefabBuilder(new Dictionary<string, object> {
                {"Name", "World"},
                {"Body", new Dictionary<string, object>{}},
                {"Children", new List<object> {
                    "Landscape"
                } }
            }).Create();
            prefabs.Add(world);
        }

    }
}
