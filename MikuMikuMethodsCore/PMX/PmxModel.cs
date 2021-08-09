using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// PMXモデル
    /// </summary>
    public class PmxModel : IPmxData
    {
        /// <summary>
        /// ヘッダ
        /// </summary>
        public PmxHeader Header { get; } = new();
        /// <summary>
        /// モデル情報
        /// </summary>
        public PmxModelInfo ModelInfo { get; } = new();

        /// <summary>
        /// 剛体
        /// </summary>
        public List<PmxBody> Bodies { get; } = new();
        /// <summary>
        /// ボーン
        /// </summary>
        public List<PmxBone> Bones { get; } = new();
        /// <summary>
        /// 面
        /// </summary>
        public List<PmxFace> Faces { get; } = new();
        /// <summary>
        /// ジョイント
        /// </summary>
        public List<PmxJoint> Joints { get; } = new();
        /// <summary>
        /// 材質
        /// </summary>
        public List<PmxMaterial> Materials { get; } = new();
        /// <summary>
        /// モーフ
        /// </summary>
        public List<PmxMorph> Morphs { get; } = new();
        /// <summary>
        /// 表情枠
        /// </summary>
        public List<PmxNode> Nodes { get; } = new();
        /// <summary>
        /// 頂点
        /// </summary>
        public List<PmxVertex> Vertices { get; } = new();
        /// <summary>
        /// テクスチャ
        /// </summary>
        public List<PmxTexture> Textures { get; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmxModel() { }

        /// <summary>
        /// データをファイルに書き込む
        /// </summary>
        /// <param name="filePath">書き出すファイルのパス</param>
        public void Write(string filePath)
        {
            using (FileStream file = new(filePath, FileMode.Create))
            using (BinaryWriter writer = new(file, MikuMikuMethods.Encoding.ShiftJIS))
            {
                Write(writer);
            }
        }

        /// <summary>
        /// データをバイナリに書き込む
        /// </summary>
        /// <param name="writer">書き込み対象のライター</param>
        public void Write(BinaryWriter writer)
        {
            Header.Write(writer);
            ModelInfo.Write(writer, Encoder);
            WriteFrames(writer, Vertices, (writer, item) => item.Write(writer));
            WriteFrames(writer, Faces, (writer, item) => item.Write(writer));

            writer.Write(Textures.Count);
            foreach (var item in Textures)
            {
                writer.Write(item.Path);
            }

            WriteFrames(writer, Materials, (writer, item) => item.Write(writer));
            WriteFrames(writer, Bones, (writer, item) => item.Write(writer));
            WriteFrames(writer, Morphs, (writer, item) => item.Write(writer));
            WriteFrames(writer, Nodes, (writer, item) => item.Write(writer));
            WriteFrames(writer, Bodies, (writer, item) => item.Write(writer));
            WriteFrames(writer, Joints, (writer, item) => item.Write(writer));
        }

        private void WriteFrames<T>(BinaryWriter writer, List<T> list, Action<BinaryWriter, T> dataWriting) where T : IPmxData
        {
            writer.Write(list.Count);
            foreach (var item in list)
                dataWriting(writer, item);
        }
    }
}
