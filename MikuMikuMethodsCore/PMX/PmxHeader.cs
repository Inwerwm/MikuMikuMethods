using System;
using System.IO;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// ヘッダー
    /// </summary>
    public class PmxHeader : IPmxData
    {
        /// <summary>
        /// フォーマット名
        /// </summary>
        public string FormatName => "PMX ";

        /// <summary>
        /// PMXのバージョン
        /// </summary>
        public float Version => 2.1f;

        /// <summary>
        /// モデルの構成情報の要素数
        /// </summary>
        private byte ConfigSize { get; set; } = 8;

        /// <summary>
        /// モデルの構成情報
        /// </summary>
        public PmxModelConfig Config { get; private set; }

        /// <summary>
        /// モデルの使用エンコード方式
        /// </summary>
        public System.Text.Encoding EncodingOfModel => Config.EncodingFormat switch
        {
            0 => System.Text.Encoding.Unicode,
            1 => System.Text.Encoding.UTF8,
            _ => throw new FormatException("エンコード情報が不正です。")
        };

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmxHeader() { }

        /// <summary>
        /// データをバイナリから読み込む
        /// </summary>
        /// <param name="reader">読み込み対象のリーダー</param>
        public void Read(BinaryReader reader)
        {
            // "PMX "
            reader.ReadBytes(4);
            var version = reader.ReadSingle();
            if (version < 2.0) throw new FormatException("PMXが非対応バージョンです。バージョン番号が未対応バージョンです。");

            ConfigSize = reader.ReadByte();
            if(ConfigSize != 8) throw new FormatException("PMXが非対応バージョンです。ヘッダデータが未対応の形式です。");
            Config = new PmxModelConfig(reader.ReadBytes(ConfigSize));
        }

        /// <summary>
        /// データをバイナリで書き込む
        /// </summary>
        /// <param name="writer">書き込み対象のライター</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(FormatName.ToCharArray(), 0, 4);
            writer.Write(Version);

            writer.Write(ConfigSize);
            writer.Write(Config.ToArray());
        }
    }
}
