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
            IOCompare("../../TestData/test.pmx", "../../TestData/write.pmx");
            IOCompare("../../TestData/tex.pmx", "../../TestData/texW.pmx");
            IOCompare("../../TestData/ツインテ少女.pmx", "../../TestData/ツインテ少女_w.pmx");

            void IOCompare(string inPath, string outPath)
            {
                PmxModel m = new(inPath);
                m.Write(outPath);
                CompareBinary(inPath, outPath);
            }
        }

        private void CompareBinary(string filePath1, string filePath2)
        {
            var b1 = File.ReadAllBytes(filePath1);
            var b2 = File.ReadAllBytes(filePath2);

            if (b1.Length != b2.Length)
                Assert.Fail("バイナリデータの長さが違います。");

            foreach (var b in b1.Zip(b2).Select((Value, Index) => (Value, Index)))
            {
                Assert.AreEqual(b.Value.First, b.Value.Second, $"{b.Index}番目で異なる値が発見されました。");
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
