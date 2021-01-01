using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.VMD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTestVMD
    {
        [TestMethod]
        public void Test_VocaloidMotionData()
        {
            using (FileStream sourceFile = new FileStream(@"TestData\Number Nine.vmd", FileMode.Open))
            using (FileStream targetFile = new(@"TestData\result.vmd", FileMode.OpenOrCreate))
            using (BinaryReader reader = new(sourceFile))
            using (BinaryWriter writer = new(targetFile))
            {
                VocaloidMotionData vmd = new(reader);
                vmd.Write(writer);
            }
        }
    }
}
