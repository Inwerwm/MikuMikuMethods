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

    /// <inheritdoc/>
    public override string ToString() => $"{Target.Position} : {Offset}";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="target">対象頂点</param>
    /// <param name="offset">移動量</param>
    public PmxOffsetVertex(PmxVertex target, Vector3 offset = default)
    {
        Target = target;
        Offset = offset;
    }
}
