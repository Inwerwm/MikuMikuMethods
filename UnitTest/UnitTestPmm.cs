using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.PMM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTestPmm
    {
        [TestMethod]
        public void IOTest()
        {
            var pmm = new PolygonMovieMaker(TestData.GetPath("PragmaticTestData.pmm"));
        }

        [TestMethod]
        public void OldIO()
        {
            var pmm = new MikuMikuMethods.Binary.PMM.PolygonMovieMaker(TestData.GetPath("PragmaticTestData.pmm"));
        }
    }
}
