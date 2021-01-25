using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// 辺
    /// </summary>
    public class PmxEdge
    {
        /// <summary>
        /// 構成頂点
        /// </summary>
        public PmxVertex[] Vertices { get; }

        /// <summary>
        /// 辺を反転する
        /// </summary>
        public void Invert()
        {
            Utility.Swap(ref Vertices[0], ref Vertices[1]);
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
        public Vector3 ToVector() => new Vector3(
                Vertices[1].Position.X - Vertices[0].Position.X,
                Vertices[1].Position.Y - Vertices[0].Position.Y,
                Vertices[1].Position.Z - Vertices[0].Position.Z
            );
    }
}
