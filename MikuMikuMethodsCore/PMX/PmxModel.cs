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
        /// <para>面</para>
        /// <para>全材質内の面リストを結合したもの</para>
        /// </summary>
        public List<PmxFace> Faces => Materials.SelectMany(m => m.Faces).ToList();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
