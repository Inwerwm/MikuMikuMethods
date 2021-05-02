using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.PMM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
            testPmm.Read("TestData/testProject.pmm");
            testPmm.Write("TestData/output.pmm");

            var outPmm = new PolygonMovieMaker("TestData/output.pmm");

            //もとのPMMと書込読込PMMのインスタンスをシリアライズして
            //テキストで差分を見れるようにする
            Assert.AreEqual(testPmm.ToJson("TestData/originPmm.json"), outPmm.ToJson("TestData/outputPmm.json"));
        }
    }
}
