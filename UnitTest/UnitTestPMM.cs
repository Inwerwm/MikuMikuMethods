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
        [TestMethod]
        public void Test_PolygonMovieMaker()
        {
            using(FileStream stream = new("TestData/testProject.pmm", FileMode.Open))
            using(BinaryReader reader = new(stream, MikuMikuMethods.Encoding.ShiftJIS))
            {
                PolygonMovieMaker pmm = new();
                pmm.Read(reader);
            }
        }
    }
}
