using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace UnitTest;

[TestClass]
public class UnitTestPmm
{
    [TestMethod]
    public void IOTest()
    {
        var PmmName = "PragmaticTestData";
        var pmm = new PolygonMovieMaker(TestData.GetPath(PmmName + ".pmm"));
        pmm.Write(TestData.GetPath(PmmName + "_out.pmm"));
        var reloaded = new PolygonMovieMaker(TestData.GetPath(PmmName + "_out.pmm"));
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

    private enum LineKind
    {
        Value,
        Array,
        Section,
    }

    [TestMethod]
    public void FrameResolvingTest()
    {
        var pmm = TestData.PmmLoggingRead("FrameTest");
        pmm.Write(TestData.GetPath("FrameTest_Rewrite.pmm"));
        TestData.PmmLoggingRead("FrameTest_Rewrite");

        var originLog = File.ReadAllLines(TestData.GetPath("FrameTest_Readlog.txt"));
        var rewriteLog = File.ReadAllLines(TestData.GetPath("FrameTest_Rewrite_Readlog.txt"));

        Assert.AreEqual(originLog.Length, rewriteLog.Length);
        IEnumerable<(string Origin, string Rewrite)> log = originLog.Zip(rewriteLog);
        var compareResult = log.Select((l, i) =>
        {
            var kind = l.Origin.FirstOrDefault() == '#' ? LineKind.Section
                     : l.Origin.Split('|')[0].IndexOf('[') != -1 ? LineKind.Array
                     : LineKind.Value;

            return kind == LineKind.Value ? (Log: l, Count: i, IsEqual: l.Origin == l.Rewrite, Kind: kind) : (Log: l, Count: i, IsEqual: true, Kind: kind);
        });

        var notEquals = compareResult.Where(r => !r.IsEqual).ToArray();
        Assert.IsFalse(notEquals.Any(), $"異なる行数: {notEquals.Length}{Environment.NewLine}{string.Join(Environment.NewLine, notEquals.Select(r => $"{r.Count, 5}:{getType(r.Log.Origin), -9}| {getValue(r.Log.Origin)} <> {getValue(r.Log.Rewrite)}"))}");

        static string getType(string logStr) => logStr.Split('|')[0].Trim();
        static string getValue(string logStr) => logStr.Split('|')[1].Trim();
    }
}
