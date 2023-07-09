using MikuMikuMethods.Extension;
using System.Numerics;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// 面
/// </summary>
public class PmxFace : IPmxData
{
    /// <summary>
    /// 構成頂点のリスト
    /// </summary>
    public PmxVertex[] Vertices { get; }

    /// <summary>
    /// 面法線
    /// </summary>
    public Vector3 Normal => Vector3.Normalize(Vector3.Cross(FetchEdge(0, 1).ToVector(), FetchEdge(0, 2).ToVector()));

    /// <summary>
    /// 面を反転
    /// </summary>
    public void Invert() => Utility.Swap(ref Vertices[1], ref Vertices[2]);

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PmxFace(PmxVertex vertex0, PmxVertex vertex1, PmxVertex vertex2)
    {
        Vertices = new PmxVertex[] { vertex0, vertex1, vertex2 };
    }

    /// <summary>
    /// 指定頂点の構成辺を取得
    /// </summary>
    /// <param name="startIndex">辺の起点頂点の面内頂点番号を指定</param>
    /// <param name="endIndex">辺の終点頂点の面内頂点番号を指定</param>
    /// <returns>辺</returns>
    public PmxEdge FetchEdge(int startIndex, int endIndex) =>
        startIndex.IsWithin(0, 2) && endIndex.IsWithin(0, 2) && startIndex != endIndex
            ? new PmxEdge(Vertices[startIndex], Vertices[endIndex])
            : throw new IndexOutOfRangeException($"PmxFace.FetchEdgeが不正な引数({startIndex}, {endIndex})によって呼び出されました。");

    /// <summary>
    /// 辺の集合に変換
    /// </summary>
    /// <returns>辺の集合</returns>
    public IEnumerable<PmxEdge> ToEdges() => new PmxEdge[]
    {
            FetchEdge(0, 1),
            FetchEdge(1, 2),
            FetchEdge(2, 0)
    };

    /// <inheritdoc/>
    public override string ToString() => "{ " + Vertices.Select(v => v.ToString()).Aggregate((acm, elm) => $"{acm}, {elm}") + " }";
}
