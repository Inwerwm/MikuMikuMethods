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

    public override string ToString() => $"{Target.Name} - {Ratio:###.00}";

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
