using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    public class PmxBinaryReader
    {
        public static PmxModel ReadModel(string filePath)
        {
            using (FileStream stream = new(filePath, FileMode.Open))
            using (BinaryReader reader = new(stream))
            {
                var model = new PmxModel();



                return model;
            }
        }
    }
}
