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
        private Prefab world;
        private float teta;
        private float fi;
        private Vector3 dir;
        private Matrix rotation;
        private LandscapeGenerator landscapeGenerator;
        private List<Vector3> neighborhudOffsets = new List<Vector3>
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
        };

        public float speed = 1;

        public BoxCraftScene(Control control) : base(control)
        {
        }

        public void PerformRightHandAction()
        {
            Ray ray = GetCameraRay();
            Prefab prefab = world.RayCast(ray);
            if(prefab == null)
            {
                return;
            }
            Vector3 position = RaycastToNearestPoint(ray, prefab);
            Prefab newPrefab = landscapeGenerator.CreatePrefab("ground", position);
            world.AddChild(newPrefab);
        }

        public void PerformLeftHandAction()
        {
            Ray ray = GetCameraRay();
            Prefab prefab = world.RayCast(ray);
            if(prefab == null)
            {
                return;
            }
            world.RemoveChild(prefab);
        }

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
            LoadSkins();
            CreateWorld();
            GenerateLandscape();
        }

        private void LoadSkins()
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

        private void CreateWorld()
        {
            world = (Prefab)new PrefabBuilder(new Dictionary<string, object> {
                {"Name", "World"},
                {"Body", new Dictionary<string, object>{}}
            }).Create();
            prefabs.Add(world);
        }

        private void GenerateLandscape()
        {
            landscapeGenerator = new LandscapeGenerator();
            List<Prefab> prefabs = landscapeGenerator.Generate();
            foreach (Prefab prefab in prefabs)
            {
                world.AddChild(prefab);
            }
        }
        
        private Ray GetCameraRay()
        {
            Vector3 direction = new Vector3((float)Math.Cos(fi + Math.PI / 2),
                                            (float)Math.Sin(teta),
                                            (float)Math.Sin(fi + Math.PI / 2));
            return new Ray(direction);

        }

        private Vector3 RaycastToNearestPoint(Ray ray, Prefab prefab)
        {
            Shape bound = prefab.Bound;
            Vector3 globalPosition = prefab.GetGlobalPosition();
            Vector3 nearestOffset = new Vector3();
            float minDistance = 1e10f;
            foreach (Vector3 offset in neighborhudOffsets)
            {
                Vector3 globalPoint = Vector3.Add(globalPosition, offset);
                if (bound.RayCast(ray, globalPoint))
                {
                    float distance = Vector3.Subtract(globalPoint, ray.Position).Length();
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestOffset = offset;
                    }
                }
            }
            return Vector3.Add(prefab.Body.Position, nearestOffset);
        }
    }
}
