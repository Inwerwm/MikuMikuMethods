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

        /// <summary>
        /// データをバイナリで書き込む
        /// </summary>
        /// <param name="writer">書き込み対象のライター</param>
        public void Write(BinaryWriter writer, Indexer boneIndexer, IEnumerable<PmxBone> bones)
        {
            writer.Write(Position);
            writer.Write(Normal);
            writer.Write(UV);

            foreach (var item in AdditonalUVs)
            {
                writer.Write(item);
            }

            writer.Write((byte)WeightType);

            var boneIdMap = bones.Select((Bone, Index) => (Bone, Index)).ToDictionary(b => b.Bone, b => b.Index);
            switch (WeightType)
            {
                case PmxWeightType.BDEF1:
                    WriteBDEF1Weights(writer, boneIndexer, boneIdMap);
                    break;
                case PmxWeightType.BDEF2:
                    WriteBDEF2Weights(writer, boneIndexer, boneIdMap);
                    break;
                case PmxWeightType.BDEF4:
                    WriteBDEF4Weights(writer, boneIndexer, boneIdMap);
                    break;
                case PmxWeightType.SDEF:
                    WriteSDEFWeights(writer, boneIndexer, boneIdMap);
                    break;
            }

            writer.Write(EdgeScale);
        }

        private void WriteBDEF1Weights(BinaryWriter writer, Indexer boneIndexer, Dictionary<PmxBone, int> boneIdMap)
        {
            boneIndexer.Write(writer, boneIdMap[Weights[0].Bone]);
        }

        private void WriteBDEF2Weights(BinaryWriter writer, Indexer boneIndexer, Dictionary<PmxBone, int> boneIdMap)
        {
            boneIndexer.Write(writer, boneIdMap[Weights[0].Bone]);
            boneIndexer.Write(writer, boneIdMap[Weights[1].Bone]);
            writer.Write(Weights[0].Value);
        }

        private void WriteBDEF4Weights(BinaryWriter writer, Indexer boneIndexer, Dictionary<PmxBone, int> boneIdMap)
        {
            boneIndexer.Write(writer, boneIdMap[Weights[0].Bone]);
            boneIndexer.Write(writer, boneIdMap[Weights[1].Bone]);
            boneIndexer.Write(writer, boneIdMap[Weights[2].Bone]);
            boneIndexer.Write(writer, boneIdMap[Weights[3].Bone]);
            writer.Write(Weights[0].Value);
            writer.Write(Weights[1].Value);
            writer.Write(Weights[2].Value);
            writer.Write(Weights[3].Value);
        }

        private void WriteSDEFWeights(BinaryWriter writer, Indexer boneIndexer, Dictionary<PmxBone, int> boneIdMap)
        {
            boneIndexer.Write(writer, boneIdMap[Weights[0].Bone]);
            boneIndexer.Write(writer, boneIdMap[Weights[1].Bone]);
            writer.Write(Weights[0].Value);

            writer.Write(SDEF.C);
            writer.Write(SDEF.R0);
            writer.Write(SDEF.R1);
        }
    }
}
