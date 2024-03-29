﻿namespace MikuMikuMethods.Pmx;

/// <summary>
/// 表示枠
/// </summary>
public class PmxNode : IPmxData
{
    /// <summary>
    /// 名前
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// 英名
    /// </summary>
    public string NameEn { get; set; } = "";

    /// <summary>
    /// 特殊枠か
    /// </summary>
    public bool IsSpecific { get; set; }

    /// <summary>
    /// 表情枠内の要素
    /// </summary>
    public List<IPmxNodeElement> Elements { get; } = new();

    /// <inheritdoc/>
    public override string ToString() => $"{Name}";
}
