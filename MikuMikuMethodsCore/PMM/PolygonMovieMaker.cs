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
        public string Version { get; internal set; }
        /// <summary>
        /// 出力解像度
        /// </summary>
        public Size OutputResolution { get; set; }

        /// <summary>
        /// カメラ
        /// </summary>
        public PmmCamera Camera { get; }
        /// <summary>
        /// 照明
        /// </summary>
        public PmmLight Light { get; }
        /// <summary>
        /// セルフ影
        /// </summary>
        public PmmSelfShadow SelfShadow { get; }
        /// <summary>
        /// 物理
        /// </summary>
        public PmmPhysics Physics { get; }
        /// <summary>
        /// アクセサリー
        /// </summary>
        public List<PmmAccessory> Accessories { get; }
        /// <summary>
        /// モデル
        /// </summary>
        public List<PmmModel> Models { get; }

        /// <summary>
        /// 背景と音声
        /// </summary>
        public PmmMedia Media { get; }

        /// <summary>
        /// 3Dビュー画面の状態
        /// </summary>
        public PmmRenderPane RenderPane { get; }
        /// <summary>
        /// キーフレーム編集画面の状態
        /// </summary>
        public PmmKeyFramePane KeyFramePane { get; }
        /// <summary>
        /// 各種操作パネル画面の状態
        /// </summary>
        public PmmPanelPane PanelPane { get; }
    }
}
