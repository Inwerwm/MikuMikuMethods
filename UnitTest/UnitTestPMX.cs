using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.PMX;
using MikuMikuMethods.PMX.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTestPMX
    {
        [TestMethod]
        public void PmxIOTest()
        {
            const string simpleI = "../../TestData/test.pmx";
            const string simpleO = "../../TestData/write.pmx";
            PmxModel model = new(simpleI);
            model.Write(simpleO);
            CompareBinary(simpleI, simpleO);

            const string texI = "../../TestData/tex.pmx";
            const string texO = "../../TestData/texW.pmx";
            PmxModel txModel = new(texI);
            txModel.Write(texO);
            CompareBinary(texI, texO);
        }

        private void CompareBinary(string filePath1, string filePath2)
        {
            var b1 = File.ReadAllBytes(filePath1);
            var b2 = File.ReadAllBytes(filePath2);

            if (b1.Length != b2.Length)
                Assert.Fail("バイナリデータの長さが違います。");

            foreach (var b in b1.Zip(b2))
            {
                Assert.AreEqual(b.First, b.Second);
            }
        }

        [TestMethod]
        public void PmxSharedToonTextureTest()
        {
            var path = new PmxTexture("toon05.bmp");
            var num = new PmxTexture(5);
            var notShared = new PmxTexture("tex/toon05.bmp");

            Assert.AreEqual((byte)5, path.ToonIndex);
            Assert.AreEqual("toon05.bmp", num.Path);
            Assert.IsNull(notShared.ToonIndex);
            Assert.AreEqual(1, new[] { path, num }.Distinct().Count());
        }
    }
}
