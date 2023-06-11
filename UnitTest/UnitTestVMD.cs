using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods;
using MikuMikuMethods.Vmd;

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

            Assert.AreEqual(0x00, xCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, xCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, xCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, xCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, yCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, yCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x00, yCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, yCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, zCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, zCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, zCurve.LateControlPoint.X);
            Assert.AreEqual(0x00, zCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, rCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x00, rCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, rCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, rCurve.LateControlPoint.Y);
        }

        {
            VocaloidMotionData vmd = new(@"../../TestData/\interpolateTest_Result.vmd");

            var curves = vmd.MotionFrames[0].InterpolationCurves;

            var xCurve = curves[InterpolationItem.XPosition];
            var yCurve = curves[InterpolationItem.YPosition];
            var zCurve = curves[InterpolationItem.ZPosition];
            var rCurve = curves[InterpolationItem.Rotation];

            Assert.AreEqual(0x00, xCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, xCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, xCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, xCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, yCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, yCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x00, yCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, yCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, zCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, zCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, zCurve.LateControlPoint.X);
            Assert.AreEqual(0x00, zCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, rCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x00, rCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, rCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, rCurve.LateControlPoint.Y);
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

            Assert.AreEqual(0x00, xCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, xCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, xCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, xCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, yCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, yCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x00, yCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, yCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, zCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, zCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, zCurve.LateControlPoint.X);
            Assert.AreEqual(0x00, zCurve.LateControlPoint.Y);

            Assert.AreEqual(0x00, dCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x00, dCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, dCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, dCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, aCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, aCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x00, aCurve.LateControlPoint.X);
            Assert.AreEqual(0x00, aCurve.LateControlPoint.Y);
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

            Assert.AreEqual(0x00, xCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, xCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, xCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, xCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, yCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, yCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x00, yCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, yCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, zCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, zCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, zCurve.LateControlPoint.X);
            Assert.AreEqual(0x00, zCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, rCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x00, rCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, rCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, rCurve.LateControlPoint.Y);

            Assert.AreEqual(0x00, dCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x00, dCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x7F, dCurve.LateControlPoint.X);
            Assert.AreEqual(0x7F, dCurve.LateControlPoint.Y);

            Assert.AreEqual(0x7F, aCurve.EarlyControlPoint.X);
            Assert.AreEqual(0x7F, aCurve.EarlyControlPoint.Y);
            Assert.AreEqual(0x00, aCurve.LateControlPoint.X);
            Assert.AreEqual(0x00, aCurve.LateControlPoint.Y);
        }
    }
}
