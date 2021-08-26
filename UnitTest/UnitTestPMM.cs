using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuMethods.PMM;

namespace UnitTest
{
    [TestClass]
    public class MyTestClass
    {
        [TestMethod]
        public void Test_Asc_TransAndVisible()
        {
            var tv = PmmAccessory.SeparateTransAndVisible(45);
            var re = PmmAccessory.CreateTransAndVisible(tv.Transparency, tv.Visible);

            Assert.IsTrue(tv.Visible);
            Assert.AreEqual(0.78f, tv.Transparency);
            Assert.AreEqual(45, re);
        }

        [TestMethod]
        public void Test_Asc_RenderOrder()
        {
            var pmm = new PolygonMovieMaker();

            int ascCount = 0;
            pmm._Accessories.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        ascCount++;
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        ascCount--;
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        ascCount = 0;
                        break;
                    default:
                        break;
                }
            };

            PmmAccessory asc = new();
            
            // 追加処理の連動
            pmm.Accessories.Add(asc);
            Assert.AreEqual(1, ascCount);
            Assert.AreEqual(1, pmm.Accessories.Count);
            Assert.AreEqual(1, pmm._Accessories.Count);
            Assert.AreEqual(1, pmm._AccessoriyRenderOrder.Count);
            Assert.AreEqual(pmm._Accessories[0], pmm._AccessoriyRenderOrder[0]);

            // 削除処理の連動
            pmm.Accessories.Remove(asc);
            Assert.AreEqual(0, ascCount);
            Assert.AreEqual(0, pmm.Accessories.Count);
            Assert.AreEqual(0, pmm._Accessories.Count);
            Assert.AreEqual(0, pmm._AccessoriyRenderOrder.Count);

            // 順序管理
            PmmAccessory ascA = new();
            PmmAccessory ascB = new();
            PmmAccessory ascC = new();
            PmmAccessory ascD = new();
            pmm.Accessories.Add(ascA);
            pmm.Accessories.Add(ascB);
            pmm.Accessories.Add(ascC);
            pmm.Accessories.Add(ascD);

            Assert.AreEqual(4, ascCount);
            Assert.AreEqual(4, pmm.Accessories.Count);
            Assert.AreEqual(4, pmm._Accessories.Count);
            Assert.AreEqual(4, pmm._AccessoriyRenderOrder.Count);

            Assert.AreEqual(0, ascA.RenderOrder);
            Assert.AreEqual(1, ascB.RenderOrder);
            Assert.AreEqual(2, ascC.RenderOrder);
            Assert.AreEqual(3, ascD.RenderOrder);

            ascA.RenderOrder = 2;
            Assert.AreEqual(0, ascB.RenderOrder);
            Assert.AreEqual(1, ascC.RenderOrder);
            Assert.AreEqual(2, ascA.RenderOrder);
            Assert.AreEqual(3, ascD.RenderOrder);

            // 全消の連動
            pmm.Accessories.Clear();
            Assert.AreEqual(0, ascCount);
            Assert.AreEqual(0, pmm.Accessories.Count);
            Assert.AreEqual(0, pmm._Accessories.Count);
            Assert.AreEqual(0, pmm._AccessoriyRenderOrder.Count);

            // 要素数より大きい数が指定される
            pmm.Accessories.Add(ascA);
            pmm.Accessories.Add(ascB);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ascA.RenderOrder = 2);
        }
    }
}
