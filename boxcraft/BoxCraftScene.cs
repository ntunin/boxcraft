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
        private Prefab world;
        private float teta;
        private float fi;
        private Vector3 dir;
        private Matrix rotation;
        
        public float speed = 1;
        
        public void Rotate(float delaX, float delaY)
        {
            teta += delaY;
            fi += delaX;
            dir = new Vector3((float)Math.Cos(fi), 0,(float)Math.Sin(fi));
            rotation=Matrix.RotationAxis(dir, teta) * Matrix.RotationY(fi);
            world.Body.FinalTransform = rotation;
        }

        public void MoveToward(float angle)
        {
            Vector3 direction = new Vector3((float)Math.Cos(fi + angle + Math.PI/2), 0, (float)Math.Sin(fi + angle + Math.PI / 2));
            world.Body.Position.X += speed * direction.X;
            world.Body.Position.Z += speed * direction.Z;
        }

        public void MoveVertical(float deltaY)
        {
            world.Body.Position.Y += speed * deltaY;
        }

        public BoxCraftScene(Control control) : base(control)
        {
        }

        protected override Camera CreateCamera(Control control)
        {
            return new PerspectiveCamera((float)Math.PI / 4, control, 0.001f, 1000f, new Vector3(0, 0, 0), new Vector3(0, 0, -1), new Vector3(0, 1, 0));
        }

        protected override void AddLight()
        {
            light1 = new D3DX.Light(new Dictionary<string, object> {
                {"Type", LightType.Point},
                {"Position", new Vector3(0, -100, 0) },
                {"Diffuse", Color.White },
                {"Attenuation", 0.0f },
                {"Range", 10000f }
            });
            light.Add(light1);
        }

        protected override void AddPrefabs()
        {
            loadSkins();
            createWorld();
        }

        private void loadSkins()
        {
            new SkinBuilder(new Dictionary<string, object> {
                { "Name", "Ground" },
                { "File", "teapot.X" },
            }).Create();
        }

        private void createWorld()
        {
            world = (Prefab)new PrefabBuilder(new Dictionary<string, object> {
                {"Name", "World"},
                {"Body", new Dictionary<string, object>{}}
            }).Create();
            prefabs.Add(world);
        }

        private Prefab createPrefab(String skinName, Vector3 position)
        {
            return (Prefab)new PrefabBuilder(new Dictionary<string, object> {
                {"Body", new Dictionary<string, object>{
                    {"Position", new Dictionary<string, object>{
                        {"X", $"{position.X}"},
                        {"Y", $"{position.Y}"},
                        {"Z", $"{position.Z}"}
                    }}
                }},
                {"Skin", skinName }
            }).Create();
        }


    }
}
