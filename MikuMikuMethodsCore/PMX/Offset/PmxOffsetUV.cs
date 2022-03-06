using System.Numerics;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// UVモーフのオフセット
/// </summary>
public class PmxOffsetUV : IPmxOffset
{
    /// <summary>
    /// 対象頂点
    /// </summary>
    public PmxVertex Target { get; set; }
    /// <summary>
    /// 移動量
    /// </summary>
    public Vector4 Offset { get; set; }

    public override string ToString() => $"{Target.UV} : {Offset}";

    public PmxOffsetUV(PmxVertex target, Vector4 offset = default)
    {
        Target = target;
        Offset = offset;
    }
}
