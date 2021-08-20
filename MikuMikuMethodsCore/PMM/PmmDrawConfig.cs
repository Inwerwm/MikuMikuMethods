using System.Drawing;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// 描画関連設定
    /// </summary>
    public class PmmDrawConfig
    {
        /// <summary>
        /// 描画情報の表示/非表示
        /// </summary>
        public bool IsShowInfomation { get; set; }
        /// <summary>
        /// 座標軸の表示/非表示
        /// </summary>
        public bool IsShowAxis { get; set; }

        /// <summary>
        /// 背景色は黒か
        /// </summary>
        public bool IsBackgroundBlack { get; set; }
        /// <summary>
        /// エッジの色
        /// </summary>
        public Color EdgeColor { get; set; }

        /// <summary>
        /// FPS制限量
        /// </summary>
        public float FPSLimit { get; set; }

        /// <summary>
        /// スクリーン用キャプチャモード
        /// </summary>
        public enum ScreenCaptureMode : int
        {
            /// <summary>
            /// オフ
            /// </summary>
            Off,
            /// <summary>
            /// 全画面
            /// </summary>
            FullScreen,
            /// <summary>
            /// 4:3比率
            /// </summary>
            Square,
            /// <summary>
            /// 背景AVI
            /// </summary>
            BackgroundVideo
        }
        /// <summary>
        /// スクリーン用キャプチャモード設定
        /// </summary>
        public ScreenCaptureMode ScreenCaptureSetting { get; set; }

        /// <summary>
        /// モデルより後に描画するアクセサリのインデックス
        /// </summary>
        public int AccessoryModelThreshold { get; set; }

        /// <summary>
        /// 地面影の表示/非表示
        /// </summary>
        public bool IsShowGrandShadow { get; set; }
        /// <summary>
        /// 地面影の明るさ
        /// </summary>
        public float GroundShadowBrightness { get; set; }
        /// <summary>
        /// 地面影の透明化
        /// </summary>
        public bool EnableTransparentGroundShadow { get; set; }

        /// <summary>
        /// 物理演算モード
        /// </summary>
        public enum PhysicsMode : byte
        {
            /// <summary>
            /// 演算しない
            /// </summary>
            Disable,
            /// <summary>
            /// 常に演算
            /// </summary>
            Always,
            /// <summary>
            /// オン/オフモード
            /// </summary>
            Switchable,
            /// <summary>
            /// トレースモード
            /// </summary>
            Trace
        }
        /// <summary>
        /// 物理演算モード
        /// </summary>
        public PhysicsMode PhysicsSetting { get; set; }
        /// <summary>
        /// 物理床のOn/Off
        /// </summary>
        public bool EnableGroundPhysics { get; set; }
    }
}
