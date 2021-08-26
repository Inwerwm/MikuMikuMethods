using MikuMikuMethods.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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
            PmmAccessory._RenderOrderCollection = _AccessoriyRenderOrder;

            _Accessories.CollectionChanged += SyncOrders(new[] { _AccessoriyRenderOrder });

            NotifyCollectionChangedEventHandler SyncOrders<T>(IEnumerable<List<T>> orderLists) =>
                (sender, e) =>
                {
                    foreach (var list in orderLists)
                    {
                        switch (e.Action)
                        {
                            case NotifyCollectionChangedAction.Add:
                                AddNewItems<T>(e, list);
                                break;
                            case NotifyCollectionChangedAction.Remove:
                                RemoveOldItems<T>(e, list);
                                break;
                            case NotifyCollectionChangedAction.Replace:
                                RemoveOldItems<T>(e, list);
                                AddNewItems<T>(e, list);
                                break;
                            case NotifyCollectionChangedAction.Reset:
                                list.Clear();
                                foreach (T item in sender as IEnumerable<T> ?? Array.Empty<T>())
                                {
                                    list.Add(item);
                                }
                                break;
                            case NotifyCollectionChangedAction.Move:
                            default:
                                break;
                        }
                    }
                };

            static void RemoveOldItems<T>(NotifyCollectionChangedEventArgs e, List<T> list)
            {
                foreach (T item in e.OldItems ?? Array.Empty<T>())
                {
                    list.Remove(item);
                }
            }

            static void AddNewItems<T>(NotifyCollectionChangedEventArgs e, List<T> list)
            {
                foreach (T item in e.NewItems ?? Array.Empty<T>())
                {
                    list.Add(item);
                }
            }
        }
    }
}
