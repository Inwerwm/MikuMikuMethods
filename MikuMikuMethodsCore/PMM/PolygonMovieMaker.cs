using MikuMikuMethods.Common;
using System.Collections.Generic;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// MMDプロジェクトファイル
    /// </summary>
    public class PolygonMovieMaker
    {
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
        public List<PmmAccessory> Accessories { get; } = new();
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
    }
}
