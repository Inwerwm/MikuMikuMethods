using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuMethods.PMM;
using MikuMikuMethods.PMM.Frame;

namespace UnitTest
{
    [TestClass]
    public class UnitTestPmm_Accessory
    {
        [TestMethod]
        public void Test_Asc_TransAndVisible()
        {
            var tv = PmmAccessoryFrame.SeparateTransAndVisible(45);
            var re = PmmAccessoryFrame.CreateTransAndVisible(tv.Transparency, tv.Visible);

            Assert.IsTrue(tv.Visible);
            Assert.AreEqual(0.78f, tv.Transparency);
            Assert.AreEqual(45, re);
        }
    }
}
