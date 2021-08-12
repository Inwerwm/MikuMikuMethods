using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.PMX;
using MikuMikuMethods.PMX.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTestPMX
    {
        [TestMethod]
        public void PmxIOTest()
        {
            PmxFileReader.ReadModel("../../TestData/test.pmx");
        }
    }
}
