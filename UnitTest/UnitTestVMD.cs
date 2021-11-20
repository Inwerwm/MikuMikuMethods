using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods;
using MikuMikuMethods.Vmd;
using System.IO;

namespace UnitTest;

[TestClass]
public class UnitTestVMD
{
    [TestMethod]
    public void Test_IO()
    {
        var inPath = @"../../TestData/\testMotion";
        var outPath = @"../../TestData/\motionOutput";

        VocaloidMotionData origin = new(inPath + ".vmd");
        origin.Write(outPath + ".vmd");
        VocaloidMotionData writed = new(outPath + ".vmd");
    }

    [TestMethod]
    public void Test_FrameAdding()
    {
        VocaloidMotionData vmd = new();

        var mf = new VmdMotionFrame("テスト");

        vmd.AddFrame(mf);

        Assert.AreEqual(1, vmd.MotionFrames.Count);
    }

    [TestMethod]
    public void Test_MotionInterpolation()
    {
        {
            VocaloidMotionData vmd = new(@"../../TestData/\interpolateTest.vmd");
            vmd.Write(@"../../TestData/\interpolateTest_Result.vmd");

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

        {
            VocaloidMotionData vmd = new(@"../../TestData/\interpolateTest_Result.vmd");

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

    [TestMethod]
    public void Test_CameraInterpolation()
    {
        {
            VocaloidMotionData vmd = new(@"../../TestData/\cameraInterpolateTest.vmd");
            vmd.Write(@"../../TestData/\cameraInterpolateTest_Result.vmd");

            var curves = vmd.CameraFrames[0].InterpolationCurves;

            var xCurve = curves[InterpolationItem.XPosition];
            var yCurve = curves[InterpolationItem.YPosition];
            var zCurve = curves[InterpolationItem.ZPosition];
            var rCurve = curves[InterpolationItem.Rotation];
            var dCurve = curves[InterpolationItem.Distance];
            var aCurve = curves[InterpolationItem.ViewAngle];

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

            Assert.AreEqual(0x00, dCurve.EarlyControlePoint.X);
            Assert.AreEqual(0x00, dCurve.EarlyControlePoint.Y);
            Assert.AreEqual(0x7F, dCurve.LateControlePoint.X);
            Assert.AreEqual(0x7F, dCurve.LateControlePoint.Y);

            Assert.AreEqual(0x7F, aCurve.EarlyControlePoint.X);
            Assert.AreEqual(0x7F, aCurve.EarlyControlePoint.Y);
            Assert.AreEqual(0x00, aCurve.LateControlePoint.X);
            Assert.AreEqual(0x00, aCurve.LateControlePoint.Y);
        }

        {
            VocaloidMotionData vmd = new(@"../../TestData/\cameraInterpolateTest_Result.vmd");

            var curves = vmd.CameraFrames[0].InterpolationCurves;

            var xCurve = curves[InterpolationItem.XPosition];
            var yCurve = curves[InterpolationItem.YPosition];
            var zCurve = curves[InterpolationItem.ZPosition];
            var rCurve = curves[InterpolationItem.Rotation];
            var dCurve = curves[InterpolationItem.Distance];
            var aCurve = curves[InterpolationItem.ViewAngle];

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

            Assert.AreEqual(0x00, dCurve.EarlyControlePoint.X);
            Assert.AreEqual(0x00, dCurve.EarlyControlePoint.Y);
            Assert.AreEqual(0x7F, dCurve.LateControlePoint.X);
            Assert.AreEqual(0x7F, dCurve.LateControlePoint.Y);

            Assert.AreEqual(0x7F, aCurve.EarlyControlePoint.X);
            Assert.AreEqual(0x7F, aCurve.EarlyControlePoint.Y);
            Assert.AreEqual(0x00, aCurve.LateControlePoint.X);
            Assert.AreEqual(0x00, aCurve.LateControlePoint.Y);
        }
    }
}
