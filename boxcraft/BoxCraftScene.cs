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
    public class BoxCraftScene : Scene
    {
        private World world;
        private float teta;
        private float fi;
        public BoxCraftSceneDelegate dlgt;

        public string selectedBoxType = "ground";
        public float speed = 1;
        private Sprite crossSprite;
        private Texture crossTexture;
        private Point crossPosition;

        public BoxCraftScene(Control control) : base(control)
        {
            InitCross();
        }

        public void Load(World world)
        {
            this.world = world;
            prefabs.Add(world.prefab);
        }

        public void PerformRightHandAction()
        {
            if (world == null)
            {
                return;
            }
            Ray ray = GetCameraRay();
            Box box = world.RayCast(ray);
            if(box == null)
            {
                return;
            }
            Vector3 position = world.RaycastToNearestPoint(ray, box);
            Box newBox = BoxFactory.CreateBox(selectedBoxType, position);
            world.Add(newBox);
            if (dlgt != null)
            {
                dlgt.OnCreateBox(newBox);
            }
        }

        public void PerformLeftHandAction()
        {
            if (world == null)
            {
                return;
            }
            Ray ray = GetCameraRay();
            Box box = world.RayCast(ray);
            if(box == null)
            {
                return;
            }
            world.Remove(box);
            if (dlgt != null)
            {
                dlgt.OnRemoveBox(box);
            }
        }

        public void Rotate(float delaX, float delaY)
        {
            if (world == null)
            {
                return;
            }
            teta += delaY;
            fi += delaX;
            world.Rotate(teta, fi);

            if (dlgt != null)
            {
                dlgt.OnRotate(new Vector3(-teta, -fi, 0));
            }
        }

        public void MoveToward(float angle)
        {
            if (world == null)
            {
                return;
            }
            Vector3 direction = GetUserDirection(angle);
            world.Translate(speed * direction.X, 0, speed * direction.Z);

            if (dlgt != null)
            {
                dlgt.OnMove(-world.prefab.Body.Position);
            }
        }

        private Vector3 GetUserDirection(float angle)
        {
            return new Vector3((float)Math.Cos(fi + angle + Math.PI / 2),
                                            0,
                                            (float)Math.Sin(fi + angle + Math.PI / 2));
        }

        public void MoveVertical(float deltaY)
        {
            if (world == null)
            {
                return;
            }
            world.Translate(0, speed * deltaY, 0);
            if (dlgt != null)
            {
                dlgt.OnMove(-world.prefab.Body.Position);
            }
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
            //prefabs.Add(CreateCross());
        }

        private Prefab CreateCross()
        {
            Prefab prefab = (Prefab)new PrefabBuilder(new Dictionary<string, object> {
                {"Body", new Dictionary<string, object>{
                    {"Position", new Dictionary<string, object>{
                        {"X", $"{0}"},
                        {"Y", $"{0}"},
                        {"Z", $"{1}"}
                    }}
                }},
                {"Skin", "Cross" },
            }).Create();
            return prefab;
        }
        
        private Ray GetCameraRay()
        {
            float teta1 = (float)(Math.PI / 2 - teta);
            float dy = -(float)(Math.Cos(teta1));
            float dx = (float)(Math.Sin(fi)*Math.Sin(teta1));
            float dz = -(float)(Math.Cos(fi)*Math.Sin(teta1));
            return new Ray(new Vector3(dx, dy, dz));
        }

        public override void Present(float deltaTime)
        {
            base.Present(deltaTime);
            DrawCross();
        }

        private void InitCross()
        {
            var device = SceneContext.Shared.Device;
            device.RenderState.SourceBlend = Blend.SourceAlpha;
            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;
            device.RenderState.AlphaBlendEnable = true;
            crossSprite = new Sprite(device);
            crossTexture = TextureLoader.FromFile(device, "cross.png", 16, 16, 0, Usage.None, Format.A8B8G8R8, Pool.Managed, Filter.None, Filter.None, Color.White.ToArgb());
            crossPosition = new Point(control.Width / 2, (int)(control.Height * 0.85 / 2));
        }

        private void DrawCross()
        {
            crossSprite.Begin(SpriteFlags.None);
            crossSprite.Draw2D(crossTexture, Point.Empty, 0, crossPosition, Color.White);
            crossSprite.End();

        }
    }
}
