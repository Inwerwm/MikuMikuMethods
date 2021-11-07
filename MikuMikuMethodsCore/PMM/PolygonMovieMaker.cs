using MikuMikuMethods.Common;
using MikuMikuMethods.Extension;
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
        internal ObservableCollection<PmmAccessory> AccessoriesSubstance { get; } = new();
        internal List<PmmAccessory> AccessoryRenderOrder { get; } = new();

        internal ObservableCollection<PmmModel> ModelsSubstance { get; } = new();
        internal List<PmmModel> ModelRenderOrder { get; } = new();
        internal List<PmmModel> ModelCalculateOrder { get; } = new();

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
        public IList<PmmAccessory> Accessories => AccessoriesSubstance;
        /// <summary>
        /// モデル
        /// </summary>
        public IList<PmmModel> Models => ModelsSubstance;

        /// <summary>
        /// 背景と音声
        /// </summary>
        public PmmBackGroundMedia BackGround { get; } = new();

        /// <summary>
        /// 描画設定
        /// </summary>
        public PmmRenderConfig RenderConfig { get; } = new();
        /// <summary>
        /// 編集対象の状態
        /// </summary>
        public PmmEditorState EditorState { get; } = new();
        /// <summary>
        /// 各種操作パネル画面の状態
        /// </summary>
        public PmmPanelPane PanelPane { get; } = new();
        /// <summary>
        /// 再生関連設定
        /// </summary>
        public PmmPlayConfig PlayConfig { get; } = new();

        public PolygonMovieMaker()
        {
            AccessoriesSubstance.CollectionChanged += CreateOrderListSynchronizer(new[] { AccessoryRenderOrder });
            ModelsSubstance.CollectionChanged += CreateOrderListSynchronizer(new[] { ModelRenderOrder, ModelCalculateOrder });
        }

        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイル名</param>
        public PolygonMovieMaker(string filePath) : this()
        {
            IO.PmmFileReader.Read(filePath, this);
        }

        /// <summary>
        /// ファイルに書き込む
        /// </summary>
        /// <param name="filePath">書き込むファイル名</param>
        public void Write(string filePath)
        {
            IO.PmmFileWriter.Write(filePath, this);
        }

        public void SetRenderOrder(PmmAccessory accessory, byte renderOrder)
        {
            if (!Accessories.Contains(accessory)) throw new ArgumentException("The accessory for which the Render Order setting was tried does not contain the PMM.");
            AccessoryRenderOrder.Move(renderOrder, accessory);
        }

        public byte? GetRenderOrder(PmmAccessory accessory)
        {
            int order = AccessoryRenderOrder.IndexOf(accessory);
            return order < 0 ? null : (byte)order;
        }

        public void SetRenderOrder(PmmModel model, byte renderOrder)
        {
            if (!Models.Contains(model)) throw new ArgumentException("The model for which the Render Order setting was tried does not contain the PMM.");
            ModelRenderOrder.Move(renderOrder, model);
        }

        public byte? GetRenderOrder(PmmModel model)
        {
            int order = ModelRenderOrder.IndexOf(model);
            return order < 0 ? null : (byte)order;
        }

        public void SetCalculateOrder(PmmModel model, byte calculateOrder)
        {
            if (!Models.Contains(model)) throw new ArgumentException("The model for which the Calculate Order setting was tried does not contain the PMM.");
            ModelCalculateOrder.Move(calculateOrder, model);
        }

        public byte? GetCalculateOrder(PmmModel model)
        {
            var order = ModelCalculateOrder.IndexOf(model);
            return order < 0 ? null : (byte)order;
        }

        static NotifyCollectionChangedEventHandler CreateOrderListSynchronizer<T>(IEnumerable<List<T>> orderLists) =>
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
                            list.Remove(item);
                        }
                        foreach (var item in e.NewItems?.Cast<T>().Select((Item, Id) => (Item, Id)))
                        {
                            list.Insert(e.OldStartingIndex + item.Id, item.Item);
                        }
                    }

                    void RemoveAll(IEnumerable<T> items)
                    {
                        foreach (T item in items)
                        {
                            list.Remove(item);
                        }
                    }

                    void AddAll(IEnumerable<T> items)
                    {
                        foreach (T item in items)
                        {
                            list.Add(item);
                        }
                    }
                }
            };
    }
}
