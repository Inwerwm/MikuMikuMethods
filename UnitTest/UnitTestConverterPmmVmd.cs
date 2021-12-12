using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.Converter;
using MikuMikuMethods.Pmm;
using MikuMikuMethods.Vmd;
using System.Numerics;

namespace UnitTest;

[TestClass]
public class UnitTestConverterPmmVmd
{
    [TestMethod]
    public void ExtractCameraFrameTest()
    {
        var pmm = new PolygonMovieMaker(TestData.GetPath("ConvertSource.pmm"));
        VocaloidMotionData vmd = pmm.ExtractCameraMotion(new()
        {
            ExtractCamera = true,
            ExtractLight = true,
            ExtractShadow = true,
        });

        AssertCameraFrame(0);
        AssertCameraFrame(1);

        AssertLightFrame(0);
        AssertLightFrame(1);

        void AssertCameraFrame(int index)
        {
            Assert.AreEqual((uint)pmm.Camera.Frames[index].Frame, vmd.CameraFrames[index].Frame);
            Assert.AreEqual((uint)pmm.Camera.Frames[index].ViewAngle, vmd.CameraFrames[index].ViewAngle);
            Assert.AreEqual(pmm.Camera.Frames[index].EyePosition, vmd.CameraFrames[index].Position);
        }

        void AssertLightFrame(int index)
        {
            Assert.AreEqual((uint)pmm.Light.Frames[index].Frame, vmd.LightFrames[index].Frame);
            Assert.AreEqual(pmm.Light.Frames[index].Color, vmd.LightFrames[index].Color);
            Assert.AreEqual(pmm.Light.Frames[index].Position, vmd.LightFrames[index].Position);
        }
    }
}
