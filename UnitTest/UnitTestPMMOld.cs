using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.Binary.PMM;
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
    public class UnitTestPMMOld
    {
        string testDir = "../../TestData/";

        [TestMethod]
        public void Test_IO()
        {
            var srcPmm = new PolygonMovieMaker(testDir + "testProject.pmm");
            srcPmm.Write(testDir + "output.pmm");
            var outPmm = new PolygonMovieMaker(testDir + "output.pmm");

            //もとのPMMと書込読込PMMのインスタンスをシリアライズして
            //テキストで差分を見れるようにする
            Assert.AreEqual(JsonSerializer.Serialize(srcPmm), JsonSerializer.Serialize(outPmm));
        }

        [TestMethod]
        public void Test_IO2()
        {
            var src = new PolygonMovieMaker(testDir + "testLight.pmm");
            src.Write(testDir + "outputLight.pmm");
            var output = new PolygonMovieMaker(testDir + "outputLight.pmm");

            Assert.AreEqual(JsonSerializer.Serialize(src), JsonSerializer.Serialize(output));
        }

        [TestMethod]
        public void Test_ViewFollowCamera()
        {
            var maybeFalse = new PolygonMovieMaker(testDir + "ViewFollowCamera_MaybeFalse.pmm");
            var maybeTrue = new PolygonMovieMaker(testDir + "ViewFollowCamera_MaybeTrue.pmm");

            Assert.IsFalse(maybeFalse.ViewConfig.IsViewFollowCamera);
            Assert.IsTrue(maybeTrue.ViewConfig.IsViewFollowCamera);
        }
    }
}
