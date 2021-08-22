namespace MikuMikuMethods.PMM
{
    public class PmmRenderPane
    {
        /// <summary>
        /// 描画情報の表示/非表示
        /// </summary>
        public bool VisibleInfomation { get; set; } = false;

        /// <summary>
        /// 座標軸の表示/非表示
        /// </summary>
        public bool VisibleAxis { get; set; } = true;

        /// <summary>
        /// 3Dビュー上部のフレーム入力欄の値
        /// </summary>
        public int FrameLocation { get; set; } = 0;

        public enum FPSLimitValue
        {
            Half = 30,
            Full = 60,
            Unlimited = 1000
        }
        /// <summary>
        /// FPS制限
        /// </summary>
        public FPSLimitValue FPSLimit { get; set; } = FPSLimitValue.Half;

        public enum ScreenCaptureModeType : int
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
        public ScreenCaptureModeType ScreenCaptureMode { get; set; }

        /// <summary>
        /// モデルより後に描画するアクセサリのインデックス
        /// </summary>
        public int PostDrawingAccessoryStartIndex { get; set; }

        /// <summary>
        /// 地面影の表示/非表示
        /// </summary>
        public bool EnableGrandShadow { get; set; }
        /// <summary>
        /// 地面影の明るさ
        /// </summary>
        public float GroundShadowBrightness { get; set; }
        /// <summary>
        /// 地面影の透明化
        /// </summary>
        public bool EnableTransparentGroundShadow { get; set; }

    }
}