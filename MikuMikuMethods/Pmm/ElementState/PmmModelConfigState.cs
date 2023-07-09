namespace MikuMikuMethods.Pmm.ElementState;

/// <summary>
/// モデル設定の状態
/// </summary>
public class PmmModelConfigState : ICloneable
{
    /// <summary>
    /// 表示
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// IKが有効か
    /// </summary>
    public Dictionary<PmmBone, bool> EnableIK { get; protected internal init; } = new();
    /// <summary>
    /// 外部親設定
    /// <list>
    ///     <item>
    ///         <term>Key</term>
    ///         <description>子側ボーン</description>
    ///     </item>
    ///     <item>
    ///         <term>Value</term>
    ///         <description>外部親設定情報</description>
    ///     </item>
    /// </list>
    /// </summary>
    public Dictionary<PmmBone, PmmOutsideParentState> OutsideParent { get; protected init; } = new();

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmModelConfigState DeepCopy() => new()
    {
        Visible = Visible,
        EnableIK = new(EnableIK),
        OutsideParent = OutsideParent.ToDictionary(p => p.Key, p => p.Value.DeepCopy()),
    };

    /// <summary>
    /// モデル移行用ディープコピー
    /// </summary>
    /// <param name="boneMap">移行前ボーンと移行先ボーンの対応辞書</param>
    /// <returns>複製</returns>
    public PmmModelConfigState DeepCopy(Dictionary<PmmBone, PmmBone> boneMap) => new()
    {
        Visible = Visible,
        EnableIK = EnableIK.Where(p => boneMap.ContainsKey(p.Key)).ToDictionary(p => boneMap[p.Key], p => p.Value),
        OutsideParent = OutsideParent.SelectKeyValue(boneMap, null).ToDictionary(p => p.Key, p => p.Value.DeepCopy()),
    };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
