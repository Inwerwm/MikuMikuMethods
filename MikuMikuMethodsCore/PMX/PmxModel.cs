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
        /// ファイル読込コンストラクタ
        /// </summary>
        /// <param name="filePath">Pmmファイルのパス</param>
        public PmxModel(string filePath) : this()
        {
            Read(filePath);
        }

        /// <summary>
        /// バイナリ読込コンストラクタ
        /// </summary>
        /// <param name="reader">PMMファイル</param>
        public PmxModel(BinaryReader reader) : this()
        {
            Read(reader);
        }

        /// <summary>
        /// データをファイルから読み込む
        /// </summary>
        /// <param name="filePath">読み込むファイルのパス</param>
        public void Read(string filePath)
        {
            using (FileStream stream = new(filePath, FileMode.Open))
            using (BinaryReader reader = new(stream, MikuMikuMethods.Encoding.ShiftJIS))
            {
                Read(reader);
            }
        }

        /// <summary>
        /// データをバイナリから読み込む
        /// </summary>  
        /// <param name="reader">読み込み対象のリーダー</param>
        public void Read(BinaryReader reader)
        {
            Header.Read(reader);
            ModelInfo.Read(reader);
            ReadFrames(reader, r => Vertices.Add(new(r)));
            ReadFrames(reader, r => Faces.Add(new(r)));
            ReadFrames(reader, r => Textures.Add(new(reader.ReadString()))); // 仮
            ReadFrames(reader, r => Materials.Add(new(r)));
            ReadFrames(reader, r => Bones.Add(new(r)));
            ReadFrames(reader, r => Morphs.Add(new(r)));
            ReadFrames(reader, r => Nodes.Add(new(r)));
            ReadFrames(reader, r => Bodies.Add(new(r)));
            ReadFrames(reader, r => Joints.Add(new(r)));
        }

        private void ReadFrames(BinaryReader reader, Action<BinaryReader> addElementToList)
        {
            var elementNum = reader.ReadUInt32();
            for (int i = 0; i < elementNum; i++)
                addElementToList(reader);
        }

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
            ModelInfo.Write(writer);
            WriteFrames(writer, Vertices);
            WriteFrames(writer, Faces);

            writer.Write(Textures.Count);
            foreach (var item in Textures)
            {
                writer.Write(item.Path);
            }

            WriteFrames(writer, Materials);
            WriteFrames(writer, Bones);
            WriteFrames(writer, Morphs);
            WriteFrames(writer, Nodes);
            WriteFrames(writer, Bodies);
            WriteFrames(writer, Joints);
        }

        private void WriteFrames<T>(BinaryWriter writer, List<T> list) where T : IPmxData
        {
            writer.Write(list.Count);
            foreach (var item in list)
                item.Write(writer);
        }
    }
}
