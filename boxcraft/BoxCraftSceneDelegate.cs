using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    public interface BoxCraftSceneDelegate
    {
        void OnRotate(Vector3 rotation);
        void OnMove(Vector3 position);
        void OnRemoveBox(Box box);
        void OnCreateBox(Box box);
    }
}
