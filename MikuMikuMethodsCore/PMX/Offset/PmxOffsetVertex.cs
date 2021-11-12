using System.Numerics;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// 頂点モーフ
/// </summary>
public class PmxOffsetVertex : IPmxOffset
{
    /// <summary>
    /// 対象頂点
    /// </summary>
    public PmxVertex Target { get; set; }

    /// <summary>
    /// 移動量
    /// </summary>
    public Vector3 Offset { get; set; }

    public override string ToString() => $"{Target.Position} : {Offset}";
}
