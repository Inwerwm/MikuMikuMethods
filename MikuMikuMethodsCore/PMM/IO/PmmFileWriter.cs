using MikuMikuMethods.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM.IO
{
    internal static class PmmFileWriter
    {
        internal static void Write(string filePath, PolygonMovieMaker pmm)
        {
            try
            {
                using (FileStream file = new(filePath, FileMode.Create))
                using (BinaryWriter writer = new(file, Encoding.ShiftJIS))
                {
                    Write(writer, pmm);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void Write(BinaryWriter writer, PolygonMovieMaker pmm)
        {
            writer.Write(pmm.Version, 30, Encoding.ShiftJIS);
            writer.Write(pmm.OutputResolution.Width);
            writer.Write(pmm.OutputResolution.Height);
        }
    }
}
