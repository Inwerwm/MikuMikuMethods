using System.Drawing;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// レンダー設定
/// </summary>
public class PmmRenderConfig : ICloneable
{
    /// <summary>
    /// 描画情報の表示/非表示
    /// </summary>
    public bool InformationVisible { get; set; } = false;

    /// <summary>
    /// 座標軸の表示/非表示
    /// </summary>
    public bool AxisVisible { get; set; } = true;

    /// <summary>
    /// 3Dビュー上部のフレーム入力欄の値
    /// </summary>
    public int JumpFrameLocation { get; set; } = 0;

    /// <summary>
    /// FPS制限
    /// </summary>
    public enum FPSLimitValue
    {
        /// <summary>
        /// 30
        /// </summary>
        Half = 30,
        /// <summary>
        /// 60
        /// </summary>
        Full = 60,
        /// <summary>
        /// 無制限
        /// </summary>
        Unlimited = 1000
    }
    /// <summary>
    /// FPS制限
    /// </summary>
    public FPSLimitValue FPSLimit { get; set; } = FPSLimitValue.Half;

    /// <summary>
    /// スクリーン用キャプチャモード
    /// </summary>
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
    /// <summary>
    /// エッジの色
    /// </summary>
    public Color EdgeColor { get; set; }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmRenderConfig DeepCopy() => new()
    {
        AxisVisible = AxisVisible,
        JumpFrameLocation = JumpFrameLocation,
        EdgeColor = EdgeColor,
        EnableGrandShadow = EnableGrandShadow,
        EnableTransparentGroundShadow = EnableTransparentGroundShadow,
        FPSLimit = FPSLimit,
        ScreenCaptureMode = ScreenCaptureMode,
        GroundShadowBrightness = GroundShadowBrightness,
        InformationVisible = InformationVisible,
        PostDrawingAccessoryStartIndex = PostDrawingAccessoryStartIndex,
    };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
