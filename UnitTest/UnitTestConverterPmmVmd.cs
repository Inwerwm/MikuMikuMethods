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
        VocaloidMotionData vmd = pmm.ExtractCameraMotion();

        AssertCameraFrame(0, expectedFrame: 0, expectedViewAngle: 30, expectedPosition: new(0, 10, 0));
        AssertCameraFrame(1, expectedFrame: 30, expectedViewAngle: 50, expectedPosition: new(0, 11.25f, 0));

        void AssertCameraFrame(int index,uint expectedFrame, uint expectedViewAngle, Vector3 expectedPosition)
        {
            Assert.AreEqual(expectedFrame, vmd.CameraFrames[index].Frame);
            Assert.AreEqual(expectedViewAngle, vmd.CameraFrames[index].ViewAngle);
            Assert.AreEqual(expectedPosition.X, vmd.CameraFrames[index].Position.X, 0.005);
            Assert.AreEqual(expectedPosition.Y, vmd.CameraFrames[index].Position.Y, 0.005);
            Assert.AreEqual(expectedPosition.Z, vmd.CameraFrames[index].Position.Z, 0.005);
        }
    }
}
