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
        PolygonMovieMaker pmm = new();

        private void LoadTestData()
        {
            using (FileStream stream = new("TestData/testProject.pmm", FileMode.Open))
            using (BinaryReader reader = new(stream, MikuMikuMethods.Encoding.ShiftJIS))
            {
                pmm.Read(reader);
            }
        }

        public UnitTestPMM()
        {
            LoadTestData();
        }

        [TestMethod]
        public void Test_PolygonMovieMaker()
        {
            Assert.AreEqual("Polygon Movie maker 0002", pmm.Version);
            Assert.IsTrue(pmm.EditorState.IsCameraMode);
            Assert.AreEqual(1, pmm.Models.Count);
        }

        [TestMethod]
        public void Test_PmmModel()
        {
            Assert.AreEqual("ルベシア・シェリングヴェーヌ", pmm.Models[0].Name);
            Assert.AreEqual("Rybecia Sherringvaine", pmm.Models[0].NameEn);
            Assert.AreEqual(@"C:\MMD\_モデル(人物)\_quappa-el\EL-D2M-Rybecia_Shader\ルベシア.pmx", pmm.Models[0].Path);
            Assert.AreEqual(16, pmm.Models[0].FrameEditor.RowCount);
            Assert.IsNull(pmm.Models[0].InitialBoneFrames[0].Index);
            Assert.AreEqual(1, pmm.Models[0].InitialBoneFrames[0].Offset.Y);
        }
    }
}
