using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.Converter;
using MikuMikuMethods.Mme;
using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.Frame;
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
        // 期待データのフレーム順をソートしておくことで後々比較できるようにする
        var expected = new PolygonMovieMaker(TestData.GetPath("ApplyExpected.pmm"));
        var expectedMiku = expected.Models[3];
        expected.Write(TestData.GetPath("ApplyExpected.pmm"));

        var pmmApply = new PolygonMovieMaker(TestData.GetPath("ApplyTarget.pmm"));
        var applyMiku = Apply(pmmApply);
        pmmApply.Write(TestData.GetPath("ApplyResult.pmm"));

        var pmmOverWrite = new PolygonMovieMaker(TestData.GetPath("ApplyExpected.pmm"));
        var overMiku = Apply(pmmOverWrite);
        pmmOverWrite.Write(TestData.GetPath("ApplyOverwrite.pmm"));

        // EMM ファイルをコピーしてテスト後の確認をしやすくしておく
        var emm = new EmmData(TestData.GetPath("ApplyTarget.emm"));
        emm.Write(TestData.GetPath("ApplyResult.emm"));
        emm.Write(TestData.GetPath("ApplyOverwrite.emm"));


        // 各フレーム比較のためフレーム数順でソート
        var boneFramesGroupedByName = expectedMiku.Bones.Select(bone => (
            Expected: bone.Frames,
            Apply: applyMiku.Bones.FirstOrDefault(b => b.Name == bone.Name)?.Frames,
            Over: overMiku.Bones.FirstOrDefault(b => b.Name == bone.Name)?.Frames
        ));

        CompareFrames(boneFramesGroupedByName, (frame, makeMsg) =>
        {
            Assert.AreEqual(frame.Expected.Movement, frame.Apply.Movement, makeMsg("Apply.Movement is not equal."));
            Assert.AreEqual(frame.Expected.Movement, frame.Over.Movement, makeMsg("Over.Movement is not equal."));

            // 回転軸制限の関係上、回転は合わせられない
            //Assert.AreEqual(frame.Expected.Rotation, frame.Apply.Rotation, makeMsg("Apply.Rotation is not equal."));
            //Assert.AreEqual(frame.Expected.Rotation, frame.Over.Rotation, makeMsg("Over.Rotation is not equal."));
        });

        var morphFramesGroupedByName = expectedMiku.Morphs.Select(morph => (
            Expected: morph.Frames,
            Apply: applyMiku.Morphs.FirstOrDefault(m => m.Name == morph.Name)?.Frames,
            Over: overMiku.Morphs.FirstOrDefault(m => m.Name == morph.Name)?.Frames
        ));

        CompareFrames(morphFramesGroupedByName, (frame, makeMsg) =>
        {
            Assert.AreEqual(frame.Expected.Weight, frame.Apply.Weight, makeMsg("Apply.Weight is not equal."));
            Assert.AreEqual(frame.Expected.Weight, frame.Over.Weight, makeMsg("Over.Weight is not equal."));
        });

        static PmmModel Apply(PolygonMovieMaker pmm)
        {
            var miku = pmm.Models[3];
            pmm.Camera.Frames.Clear();
            foreach (var bone in miku.Bones)
            {
                bone.Frames.Clear();
            }

            var cameraVmd = new VocaloidMotionData(TestData.GetPath("ApplySource_Camera.vmd"));
            var motionVmd = new VocaloidMotionData(TestData.GetPath("ApplySource_Motion.vmd"));

            pmm.ApplyCameraVmd(cameraVmd);
            miku.ApplyModelVmd(motionVmd);

            return miku;
        }

        static void SortMotionFrames<T>(List<T> frames) where T : IPmmFrame
        {
            frames.Sort((left, right) => left.Frame.CompareTo(right.Frame));
        }

        static void CompareFrames<T>(IEnumerable<(List<T> Expected, List<T> Apply, List<T> Over)> framesGroupedByName, Action<(T Expected, T Apply, T Over, int Count), Func<string, string>> comparer) where T : IPmmFrame
        {
            foreach (var sameNameFrames in framesGroupedByName)
            {
                SortMotionFrames(sameNameFrames.Expected);
                SortMotionFrames(sameNameFrames.Apply);
                SortMotionFrames(sameNameFrames.Over);
            }

            var frames = framesGroupedByName.SelectMany(sameNameFrames =>
                sameNameFrames.Expected.Zip(sameNameFrames.Apply, sameNameFrames.Over)
                                       .Select((tp, i) => (Expected: tp.First, Apply: tp.Second, Over: tp.Third, Count: i))
            );

            foreach (var frame in frames)
            {
                var makeMsg = (string str) => $"Count:{frame.Count} - {str}";

                Assert.AreEqual(frame.Expected.Frame, frame.Apply.Frame, makeMsg("Apply.Frame is not equal."));
                Assert.AreEqual(frame.Expected.Frame, frame.Over.Frame, makeMsg("Over.Frame is not equal."));

                comparer(frame, makeMsg);
            }
        }
    }
}
