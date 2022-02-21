using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.Converter;
using MikuMikuMethods.Pmm;
using MikuMikuMethods.Vmd;

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
            Camera = true,
            Light = true,
            Shadow = true,
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

    [TestMethod]
    public void ExtractModelFrameTest()
    {
        var pmm = new PolygonMovieMaker(TestData.GetPath("ConvertSource.pmm"));
        PmmModel model = pmm.Models[0];
        VocaloidMotionData vmd = model.ExtractModelMotion();

        var pmmBoneFrames = model.Bones.SelectMany(b => b.Frames).ToArray();
        var pmmMorphFrames = model.Morphs.SelectMany(b => b.Frames).ToArray();

        Assert.AreEqual(model.Name, vmd.ModelName);

        for (int i = 0; i < vmd.MotionFrames.Count; i++)
        {
            Assert.AreEqual((uint)pmmBoneFrames[i].Frame, vmd.MotionFrames[i].Frame);
            Assert.AreEqual(pmmBoneFrames[i].Movement, vmd.MotionFrames[i].Position);
            Assert.AreEqual(pmmBoneFrames[i].Rotation, vmd.MotionFrames[i].Rotation);
        }

        for (int i = 0; i < vmd.MorphFrames.Count; i++)
        {
            Assert.AreEqual((uint)pmmMorphFrames[i].Frame, vmd.MorphFrames[i].Frame);
            Assert.AreEqual(pmmMorphFrames[i].Weight, vmd.MorphFrames[i].Weight);
        }

        for (int i = 0; i < vmd.PropertyFrames.Count; i++)
        {
            Assert.AreEqual((uint)model.ConfigFrames[i].Frame, vmd.PropertyFrames[i].Frame);
            Assert.AreEqual(model.ConfigFrames[i].Visible, vmd.PropertyFrames[i].IsVisible);

            var IKs = model.ConfigFrames[i].EnableIK.Zip(vmd.PropertyFrames[i].IKEnabled);
            foreach (var comPair in IKs)
            {
                Assert.AreEqual(comPair.First.Key.Name, comPair.Second.Key);
                Assert.AreEqual(comPair.First.Value, comPair.Second.Value);
            }
        }
    }

    [TestMethod]
    public void ApplyVmdTest()
    {
        var pmm = new PolygonMovieMaker(TestData.GetPath("ApplyTarget.pmm"));
        var miku = pmm.Models[3];

        var cameraVmd = new VocaloidMotionData(TestData.GetPath("ApplySource_Camera.vmd"));
        var motionVmd = new VocaloidMotionData(TestData.GetPath("ApplySource_Motion.vmd"));

        pmm.ApplyCameraVmd(cameraVmd);
        miku.ApplyModelVmd(motionVmd);

        var expected = new PolygonMovieMaker(TestData.GetPath("ApplyExpected.pmm"));

        pmm.Write(TestData.GetPath("ApplyResult.pmm"));
        var result = new PolygonMovieMaker(TestData.GetPath("ApplyResult.pmm"));
    }
}
