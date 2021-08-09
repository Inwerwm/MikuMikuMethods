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
        /// <param name="encoder">エンコード用クラス</param>
        internal void Read(BinaryReader reader, StringEncoder encoder)
        {
            Name = encoder.Read(reader);
            NameEn = encoder.Read(reader);
            Comment = encoder.Read(reader);
            CommentEn = encoder.Read(reader);
        }

        /// <summary>
        /// データをバイナリで書き込む
        /// </summary>
        /// <param name="writer">書き込み対象のライター</param>
        /// <param name="encoder">エンコード用クラス</param>
        internal void Write(BinaryWriter writer, StringEncoder encoder)
        {
            encoder.Write(writer, Name);
            encoder.Write(writer, NameEn);
            encoder.Write(writer, Comment);
            encoder.Write(writer, CommentEn);
        }
    }
}
