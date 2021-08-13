using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.PMX;
using MikuMikuMethods.PMX.IO;
using System;
using System.Collections.Generic;
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
            PmxModel model = new("../../TestData/test.pmx");
            model.Write("../../TestData/write.pmx");
            PmxModel writed = new("../../TestData/write.pmx");

            PmxModel txModel = new("../../TestData/tex.pmx");
            txModel.Write("../../TestData/texW.pmx");
            PmxModel texW = new("../../TestData/texW.pmx");
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
