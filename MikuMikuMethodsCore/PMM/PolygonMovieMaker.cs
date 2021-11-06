using MikuMikuMethods.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            AccessoriesSubstance.CollectionChanged += IRelationableElement<PmmAccessory>.SyncOrders(new[] { AccessoryRenderOrder });
            ModelsSubstance.CollectionChanged += IRelationableElement<PmmModel>.SyncOrders(new[] { ModelRenderOrder, ModelCalculateOrder });
        }

        /// <summary>
        /// ファイルから読み込む
        /// </summary>
        /// <param name="filePath">読み込むpmmファイル</param>
        public PolygonMovieMaker(string filePath) : this()
        {
            IO.PmmFileReader.Read(filePath, this);
        }
        public void SetRenderOrder(PmmAccessory accessory, byte renderOrder)
        {

        }

        public byte GetRenderOrder(PmmAccessory accessory)
        {
            throw new NotImplementedException();
        }

        public void SetRenderOrder(PmmModel model, byte renderOrder)
        {

        }

        public byte GetRenderOrder(PmmModel model)
        {
            throw new NotImplementedException();
        }

        public void SetCalculateOrder(PmmModel model, byte calculateOrder)
        {

        }

        public byte GetCalculateOrder(PmmModel model)
        {
            throw new NotImplementedException();
        }
    }
}
