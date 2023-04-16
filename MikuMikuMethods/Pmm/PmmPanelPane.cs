namespace MikuMikuMethods.Pmm;

public class PmmPanelPane : ICloneable
{
    /// <summary>
    /// カメラ操作パネル開閉状態
    /// </summary>
    public bool DoesOpenCameraPanel { get; set; } = true;
    /// <summary>
    /// ライト操作パネル開閉状態
    /// </summary>
    public bool DoesOpenLightPanel { get; set; } = true;
    /// <summary>
    /// アクセサリ操作パネル開閉状態
    /// </summary>
    public bool DoesOpenAccessaryPanel { get; set; } = true;
    /// <summary>
    /// ボーン操作パネル開閉状態
    /// </summary>
    public bool DoesOpenBonePanel { get; set; } = true;
    /// <summary>
    /// 表情操作パネル開閉状態
    /// </summary>
    public bool DoesOpenMorphPanel { get; set; } = true;
    /// <summary>
    /// セルフ影操作パネル開閉状態
    /// </summary>
    public bool DoesOpenSelfShadowPanel { get; set; } = true;

    public object Clone() => DeepCopy();

    public PmmPanelPane DeepCopy() => new()
    {
        DoesOpenAccessaryPanel = DoesOpenAccessaryPanel,
        DoesOpenBonePanel = DoesOpenBonePanel,
        DoesOpenCameraPanel = DoesOpenCameraPanel,
        DoesOpenLightPanel = DoesOpenLightPanel,
        DoesOpenMorphPanel = DoesOpenMorphPanel,
        DoesOpenSelfShadowPanel = DoesOpenSelfShadowPanel,
    };
}
