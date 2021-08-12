using MikuMikuMethods.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// 頂点
    /// </summary>
    public class PmxVertex : IPmxData
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 法線
        /// </summary>
        public Vector3 Normal { get; set; }

        /// <summary>
        /// テクスチャのUV座標
        /// </summary>
        public Vector2 UV { get; set; }

        /// <summary>
        /// 追加UVの座標
        /// </summary>
        public Vector4[] AdditonalUVs { get; set; }

        /// <summary>
        /// ウェイト変形方式
        /// </summary>
        public PmxWeightType WeightType { get; set; }

        /// <summary>
        /// ウェイト値
        /// </summary>
        public List<PmxWeight> Weights { get; } = new();

        /// <summary>
        /// SDEFパラメータ
        /// </summary>
        public record SDEFParams(Vector3 C, Vector3 R0, Vector3 R1);
        /// <summary>
        /// SDEFパラメータ
        /// </summary>
        public SDEFParams SDEF { get; set; } = new(new(), new(), new());

        /// <summary>
        /// エッジ倍率
        /// </summary>
        public float EdgeScale { get; set; }

        public override string ToString() => $"{Position}";
    }
}
