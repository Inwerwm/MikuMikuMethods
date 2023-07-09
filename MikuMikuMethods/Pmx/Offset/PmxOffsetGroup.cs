namespace MikuMikuMethods.Pmx;

/// <summary>
/// グループモーフのオフセット
/// </summary>
public class PmxOffsetGroup : IPmxOffset
{
    /// <summary>
    /// 対象モーフ
    /// </summary>
    public PmxMorph Target { get; set; }
    /// <summary>
    /// 適用率
    /// </summary>
    public float Ratio { get; set; }

    /// <inheritdoc/>
    public override string ToString() => $"{Target.Name} - {Ratio:###.00}";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="target">対象モーフ</param>
    /// <param name="ratio">適用率</param>
    public PmxOffsetGroup(PmxMorph target, float ratio = default)
    {
        Target = target;
        Ratio = ratio;
    }

    internal PmxOffsetGroup()
    {
        Target = null!;
    }
}
