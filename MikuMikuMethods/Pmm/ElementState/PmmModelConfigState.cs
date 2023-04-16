namespace MikuMikuMethods.Pmm.ElementState;

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

    public PmmModelConfigState DeepCopy() => new()
    {
        Visible = Visible,
        EnableIK = new(EnableIK),
        OutsideParent = OutsideParent.ToDictionary(p => p.Key, p => p.Value.DeepCopy()),
    };

    public PmmModelConfigState DeepCopy(Dictionary<PmmBone, bool> enableIK, Dictionary<PmmBone, PmmOutsideParentState> outsideParent) => new()
    {
        Visible = Visible,
        EnableIK = enableIK,
        OutsideParent = outsideParent,
    };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
