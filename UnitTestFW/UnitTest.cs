using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods;
using MikuMikuMethods.MME;
using System;
using System.IO;
using System.Text;

namespace UnitTestFW
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Test_MME()
        {
            using(var file = new FileStream(@"C:\MMD\_Programs\MikuMikuMethods\UnitTestFW\TestData\test.emm", FileMode.Open))
            using(var reader = new StreamReader(file,Encoding.GetEncoding("shift_jis")))
            {
                var emm = new EMMData(reader);

            }


        }
    }
}
