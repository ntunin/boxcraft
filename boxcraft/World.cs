using D3DX;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    class World
    {
        public Prefab prefab;
        public List<Box> boxes;

        private List<Vector3> neighborhudOffsets = new List<Vector3>
        {
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
        };

        public World()
        {
            prefab = new Prefab(new Body(new Vector3(), new Vector3(), null), 
                                null, null, null);
            boxes = new List<Box>(); 
        }

        public void Add(Box box)
        {
            boxes.Add(box);
            prefab.AddChild(box.prefab);
        }

        public void Remove(Box box)
        {
            boxes.Remove(box);
            prefab.RemoveChild(box.prefab);
        }

        public void Rotate(float teta, float fi)
        {
            Vector3 dir = new Vector3((float)Math.Cos(fi), 0, (float)Math.Sin(fi));
            Matrix rotation = Matrix.RotationAxis(dir, teta) * Matrix.RotationY(fi);
            prefab.Body.FinalTransform = rotation;
        }

        public void Translate(float dx, float dy, float dz)
        {
            prefab.Body.Position.X += dx;
            prefab.Body.Position.Y += dy;
            prefab.Body.Position.Z += dz;
        }

        public Box RayCast(Ray ray)
        {
            Box raycastedBox= null;
            List<Box> raycastedBoxes = new List<Box>();
            foreach (Box box in boxes)
            {
                Vector3 position = box.prefab.GetGlobalPosition();
                if (box.prefab.Bound.RayCast(ray, position))
                {
                    raycastedBoxes.Add(box);
                }
            }
            Box nearest = null;
            float minDistance = 1e8f;
            foreach (Box box in raycastedBoxes)
            {
                float distance = new Vector3(box.prefab.Body.Position.X - ray.Position.X,
                                               box.prefab.Body.Position.Y - ray.Position.Y,
                                               box.prefab.Body.Position.Z - ray.Position.Z).Length();
                if (distance < minDistance)
                {
                    nearest = box;
                    minDistance = distance;
                }
            }
            if (nearest != null)
            {
                raycastedBox = nearest;
            }
            
            return raycastedBox;
        }

        public Vector3 RaycastToNearestPoint(Ray ray, Box box)
        {
            Prefab prefab = box.prefab;
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
