using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// モデル情報
    /// </summary>
    public class PmxModelInfo : IPmxData
    {
        /// <summary>
        /// モデル名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// モデル名(英語)
        /// </summary>
        public string NameEn { get; set; }
        /// <summary>
        /// モデルのコメント
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// モデルのコメント(英語)
        /// </summary>
        public string CommentEn { get; set; }

        /// <summary>
        /// データをバイナリから読み込む
        /// </summary>
        /// <param name="reader">読み込み対象のリーダー</param>
        public void Read(BinaryReader reader)
        {
            Name = reader.ReadString();
            NameEn = reader.ReadString();
            Comment = reader.ReadString();
            CommentEn = reader.ReadString();
        }

        /// <summary>
        /// データをバイナリで書き込む
        /// </summary>
        /// <param name="writer">書き込み対象のライター</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(NameEn);
            writer.Write(Comment);
            writer.Write(CommentEn);
        }
    }
}
