using D3DX;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    class LandscapeGenerator
    {
        private Dictionary<string, Prefab> map;
        private Dictionary<string, object> skinConfig = new Dictionary<string, object>
        {
            {"ground", new Dictionary<string, object> {
                {"name", "Ground" }
            } }
        };

        public List<Prefab> Generate()
        {
            map = new Dictionary<string, Prefab>();
            GeneratePlane("ground", new Vector3(0, -2, 0), 10, 10);
            GenerateSphere("ground", new Vector3(0, -2, -10), 5);
            return UnwrapMap();
        }

        public void GeneratePlane(string type, Vector3 center, int width, int height)
        {
            float distance = 1f;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector3 position = new Vector3(distance * (j - height / 2) + center.X,
                                                        center.Y,
                                                       distance * (i - height / 2) + center.Z);
                    Prefab prefab = CreatePrefab(type, position);
                    map[position.ToString()] = prefab;
                }
            }
        }

        public void GenerateSphere(string type, Vector3 center, int radius)
        {
            float distance = 1f;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    for (int z = -radius; z <= radius; z++)
                    {
                        if(Math.Abs(x*x + y*y + z*z - radius*radius) < 5)
                        {
                            Vector3 position = new Vector3(distance * x + center.X,
                                                           distance * y + center.Y,
                                                           distance * z + center.Z);
                            Prefab prefab = CreatePrefab(type, position);
                            map[position.ToString()] = prefab;

                        }
                    }
                }
            }
        }

        private List<Prefab> UnwrapMap()
        {
            List<Prefab> prefabs = new List<Prefab>();
            foreach(string key in map.Keys)
            {
                prefabs.Add(map[key]);
            }
            return prefabs;
        }

        public Prefab CreatePrefab(string type, Vector3 position)
        {
            Dictionary<string, object> configs = (Dictionary<string, object>)skinConfig[type];
            if (configs == null)
            {
                return null;
            }
            string skinName = (string)configs["name"];
            return (Prefab)new PrefabBuilder(new Dictionary<string, object> {
                {"Body", new Dictionary<string, object>{
                    {"Position", new Dictionary<string, object>{
                        {"X", $"{position.X}"},
                        {"Y", $"{position.Y}"},
                        {"Z", $"{position.Z}"}
                    }}
                }},
                {"Skin", skinName },
                {"Bound", new Dictionary<string, object> {
                    { "Type", "Box" },
                    { "Width", "1" },
                    { "Height", "1" },
                    { "Depth", "1" },
                } }
            }).Create();
        }
    }
}
