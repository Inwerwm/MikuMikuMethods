using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.PMM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTestPMM
    {
        PolygonMovieMaker testPmm;

        public UnitTestPMM()
        {
            testPmm = new();

        }

        [TestMethod]
        public void Test_IO()
        {
            using (FileStream stream = new("TestData/testProject.pmm", FileMode.Open))
            using (BinaryReader reader = new(stream, MikuMikuMethods.Encoding.ShiftJIS))
            using (FileStream outStream = new("TestData/output.pmm", FileMode.Create))
            using (BinaryWriter writer = new(stream, MikuMikuMethods.Encoding.ShiftJIS))
            {
                testPmm.Read(reader);
                testPmm.Write(writer);

                // 比較のため巻き戻し
                reader.BaseStream.Position = 0;
                writer.BaseStream.Position = 0;

                using(BinaryReader outReader = new(outStream, MikuMikuMethods.Encoding.ShiftJIS))
                {
                    Assert.AreEqual(reader.BaseStream.Length, outReader.BaseStream.Length);

                    for (int i = 0; reader.BaseStream.Position < reader.BaseStream.Length; i++)
                    {
                        Assert.AreEqual(reader.ReadByte(), outReader.ReadByte());
                    }
                }
            }
        }
    }
}
