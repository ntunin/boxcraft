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
        private World world;
        private float teta;
        private float fi;
        private BoxFactory boxFactory = new BoxFactory();

        public float speed = 1;

        public BoxCraftScene(Control control) : base(control)
        {
        }

        public void PerformRightHandAction()
        {
            Ray ray = GetCameraRay();
            Box box = world.RayCast(ray);
            if(box == null)
            {
                return;
            }
            Vector3 position = world.RaycastToNearestPoint(ray, box);
            Box newBox = boxFactory.CreateBox("ground", position);
            world.Add(newBox);
        }

        public void PerformLeftHandAction()
        {
            Ray ray = GetCameraRay();
            Box box = world.RayCast(ray);
            if(box == null)
            {
                return;
            }
            world.Remove(box);
        }

        public void Rotate(float delaX, float delaY)
        {
            teta += delaY;
            fi += delaX;
            world.Rotate(teta, fi);
        }

        public void MoveToward(float angle)
        {
            Vector3 direction = new Vector3((float)Math.Cos(fi + angle + Math.PI/2), 
                                            0, 
                                            (float)Math.Sin(fi + angle + Math.PI / 2));
            world.Translate(speed * direction.X, 0, speed * direction.Z);
        }

        public void MoveVertical(float deltaY)
        {
            world.Translate(0, speed * deltaY, 0);
        }

        protected override Camera CreateCamera(Control control)
        {
            return new PerspectiveCamera((float)Math.PI / 4, control, 1f, 1000f, new Vector3(0, 0, 0), new Vector3(0, 0, -1), new Vector3(0, 1, 0));
        }

        protected override void AddLight()
        {
            D3DX.Light light1 = new D3DX.Light(new Dictionary<string, object> {
                {"Type", LightType.Point},
                {"Position", new Vector3(0, 10, 0) },
                {"Diffuse", Color.White },
                {"Attenuation", 1f },
                {"Range", 100f }
            });
            light.Add(light1);
        }

        protected override void AddPrefabs()
        {
            new SkinLoader().Load();
            world = new WorldGenarator().Generate();
            prefabs.Add(world.prefab);
        }
        
        private Ray GetCameraRay()
        {
            float teta1 = (float)(Math.PI / 2 - teta);
            float dy = -(float)(Math.Cos(teta1));
            float dx = (float)(Math.Sin(fi)*Math.Sin(teta1));
            float dz = -(float)(Math.Cos(fi)*Math.Sin(teta1));
            return new Ray(new Vector3(dx, dy, dz));

        }
    }
}
