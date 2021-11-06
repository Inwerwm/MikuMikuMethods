using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.PMM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTestPmm
    {
        [TestMethod]
        public void IOTest()
        {
            var pmm = new PolygonMovieMaker(TestData.GetPath("PragmaticTestData.pmm"));
        }

        [TestMethod]
        public void OldIO()
        {
            var pmm = new MikuMikuMethods.Binary.PMM.PolygonMovieMaker(TestData.GetPath("PragmaticTestData.pmm"));
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
            Assert.AreEqual(0, pmm1.GetRenderOrder(model1));
            Assert.AreEqual(1, pmm1.GetRenderOrder(model2));
            Assert.AreEqual(0, pmm1.GetCalculateOrder(model1));
            Assert.AreEqual(1, pmm1.GetCalculateOrder(model2));

            // 同じモデルが違う PMM に追加されても変わらない
            // 初期実装だとこれがダメだった
            pmm2.Models.Add(model2);
            pmm2.Models.Add(model1);

            Assert.AreEqual(0, pmm2.GetRenderOrder(model2));
            Assert.AreEqual(1, pmm2.GetRenderOrder(model1));
            Assert.AreEqual(0, pmm2.GetCalculateOrder(model2));
            Assert.AreEqual(1, pmm2.GetCalculateOrder(model1));

            Assert.AreEqual(0, pmm1.GetRenderOrder(model1));
            Assert.AreEqual(1, pmm1.GetRenderOrder(model2));
            Assert.AreEqual(0, pmm1.GetCalculateOrder(model1));
            Assert.AreEqual(1, pmm1.GetCalculateOrder(model2));

            // 順序設定の反映がされているか
            pmm1.SetRenderOrder(model1, 1);
            pmm1.SetCalculateOrder(model2, 0);

            Assert.AreEqual(0, pmm1.GetRenderOrder(model2));
            Assert.AreEqual(1, pmm1.GetRenderOrder(model1));
            Assert.AreEqual(0, pmm1.GetCalculateOrder(model2));
            Assert.AreEqual(1, pmm1.GetCalculateOrder(model1));

            // モデルが削除されたらそれが反映されるか
            // 存在しないモデルの順序の get/set できちんと例外が投げられるか
            pmm1.Models.Clear();
            Assert.ThrowsException<KeyNotFoundException>(() => pmm1.GetRenderOrder(model1));
            Assert.ThrowsException<KeyNotFoundException>(() => pmm1.GetCalculateOrder(model2));
            Assert.ThrowsException<ArgumentException>(() => pmm1.SetRenderOrder(model1, 0));
            Assert.ThrowsException<ArgumentException>(() => pmm1.SetCalculateOrder(model2, 0));

            // 一部のモデルを削除したら番号が繰り上がるか
            // ちゃんと反映されているか
            pmm1.Models.Add(model1);
            pmm1.Models.Add(model2);
            Assert.AreEqual(0, pmm1.GetRenderOrder(model1));
            Assert.AreEqual(1, pmm1.GetRenderOrder(model2));

            pmm1.Models.Remove(model1);
            Assert.ThrowsException<KeyNotFoundException>(() => pmm1.GetRenderOrder(model1));
            Assert.AreEqual(0, pmm1.GetCalculateOrder(model2));

            // モデルの差し替えが発生したとき順序が保たれるか
            Assert.AreEqual(0, pmm2.GetRenderOrder(model2));
            Assert.AreEqual(1, pmm2.GetRenderOrder(model1));
            Assert.AreEqual(0, pmm2.GetCalculateOrder(model2));
            Assert.AreEqual(1, pmm2.GetCalculateOrder(model1));

            var model3 = new PmmModel() { Name = "モデル3" };
            pmm2.Models[0] = model3;
            Assert.AreEqual(0, pmm2.GetRenderOrder(model3));
            Assert.ThrowsException<KeyNotFoundException>(() => pmm2.GetRenderOrder(model2));
            Assert.AreEqual(1, pmm2.GetRenderOrder(model1));
            Assert.AreEqual(0, pmm2.GetCalculateOrder(model3));
            Assert.ThrowsException<KeyNotFoundException>(() => pmm2.GetCalculateOrder(model2));
            Assert.AreEqual(1, pmm2.GetCalculateOrder(model1));
        }

        [TestMethod]
        public void AccessoryOrderTest()
        {
            var pmm1 = new PolygonMovieMaker();
            var pmm2 = new PolygonMovieMaker();

            var acs1 = new PmmAccessory() { Name = "アクセサリー1" };
            var acs2 = new PmmAccessory() { Name = "アクセサリー2" };

            pmm1.Accessories.Add(acs1);
            pmm1.Accessories.Add(acs2);

            // 追加した順に設定される
            Assert.AreEqual(0, pmm1.GetRenderOrder(acs1));
            Assert.AreEqual(1, pmm1.GetRenderOrder(acs2));

            // 同じアクセサリーが違う PMM に追加されても変わらない
            // 初期実装だとこれがダメだった
            pmm2.Accessories.Add(acs2);
            pmm2.Accessories.Add(acs1);

            Assert.AreEqual(0, pmm2.GetRenderOrder(acs2));
            Assert.AreEqual(1, pmm2.GetRenderOrder(acs1));

            Assert.AreEqual(0, pmm1.GetRenderOrder(acs1));
            Assert.AreEqual(1, pmm1.GetRenderOrder(acs2));

            // 順序設定の反映がされているか
            pmm1.SetRenderOrder(acs1, 1);

            Assert.AreEqual(0, pmm1.GetRenderOrder(acs2));
            Assert.AreEqual(1, pmm1.GetRenderOrder(acs1));

            // アクセサリーが削除されたらそれが反映されるか
            // 存在しないアクセサリーの順序の get/set できちんと例外が投げられるか
            pmm1.Accessories.Clear();
            Assert.ThrowsException<KeyNotFoundException>(() => pmm1.GetRenderOrder(acs1));
            Assert.ThrowsException<ArgumentException>(() => pmm1.SetRenderOrder(acs1, 0));

            // 一部のアクセサリーを削除したら番号が繰り上がるか
            // ちゃんと反映されているか
            pmm1.Accessories.Add(acs1);
            pmm1.Accessories.Add(acs2);
            Assert.AreEqual(0, pmm1.GetRenderOrder(acs1));
            Assert.AreEqual(1, pmm1.GetRenderOrder(acs2));

            pmm1.Accessories.Remove(acs1);
            Assert.ThrowsException<KeyNotFoundException>(() => pmm1.GetRenderOrder(acs1));

            // アクセサリーの差し替えが発生したとき順序が保たれるか
            Assert.AreEqual(0, pmm2.GetRenderOrder(acs2));
            Assert.AreEqual(1, pmm2.GetRenderOrder(acs1));

            var acs3 = new PmmAccessory() { Name = "アクセサリー3" };
            pmm2.Accessories[0] = acs3;
            Assert.AreEqual(0, pmm2.GetRenderOrder(acs3));
            Assert.ThrowsException<KeyNotFoundException>(() => pmm2.GetRenderOrder(acs2));
            Assert.AreEqual(1, pmm2.GetRenderOrder(acs1));
        }
    }
}
