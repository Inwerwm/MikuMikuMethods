﻿namespace MikuMikuMethods.Vmd;

/// <summary>
/// 表示・IKフレーム
/// </summary>
public class VmdPropertyFrame : VmdModelTypeFrame
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public override VmdFrameKind FrameKind => VmdFrameKind.Property;

    /// <summary>
    /// 表示/非表示
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// IK有効/無効
    /// </summary>
    public Dictionary<string, bool> IKEnabled { get; init; }

    /// <summary>
    /// フレーム時間を指定して <see cref="VmdPropertyFrame"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="frame">フレーム時間</param>
    public VmdPropertyFrame(uint frame = 0) : base("Property")
    {
        Frame = frame;
        IKEnabled = new();
    }

    /// <inheritdoc/>
    public override object Clone() => new VmdPropertyFrame(Frame)
    {
        IKEnabled = new(IKEnabled),
        IsVisible = IsVisible,
        Name = Name
    };
}
