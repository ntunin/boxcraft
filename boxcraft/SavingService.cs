using D3DX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boxcraft
{
    public class SavingService
    {

        string filename = "boxcraft.save";

        public void Save(World world)
        {
            var str = WorldSerializer.Serialize(world);
            var writer = new StreamWriter(filename);
            writer.Write(str);
            writer.Close();
        }

        public World Load()
        {
            var reader = new StreamReader(filename);
            var str = reader.ReadToEnd();
            var world = WorldSerializer.Deserialize(str);
            reader.Close();
            return world;
        }

        public bool HasSave()
        {
            return new FileInfo(filename).Exists;
        }
        
    }
}
