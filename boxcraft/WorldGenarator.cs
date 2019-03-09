using D3DX;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    class WorldGenarator
    {
        private BoxFactory boxFactory = new BoxFactory();

        public World Generate()
        {
            World world = new World();
            Dictionary<string, Box> map = new Dictionary<string, Box>();
            PutIntoMap(map, GeneratePlane("ground", new Vector3(0, -2, 0), 10, 10));
            PutIntoMap(map, GenerateSphere("ground", new Vector3(0, -2, -10), 5));
            UnwrapMap(map, world);
            return world;
        }

        public List<Box> GeneratePlane(string type, Vector3 center, int width, int height)
        {
            List<Box> list = new List<Box>();
            float distance = 1f;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector3 position = new Vector3(distance * (j - width * 0.5f) + center.X + 0.5f,
                                                        center.Y + 0.5f,
                                                       distance * (i - height * 0.5f) + center.Z + 0.5f);
                    Box box = boxFactory.CreateBox(type, position);
                    list.Add(box);
                }
            }
            return list;
        }

        public List<Box> GenerateSphere(string type, Vector3 center, int radius)
        {
            List<Box> list = new List<Box>();
            float distance = 1f;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    for (int z = -radius; z <= radius; z++)
                    {
                        if (Math.Abs(x * x + y * y + z * z - radius * radius) < 5)
                        {
                            Vector3 position = new Vector3(distance * x + center.X + 0.5f,
                                                           distance * y + center.Y + 0.5f,
                                                           distance * z + center.Z + 0.5f);
                            Box box = boxFactory.CreateBox(type, position);
                            list.Add(box);
                        }
                    }
                }
            }
            return list;
        }

        private void PutIntoMap(Dictionary<string, Box> map, List<Box> list)
        {
            foreach (Box box in list)
            {
                map[box.prefab.Body.Position.ToString()] = box;
            }
        }

        private void UnwrapMap(Dictionary<string, Box> map, World world)
        {
            foreach (string key in map.Keys)
            {
                world.Add(map[key]);
            }
        }
    }
}
