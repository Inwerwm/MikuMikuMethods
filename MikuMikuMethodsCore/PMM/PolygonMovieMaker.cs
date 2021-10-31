using MikuMikuMethods.Common;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// MMDプロジェクトファイル
    /// </summary>
    public class PolygonMovieMaker
    {
        internal ObservableCollection<PmmAccessory> _Accessories { get; } = new();
        internal List<PmmAccessory> _AccessoryRenderOrder { get; } = new();

        internal ObservableCollection<PmmModel> _Models { get; } = new();
        internal List<PmmModel> _ModelRenderOrder { get; } = new();
        internal List<PmmModel> _ModelCalculateOrder { get; } = new();

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
        public IList<PmmModel> Models => _Models;

        /// <summary>
        /// 背景と音声
        /// </summary>
        public PmmBackGroundMedia BackGround { get; } = new();

        /// <summary>
        /// 3Dビュー画面の状態
        /// </summary>
        public PmmRenderPane RenderPane { get; } = new();
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
            _Accessories.CollectionChanged += IRelationableElement<PmmAccessory>.SyncOrders(new[] { _AccessoryRenderOrder });
            _Models.CollectionChanged += IRelationableElement<PmmModel>.SyncOrders(new[] { _ModelRenderOrder, _ModelCalculateOrder });
        }
    }
}
