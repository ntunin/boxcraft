using boxcraft;
using ClientServer;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BoxCraftClient
{
    class BoxCraftClient : Client, BoxCraftSceneDelegate
    {
        public World world;

        private WorldSerializer serializer = new WorldSerializer();
        BoxCraftScene scene;

        public BoxCraftClient(string host, string username, BoxCraftScene scene)
        {
            this.scene = scene;
            scene.dlgt = this;
            Authenticate(new SocketConnection(host, 11020));
            Send($"<join>{username}|0;0;0|0;0;0</join>");
        }

        public override void Handle(string message)
        {
            XElement e = XElement.Parse(message);
            switch(e.Name.LocalName)
            {
                case "world":
                    LoadWorld(e);
                    break;
            }
        }

        public void onRightHandAction()
        {
        }

        public void onLeftHandAction()
        {
        }

        public void onMove(Vector3 position)
        {
        }

        public void onRotate(Vector3 rotation)
        {
        }

        private void LoadWorld(XElement e)
        {
            world = serializer.Deserialize(e.Value);
            scene.Load(world);
        }
    }
}
