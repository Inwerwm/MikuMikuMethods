using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.PMM;
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
            testPmm.Read("../../TestData/testProject.pmm");
            testPmm.Write("../../TestData/output.pmm");

            var outPmm = new PolygonMovieMaker("../../TestData/output.pmm");

            //もとのPMMと書込読込PMMのインスタンスをシリアライズして
            //テキストで差分を見れるようにする
            
            Assert.AreEqual(JsonSerializer.Serialize(testPmm), JsonSerializer.Serialize(outPmm));
        }
    }
}
