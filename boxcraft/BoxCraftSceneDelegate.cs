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
        void onRotate(Vector3 rotation);
        void onMove(Vector3 position);
        void onLeftHandAction();
        void onRightHandAction();
    }
}
