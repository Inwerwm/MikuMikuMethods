using System.Numerics;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// 辺
/// </summary>
public class PmxEdge
{
    /// <summary>
    /// 構成頂点
    /// </summary>
    public PmxVertex[] Vertices { get; } = new PmxVertex[2];

    /// <summary>
    /// 辺を反転する
    /// </summary>
    public void Invert()
    {
        (Vertices[1], Vertices[0]) = (Vertices[0], Vertices[1]);
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="start">起点</param>
    /// <param name="end">終点</param>
    public PmxEdge(PmxVertex start, PmxVertex end)
    {
        Vertices[0] = start;
        Vertices[1] = end;
    }

    /// <summary>
    /// ベクトルに変換
    /// </summary>
    public Vector3 ToVector() => new(
            Vertices[1].Position.X - Vertices[0].Position.X,
            Vertices[1].Position.Y - Vertices[0].Position.Y,
            Vertices[1].Position.Z - Vertices[0].Position.Z
        );
}
