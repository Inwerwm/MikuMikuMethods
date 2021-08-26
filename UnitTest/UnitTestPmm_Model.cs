using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuMethods.PMM;
using MikuMikuMethods.PMM.Frame;

namespace UnitTest
{
    [TestClass]
    public class UnitTestPmm_Model
    {
        [TestMethod]
        public void Test_Model_RenderOrder()
        {
            var pmm = new PolygonMovieMaker();

            int modelCount = 0;
            pmm._Models.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        modelCount++;
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        modelCount--;
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        modelCount = 0;
                        break;
                    default:
                        break;
                }
            };

            PmmModel model = new();
            
            // 追加処理の連動
            pmm.Models.Add(model);
            Assert.AreEqual(1, modelCount);
            Assert.AreEqual(1, pmm.Models.Count);
            Assert.AreEqual(1, pmm._Models.Count);
            Assert.AreEqual(1, pmm._ModelRenderOrder.Count);
            Assert.AreEqual(1, pmm._ModelCalculateOrder.Count);
            Assert.AreEqual(pmm._Models[0], pmm._ModelRenderOrder[0]);

            // 削除処理の連動
            pmm.Models.Remove(model);
            Assert.AreEqual(0, modelCount);
            Assert.AreEqual(0, pmm.Models.Count);
            Assert.AreEqual(0, pmm._Models.Count);
            Assert.AreEqual(0, pmm._ModelRenderOrder.Count);
            Assert.AreEqual(0, pmm._ModelCalculateOrder.Count);

            // 順序管理
            PmmModel modelA = new();
            PmmModel modelB = new();
            PmmModel modelC = new();
            PmmModel modelD = new();
            pmm.Models.Add(modelA);
            pmm.Models.Add(modelB);
            pmm.Models.Add(modelC);
            pmm.Models.Add(modelD);

            Assert.AreEqual(4, modelCount);
            Assert.AreEqual(4, pmm.Models.Count);
            Assert.AreEqual(4, pmm._Models.Count);
            Assert.AreEqual(4, pmm._ModelRenderOrder.Count);
            Assert.AreEqual(4, pmm._ModelCalculateOrder.Count);

            Assert.AreEqual(0, modelA.RenderOrder);
            Assert.AreEqual(1, modelB.RenderOrder);
            Assert.AreEqual(2, modelC.RenderOrder);
            Assert.AreEqual(3, modelD.RenderOrder);

            Assert.AreEqual(0, modelA.CalculateOrder);
            Assert.AreEqual(1, modelB.CalculateOrder);
            Assert.AreEqual(2, modelC.CalculateOrder);
            Assert.AreEqual(3, modelD.CalculateOrder);


            modelA.RenderOrder = 2;
            Assert.AreEqual(0, modelB.RenderOrder);
            Assert.AreEqual(1, modelC.RenderOrder);
            Assert.AreEqual(2, modelA.RenderOrder);
            Assert.AreEqual(3, modelD.RenderOrder);

            Assert.AreEqual(0, modelA.CalculateOrder);
            Assert.AreEqual(1, modelB.CalculateOrder);
            Assert.AreEqual(2, modelC.CalculateOrder);
            Assert.AreEqual(3, modelD.CalculateOrder);

            modelC.CalculateOrder = 1;
            Assert.AreEqual(0, modelB.RenderOrder);
            Assert.AreEqual(1, modelC.RenderOrder);
            Assert.AreEqual(2, modelA.RenderOrder);
            Assert.AreEqual(3, modelD.RenderOrder);

            Assert.AreEqual(0, modelA.CalculateOrder);
            Assert.AreEqual(1, modelC.CalculateOrder);
            Assert.AreEqual(2, modelB.CalculateOrder);
            Assert.AreEqual(3, modelD.CalculateOrder);


            // 全消の連動
            pmm.Models.Clear();
            Assert.AreEqual(0, modelCount);
            Assert.AreEqual(0, pmm.Models.Count);
            Assert.AreEqual(0, pmm._Models.Count);
            Assert.AreEqual(0, pmm._ModelRenderOrder.Count);
            Assert.AreEqual(0, pmm._ModelCalculateOrder.Count);


            pmm.Models.Add(modelA);
            pmm.Models.Add(modelB);

            // 要素数より大きい数が指定されると例外を吐く
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => modelA.RenderOrder = 2);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => modelB.CalculateOrder = 2);

            // get/set は pmm に組み込まれてないとできない
            Assert.ThrowsException<InvalidOperationException>(() => Console.WriteLine(modelC.RenderOrder));
            Assert.ThrowsException<InvalidOperationException>(() => modelC.RenderOrder = 0);
            Assert.ThrowsException<InvalidOperationException>(() => Console.WriteLine(modelD.CalculateOrder));
            Assert.ThrowsException<InvalidOperationException>(() => modelD.CalculateOrder = 0);

            // 違うpmmで異なる順序が割り振られる
            var pmm2 = new PolygonMovieMaker();
            pmm2.Models.Add(modelC);
            pmm2.Models.Add(modelD);
            Assert.AreEqual(0, modelA.RenderOrder);
            Assert.AreEqual(0, modelA.CalculateOrder);
            Assert.AreEqual(0, modelC.RenderOrder);
            Assert.AreEqual(0, modelC.CalculateOrder);
        }
    }
}
