using boxcraft;
using ClientServer;
using D3DX;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BoxCraftServer
{
    public class BoxCraftServer : Server
    {
        World world;
        WorldSerializer serializer = new WorldSerializer();
        SavingService savingService = new SavingService();

        public BoxCraftServer()
        {
            var form = new Form1(); // To primary device initializtion
            new SkinLoader().Load();
            if (savingService.HasSave())
            {
                world = savingService.Load();
            }
            else
            {
                world = new WorldGenarator().Generate();
            }
        }

        public override void HandleRequest(string request, Pipe pipe)
        {
            XElement e = XElement.Parse(request);
            switch(e.Name.LocalName)
            {
                case "join":
                    Join(e, pipe);
                    break;
            }

        }

        private void Join(XElement e, Pipe pipe)
        {
            var user = serializer.DeserializeUser(e.Value);
            world.Add(user);
            var response = $"<world>{serializer.Serialize(world)}</world>";
            pipe.SendMessage(response);
        }
    }
}
