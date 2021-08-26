using MikuMikuMethods.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// MMDプロジェクトファイル
    /// </summary>
    public class PolygonMovieMaker
    {
        internal ObservableCollection<PmmAccessory> _Accessories { get; } = new();
        internal List<PmmAccessory> _AccessoriyRenderOrder { get; } = new();

        /// <summary>
        /// PMMファイルバージョン
        /// </summary>
        public string Version { get; internal set; } = "Polygon Movie maker 0002";
        /// <summary>
        /// 出力解像度
        /// </summary>
        public Size OutputResolution { get; set; } = new(1920, 1080);

        /// <summary>
        /// カメラ
        /// </summary>
        public PmmCamera Camera { get; } = new();
        /// <summary>
        /// 照明
        /// </summary>
        public PmmLight Light { get; } = new();
        /// <summary>
        /// セルフ影
        /// </summary>
        public PmmSelfShadow SelfShadow { get; } = new();
        /// <summary>
        /// 物理
        /// </summary>
        public PmmPhysics Physics { get; } = new();
        /// <summary>
        /// アクセサリー
        /// </summary>
        public IList<PmmAccessory> Accessories => _Accessories;
        /// <summary>
        /// モデル
        /// </summary>
        public List<PmmModel> Models { get; } = new();

        /// <summary>
        /// 背景と音声
        /// </summary>
        public PmmBackGroundMedia BackGround { get; } = new();

        /// <summary>
        /// 3Dビュー画面の状態
        /// </summary>
        public PmmRenderPane RenderPane { get; } = new();
        /// <summary>
        /// キーフレーム編集画面の状態
        /// </summary>
        public PmmKeyFramePane KeyFramePane { get; } = new();
        /// <summary>
        /// 各種操作パネル画面の状態
        /// </summary>
        public PmmPanelPane PanelPane { get; } = new();

        public PolygonMovieMaker()
        {
            _Accessories.CollectionChanged += SyncOrders(new[] { _AccessoriyRenderOrder });

            NotifyCollectionChangedEventHandler SyncOrders<T>(IEnumerable<List<T>> orderLists) where T : IRelationableElement<T> =>
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
                                RemoveAll(e.OldItems?.Cast<T>());
                                AddAll(e.NewItems?.Cast<T>() ?? Array.Empty<T>());
                                break;
                            case NotifyCollectionChangedAction.Reset:
                                RemoveAll(list.ToArray());
                                AddAll(sender as IEnumerable<T> ?? Array.Empty<T>());
                                break;
                            case NotifyCollectionChangedAction.Move:
                            default:
                                break;
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
}
