﻿using D3DX;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    class BoxFactory
    {
        private Dictionary<string, object> skinConfig = new Dictionary<string, object>
        {
            {"ground", new Dictionary<string, object> {
                {"name", "Ground" }
            } }
        };

        public Box CreateBox(string type, Vector3 position)
        {
            Dictionary<string, object> configs = (Dictionary<string, object>)skinConfig[type];
            if (configs == null)
            {
                return null;
            }
            string skinName = (string)configs["name"];
            Prefab prefab = (Prefab)new PrefabBuilder(new Dictionary<string, object> {
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
            return new Box(type, prefab);
        }
    }
}
