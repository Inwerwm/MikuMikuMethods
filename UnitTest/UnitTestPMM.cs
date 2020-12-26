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
            PolygonMovieMaker pmm = new();
            using(FileStream stream = new("TestData/testProject.pmm", FileMode.Open))
            using(BinaryReader reader = new(stream, MikuMikuMethods.Encoding.ShiftJIS))
            {
                pmm.Read(reader);
            }

            Assert.AreEqual("Polygon Movie maker 0002", pmm.Version);
            Assert.IsTrue(pmm.EditorState.IsCameraMode);
            Assert.AreEqual(1, pmm.EditorState.ModelCount);
        }
    }
}
