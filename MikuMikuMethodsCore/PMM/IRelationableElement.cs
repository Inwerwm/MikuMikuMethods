using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MikuMikuMethods.PMM
{
    internal interface IRelationableElement<T> where T : IRelationableElement<T>
    {
        internal void AddRelation(IEnumerable<List<T>> lists);
        internal void RemoveRelation();

        static byte GetOrder(T self, List<T> orderCollection, string orderName)
        {
            int order = orderCollection?.IndexOf(self) ?? -1;
            return order != -1 ? (byte)order : throw new InvalidOperationException($"{orderName}順操作は PolygonMovieMaker クラスに登録されていなければできません。");
        }

        static void SetOrder(T self, List<T> orderCollection, string orderName, byte value)
        {
            var oldIndex = GetOrder(self, orderCollection, orderName);
            orderCollection.Remove(self);
            try
            {
                orderCollection.Insert(value, self);
            }
            catch (Exception)
            {
                orderCollection.Insert(oldIndex, self);
                throw;
            }
        }

        static NotifyCollectionChangedEventHandler SyncOrders(IEnumerable<List<T>> orderLists) =>
            (sender, e) =>
            {
                foreach (var list in orderLists)
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            AddAll(e.NewItems?.Cast<T>() ?? Array.Empty<T>());
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            RemoveAll(e.OldItems?.Cast<T>() ?? Array.Empty<T>());
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            ReplaceAll();
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            RemoveAll(list.ToArray());
                            AddAll(sender as IEnumerable<T> ?? Array.Empty<T>());
                            break;
                        case NotifyCollectionChangedAction.Move:
                        default:
                            break;
                    }

                    void ReplaceAll()
                    {
                        foreach (var item in e.OldItems?.Cast<T>())
                        {
                            item.RemoveRelation();
                        }
                        foreach (var item in e.NewItems?.Cast<T>().Select((Item, Id) => (Item, Id)))
                        {
                            item.Item.AddRelation(orderLists);
                            list[e.OldStartingIndex + item.Id] = item.Item;
                        }
                    }

                    void RemoveAll(IEnumerable<T> items)
                    {
                        foreach (T item in items)
                        {
                            list.Remove(item);
                            item.RemoveRelation();
                        }
                    }

                    void AddAll(IEnumerable<T> items)
                    {
                        foreach (T item in items)
                        {
                            list.Add(item);
                            item.AddRelation(orderLists);
                        }
                    }
                }
            };
    }
}
