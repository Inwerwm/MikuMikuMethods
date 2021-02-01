using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods;
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
            using (BinaryReader reader = new(new FileStream(@"TestData\interpolateTest.vmd", FileMode.Open)))
            using (BinaryWriter writer = new(new FileStream(@"TestData\interpolateTest_Result.vmd", FileMode.OpenOrCreate)))
            {
                VocaloidMotionData vmd = new(reader);
                vmd.Write(writer);

                var curves = vmd.MotionFrames[0].InterpolationCurves;

                var xCurve = curves[InterpolationItem.XPosition];
                var yCurve = curves[InterpolationItem.YPosition];
                var zCurve = curves[InterpolationItem.ZPosition];
                var rCurve = curves[InterpolationItem.Rotation];

                Assert.AreEqual(0x00, xCurve.EarlyControlePoint.X);
                Assert.AreEqual(0x7F, xCurve.EarlyControlePoint.Y);
                Assert.AreEqual(0x7F, xCurve.LateControlePoint.X);
                Assert.AreEqual(0x7F, xCurve.LateControlePoint.Y);

                Assert.AreEqual(0x7F, yCurve.EarlyControlePoint.X);
                Assert.AreEqual(0x7F, yCurve.EarlyControlePoint.Y);
                Assert.AreEqual(0x00, yCurve.LateControlePoint.X);
                Assert.AreEqual(0x7F, yCurve.LateControlePoint.Y);

                Assert.AreEqual(0x7F, zCurve.EarlyControlePoint.X);
                Assert.AreEqual(0x7F, zCurve.EarlyControlePoint.Y);
                Assert.AreEqual(0x7F, zCurve.LateControlePoint.X);
                Assert.AreEqual(0x00, zCurve.LateControlePoint.Y);

                Assert.AreEqual(0x7F, rCurve.EarlyControlePoint.X);
                Assert.AreEqual(0x00, rCurve.EarlyControlePoint.Y);
                Assert.AreEqual(0x7F, rCurve.LateControlePoint.X);
                Assert.AreEqual(0x7F, rCurve.LateControlePoint.Y);
            }

            using (BinaryReader reader = new(new FileStream(@"TestData\interpolateTest_Result.vmd", FileMode.Open)))
            {
                VocaloidMotionData vmd = new(reader);

                var curves = vmd.MotionFrames[0].InterpolationCurves;

                var xCurve = curves[InterpolationItem.XPosition];
                var yCurve = curves[InterpolationItem.YPosition];
                var zCurve = curves[InterpolationItem.ZPosition];
                var rCurve = curves[InterpolationItem.Rotation];

                Assert.AreEqual(0x00, xCurve.EarlyControlePoint.X);
                Assert.AreEqual(0x7F, xCurve.EarlyControlePoint.Y);
                Assert.AreEqual(0x7F, xCurve.LateControlePoint.X);
                Assert.AreEqual(0x7F, xCurve.LateControlePoint.Y);

                Assert.AreEqual(0x7F, yCurve.EarlyControlePoint.X);
                Assert.AreEqual(0x7F, yCurve.EarlyControlePoint.Y);
                Assert.AreEqual(0x00, yCurve.LateControlePoint.X);
                Assert.AreEqual(0x7F, yCurve.LateControlePoint.Y);

                Assert.AreEqual(0x7F, zCurve.EarlyControlePoint.X);
                Assert.AreEqual(0x7F, zCurve.EarlyControlePoint.Y);
                Assert.AreEqual(0x7F, zCurve.LateControlePoint.X);
                Assert.AreEqual(0x00, zCurve.LateControlePoint.Y);

                Assert.AreEqual(0x7F, rCurve.EarlyControlePoint.X);
                Assert.AreEqual(0x00, rCurve.EarlyControlePoint.Y);
                Assert.AreEqual(0x7F, rCurve.LateControlePoint.X);
                Assert.AreEqual(0x7F, rCurve.LateControlePoint.Y);

            }
        }
    }
}
