using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public float Version => 2.0f;

        /// <summary>
        /// モデルの構成情報の要素数
        /// </summary>
        private byte ModelSize => 8;

        /// <summary>
        /// モデルの構成情報
        /// <list type="number">
        ///     <item>
        ///         <term>エンコード方式</term>
        ///         <description>0 : UTF16, 1 : UTF8</description>
        ///     </item>
        ///     <item>
        ///         <term>追加UV数</term>
        ///         <description>[0-4]</description>
        ///     </item>
        ///     <item>
        ///         <term>頂点Indexサイズ</term>
        ///         <description>1 | 2 | 4</description>
        ///     </item>
        ///     <item>
        ///         <term>テクスチャIndexサイズ</term>
        ///         <description>1 | 2 | 4</description>
        ///     </item>
        ///     <item>
        ///         <term>材質Indexサイズ</term>
        ///         <description>1 | 2 | 4</description>
        ///     </item>
        ///     <item>
        ///         <term>ボーンIndexサイズ</term>
        ///         <description>1 | 2 | 4</description>
        ///     </item>
        ///     <item>
        ///         <term>モーフIndexサイズ</term>
        ///         <description>1 | 2 | 4</description>
        ///     </item>
        ///     <item>
        ///         <term>剛体Indexサイズ</term>
        ///         <description>1 | 2 | 4</description>
        ///     </item>
        /// </list>
        /// </summary>
        public byte[] ModelStructureInfo { get; }

        /// <summary>
        /// データをバイナリから読み込む
        /// </summary>
        /// <param name="reader">読み込み対象のリーダー</param>
        public void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// データをバイナリで書き込む
        /// </summary>
        /// <param name="writer">書き込み対象のライター</param>
        public void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
