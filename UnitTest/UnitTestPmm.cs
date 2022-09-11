using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;
using System.Text;

namespace UnitTest;

[TestClass]
public class UnitTestPmm
{
    [TestMethod]
    public void IOTest()
    {
        var PmmName = "PragmaticTestData";
        var pmm = TestData.PmmLoggingRead(PmmName);
        pmm.Write(TestData.GetPath(PmmName + "_out.pmm"));
        var reloaded = TestData.PmmLoggingRead(PmmName + "_out");

        var interpolationCurvesComparer = new EqualityComparer<MikuMikuMethods.InterpolationCurve>((o, r) => o == r);

        var modelComparer = new EqualityComparer<PmmModel>((o, r) =>
            o?.Name == r?.Name &&
            o?.NameEn == r?.NameEn &&
            o?.Path == r?.Path &&
            o?.EnableSelfShadow == r?.EnableSelfShadow &&
            o?.EnableAlphaBlend == r?.EnableAlphaBlend
        );
        var boneComparer = new EqualityComparer<PmmBone>((o, r) =>
            o?.IsIK == r?.IsIK &&
            o?.CanSetOutsideParent == r?.CanSetOutsideParent &&
            o?.Name == r?.Name
        );
        var boneFrameComparer = new EqualityComparer<PmmBoneFrame>((o, r) =>
            o?.Frame == r?.Frame &&
            o?.EnablePhysic == r?.EnablePhysic &&
            o?.Movement == r?.Movement &&
            o?.Rotation == r?.Rotation
        );
        var morphComparer = new EqualityComparer<PmmMorph>((o, r) =>
            o?.Name == r?.Name
        );
        var morphFrameComparer = new EqualityComparer<PmmMorphFrame>((o, r) =>
            o?.Frame == r?.Frame &&
            o?.Weight == r?.Weight
        );
        var modelConfigFrameComparer = new EqualityComparer<PmmModelConfigFrame>((o, r) =>
            o?.Frame == r?.Frame &&
            o?.Visible == r?.Visible
        );
        var outsideParentComparer = new EqualityComparer<KeyValuePair<PmmBone, PmmOutsideParentState>>((o, r) =>
            boneComparer.Equals(o.Key, r.Key) &&
            o.Value?.StartFrame == r.Value?.StartFrame &&
            o.Value?.EndFrame == r.Value?.EndFrame &&
            modelComparer.Equals(o.Value?.ParentModel, o.Value?.ParentModel) &&
            boneComparer.Equals(o.Value?.ParentBone, r.Value?.ParentBone)
        );
        var enableIKComparer = new EqualityComparer<KeyValuePair<PmmBone, bool>>((o, r) =>
            boneComparer.Equals(o.Key, r.Key) &&
            o.Value == r.Value
        );

        var accessoryComparer = new EqualityComparer<PmmAccessory>((o, r) =>
            o.Name == r.Name &&
            o.Path == r.Path
        );
        var accessoryFrameComparer = new EqualityComparer<PmmAccessoryFrame>((o, r) =>
            o.Frame == r.Frame &&
            modelComparer.Equals(o.ParentModel, r.ParentModel) &&
            boneComparer.Equals(o.ParentBone, r.ParentBone) &&
            o.Position == r.Position &&
            o.Rotation == r.Rotation &&
            o.Scale == r.Scale &&
            o.Transparency == r.Transparency &&
            o.Visible == r.Visible
        );

        var cameraFrameComparer = new EqualityComparer<PmmCameraFrame>((o, r) =>
            o.Frame == r.Frame &&
            o.Distance == r.Distance &&
            o.DisablePerspective == r.DisablePerspective &&
            o.EyePosition == r.EyePosition &&
            o.TargetPosition == r.TargetPosition &&
            o.Rotation == r.Rotation &&
            o.ViewAngle == r.ViewAngle &&
            modelComparer.Equals(o.FollowingModel, r.FollowingModel) &&
            boneComparer.Equals(o.FollowingBone, r.FollowingBone)
        );

        var lightFrameComparer = new EqualityComparer<PmmLightFrame>((o, r) =>
            o.Frame == r.Frame &&
            o.Position == r.Position &&
            o.Color == r.Color
        );

        MacroAssert.AreElementsSame(pmm.Models, reloaded.Models, modelComparer,
            (oModel, rModel) => MacroAssert.AreElementsSame(oModel.Bones, rModel.Bones, boneComparer,
                (oBone, rBone) => MacroAssert.AreElementsSame(oBone.Frames, rBone.Frames, boneFrameComparer,
                    (oFrame, rFrame) => MacroAssert.AreElementsSame(oFrame.InterpolationCurves, rFrame.InterpolationCurves, interpolationCurvesComparer))),
            (oModel, rModel) => MacroAssert.AreElementsSame(oModel.Morphs, rModel.Morphs, morphComparer,
                (oMorph, rMorph) => MacroAssert.AreElementsSame(oMorph.Frames, rMorph.Frames, morphFrameComparer)),
            (oModel, rModel) => MacroAssert.AreElementsSame(oModel.ConfigFrames, rModel.ConfigFrames, modelConfigFrameComparer,
                (oConfig, rConfig) => MacroAssert.AreElementsSame(oConfig.OutsideParent, rConfig.OutsideParent, outsideParentComparer),
                (oConfig, rConfig) => MacroAssert.AreElementsSame(oConfig.EnableIK, rConfig.EnableIK, enableIKComparer)));

        MacroAssert.AreElementsSame(pmm.Accessories, reloaded.Accessories, accessoryComparer,
            (oAcs, rAcs) => MacroAssert.AreElementsSame(oAcs.Frames, rAcs.Frames, accessoryFrameComparer));

        MacroAssert.AreElementsSame(pmm.Camera.Frames.OrderBy(f => f.Frame), reloaded.Camera.Frames.OrderBy(f => f.Frame), cameraFrameComparer,
            (oCameraFrame, rCameraFrame) => MacroAssert.AreElementsSame(oCameraFrame.InterpolationCurves, rCameraFrame.InterpolationCurves, interpolationCurvesComparer));

        MacroAssert.AreElementsSame(pmm.Light.Frames.OrderBy(f => f.Frame), reloaded.Light.Frames.OrderBy(f => f.Frame), lightFrameComparer);
    }

    [TestMethod]
    public void ModelOrderTest()
    {
        var pmm1 = new PolygonMovieMaker();
        var pmm2 = new PolygonMovieMaker();

        var model1 = new PmmModel() { Name = "モデル1" };
        var model2 = new PmmModel() { Name = "モデル2" };

        pmm1.Models.Add(model1);
        pmm1.Models.Add(model2);

        // 追加した順に設定される
        Assert.AreEqual((byte)0, pmm1.GetRenderOrder(model1).Value);
        Assert.AreEqual((byte)1, pmm1.GetRenderOrder(model2).Value);
        Assert.AreEqual((byte)0, pmm1.GetCalculateOrder(model1).Value);
        Assert.AreEqual((byte)1, pmm1.GetCalculateOrder(model2).Value);

        // 同じモデルが違う PMM に追加されても変わらない
        // 初期実装だとこれがダメだった
        pmm2.Models.Add(model2);
        pmm2.Models.Add(model1);

        Assert.AreEqual((byte)0, pmm2.GetRenderOrder(model2).Value);
        Assert.AreEqual((byte)1, pmm2.GetRenderOrder(model1).Value);
        Assert.AreEqual((byte)0, pmm2.GetCalculateOrder(model2).Value);
        Assert.AreEqual((byte)1, pmm2.GetCalculateOrder(model1).Value);

        Assert.AreEqual((byte)0, pmm1.GetRenderOrder(model1).Value);
        Assert.AreEqual((byte)1, pmm1.GetRenderOrder(model2).Value);
        Assert.AreEqual((byte)0, pmm1.GetCalculateOrder(model1).Value);
        Assert.AreEqual((byte)1, pmm1.GetCalculateOrder(model2).Value);

        // 順序設定の反映がされているか
        pmm1.SetRenderOrder(model1, 1);
        pmm1.SetCalculateOrder(model2, 0);

        Assert.AreEqual((byte)0, pmm1.GetRenderOrder(model2).Value);
        Assert.AreEqual((byte)1, pmm1.GetRenderOrder(model1).Value);
        Assert.AreEqual((byte)0, pmm1.GetCalculateOrder(model2).Value);
        Assert.AreEqual((byte)1, pmm1.GetCalculateOrder(model1).Value);

        // モデルが削除されたらそれが反映されるか
        // 存在しないモデルの順序の get/set できちんと例外が投げられるか
        pmm1.Models.Clear();
        Assert.IsNull(pmm1.GetRenderOrder(model1));
        Assert.IsNull(pmm1.GetCalculateOrder(model2));
        Assert.ThrowsException<ArgumentException>(() => pmm1.SetRenderOrder(model1, 0));
        Assert.ThrowsException<ArgumentException>(() => pmm1.SetCalculateOrder(model2, 0));

        // 一部のモデルを削除したら番号が繰り上がるか
        // ちゃんと反映されているか
        pmm1.Models.Add(model1);
        pmm1.Models.Add(model2);
        Assert.AreEqual((byte)0, pmm1.GetRenderOrder(model1).Value);
        Assert.AreEqual((byte)1, pmm1.GetRenderOrder(model2).Value);

        pmm1.Models.Remove(model1);
        Assert.IsNull(pmm1.GetRenderOrder(model1));
        Assert.AreEqual((byte)0, pmm1.GetCalculateOrder(model2).Value);

        // モデルの差し替えが発生したとき順序が保たれるか
        Assert.AreEqual((byte)0, pmm2.GetRenderOrder(model2).Value);
        Assert.AreEqual((byte)1, pmm2.GetRenderOrder(model1).Value);
        Assert.AreEqual((byte)0, pmm2.GetCalculateOrder(model2).Value);
        Assert.AreEqual((byte)1, pmm2.GetCalculateOrder(model1).Value);

        var model3 = new PmmModel() { Name = "モデル3" };
        pmm2.Models[0] = model3;
        Assert.AreEqual((byte)0, pmm2.GetRenderOrder(model3).Value);
        Assert.IsNull(pmm2.GetRenderOrder(model2));
        Assert.AreEqual((byte)1, pmm2.GetRenderOrder(model1).Value);
        Assert.AreEqual((byte)0, pmm2.GetCalculateOrder(model3).Value);
        Assert.IsNull(pmm2.GetCalculateOrder(model2));
        Assert.AreEqual((byte)1, pmm2.GetCalculateOrder(model1).Value);
    }

    [TestMethod]
    public void AccessoryOrderTest()
    {
        var pmm1 = new PolygonMovieMaker();
        var pmm2 = new PolygonMovieMaker();

        var acs1 = new PmmAccessory("アクセサリー1", "");
        var acs2 = new PmmAccessory("アクセサリー2", "");

        pmm1.Accessories.Add(acs1);
        pmm1.Accessories.Add(acs2);

        // 追加した順に設定される
        Assert.AreEqual((byte)0, pmm1.GetRenderOrder(acs1).Value);
        Assert.AreEqual((byte)1, pmm1.GetRenderOrder(acs2).Value);

        // 同じアクセサリーが違う PMM に追加されても変わらない
        // 初期実装だとこれがダメだった
        pmm2.Accessories.Add(acs2);
        pmm2.Accessories.Add(acs1);

        Assert.AreEqual((byte)0, pmm2.GetRenderOrder(acs2).Value);
        Assert.AreEqual((byte)1, pmm2.GetRenderOrder(acs1).Value);

        Assert.AreEqual((byte)0, pmm1.GetRenderOrder(acs1).Value);
        Assert.AreEqual((byte)1, pmm1.GetRenderOrder(acs2).Value);

        // 順序設定の反映がされているか
        pmm1.SetRenderOrder(acs1, 1);

        Assert.AreEqual((byte)0, pmm1.GetRenderOrder(acs2).Value);
        Assert.AreEqual((byte)1, pmm1.GetRenderOrder(acs1).Value);

        // アクセサリーが削除されたらそれが反映されるか
        // 存在しないアクセサリーの順序の get/set できちんと例外が投げられるか
        pmm1.Accessories.Clear();
        Assert.IsNull(pmm1.GetRenderOrder(acs1));
        Assert.ThrowsException<ArgumentException>(() => pmm1.SetRenderOrder(acs1, 0));

        // 一部のアクセサリーを削除したら番号が繰り上がるか
        // ちゃんと反映されているか
        pmm1.Accessories.Add(acs1);
        pmm1.Accessories.Add(acs2);
        Assert.AreEqual((byte)0, pmm1.GetRenderOrder(acs1).Value);
        Assert.AreEqual((byte)1, pmm1.GetRenderOrder(acs2).Value);

        pmm1.Accessories.Remove(acs1);
        Assert.IsNull(pmm1.GetRenderOrder(acs1));

        // アクセサリーの差し替えが発生したとき順序が保たれるか
        Assert.AreEqual((byte)0, pmm2.GetRenderOrder(acs2).Value);
        Assert.AreEqual((byte)1, pmm2.GetRenderOrder(acs1).Value);

        var acs3 = new PmmAccessory("アクセサリー3", "");
        pmm2.Accessories[0] = acs3;
        Assert.AreEqual((byte)0, pmm2.GetRenderOrder(acs3).Value);
        Assert.IsNull(pmm2.GetRenderOrder(acs2));
        Assert.AreEqual((byte)1, pmm2.GetRenderOrder(acs1).Value);
    }

    [TestMethod]
    public void Test_Asc_TransAndVisible()
    {
        var tv = PmmAccessoryFrame.SeparateTransAndVisible(45);
        var re = PmmAccessoryFrame.CreateTransAndVisible(tv.Transparency, tv.Visible);

        Assert.IsTrue(tv.Visible);
        Assert.AreEqual(0.78f, tv.Transparency);
        Assert.AreEqual(45, re);
    }

    [TestMethod]
    public void RangeSelectorTest()
    {
        PmmModel model = new() { Name = "テストモデル" };
        PmmBone bone1 = new("ボーン1");
        model.Bones.Add(bone1);

        Assert.AreEqual(7, PmmRangeSelector.Create(bone1, model).Index);

        PmmMorph morph1 = new("モーフ1");
        model.Morphs.Add(morph1);

        Assert.AreEqual(6, PmmRangeSelector.Create(morph1, model).Index);
        Assert.AreEqual(7, PmmRangeSelector.Create(bone1, model).Index);
    }

    [TestMethod]
    public void RenderOrderResolveTest()
    {
        _ = new PolygonMovieMaker(TestData.GetPath("RenderOrder.pmm"));
        _ = new PolygonMovieMaker(TestData.GetPath("RenderOrder2.pmm"));
    }
}
