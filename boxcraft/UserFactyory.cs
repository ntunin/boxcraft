using D3DX;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    public class UserFactyory
    {
        public static User CreateUser(string name, Vector3 position, Vector3 rotation)
        {
            var record = DB.Users.Where((user) => { return user.Name.Equals(name); }).First();
            var prefab = (Prefab)new PrefabBuilder(new Dictionary<string, object> {
                {"Body", new Dictionary<string, object>{
                    {"Rotation", new Dictionary<string, object> {
                        {"X", $"{rotation.X}"},
                        {"Y", $"{rotation.Y}"},
                        {"Z", $"{rotation.Z}"}
                    }},
                    {"Position", new Dictionary<string, object>{
                        {"X", $"{position.X}"},
                        {"Y", $"{position.Y}"},
                        {"Z", $"{position.Z}"}
                    }}
                }},
                {"Skin", record.Skin }
            }).Create();
            //prefab.Body.StartTransform = Matrix.RotationY((float)Math.PI / 2);
            return new User(name, prefab);
        }
    }
}
