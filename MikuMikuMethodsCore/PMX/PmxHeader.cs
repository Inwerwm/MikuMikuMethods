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
        public float Version => 2.1f;

        /// <summary>
        /// モデルの構成情報の要素数
        /// </summary>
        private byte ConfigSize => 8;

        /// <summary>
        /// モデルの構成情報
        /// </summary>
        public ModelConfig Config { get; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmxHeader(){}

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

        /// <summary>
        /// モデルの各種情報
        /// </summary>
        public record ModelConfig
        {
            private static byte[] ZeroOrOne = { 0, 1 };
            private static byte[] ZeroToFour = { 0, 1, 2, 3, 4 };
            private static byte[] SizeNum = { 1, 2, 4 };

            private byte encodingFormat;
            private byte numOfAdditionalUV;
            private byte sizeOfVertexIndex;
            private byte sizeOfTextureIndex;
            private byte sizeOfMaterialIndex;
            private byte sizeOfBoneIndex;
            private byte sizeOfMorphIndex;
            private byte sizeOfBodyIndex;

            /// <summary>
            /// エンコード方式
            ///     <list type="table">
            ///         <item>
            ///             <term>0</term>
            ///             <description>UTF16</description>
            ///         </item>
            ///         <item>
            ///             <term>1</term>
            ///             <description>UTF8</description>
            ///         </item>
            ///     </list>
            /// </summary>
            public byte EncodingFormat
            {
                get => encodingFormat;
                set
                {
                    if (ZeroOrOne.Contains(value))
                        encodingFormat = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
            }
            /// <summary>
            /// <term>追加UV数</term>
            /// <description>0 ～ 4</description>
            /// </summary>
            public byte NumOfAdditionalUV
            {
                get => numOfAdditionalUV;
                set
                {
                    if (ZeroToFour.Contains(value))
                        numOfAdditionalUV = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
            }
            /// <summary>
            /// <term>頂点Indexサイズ</term>
            /// <description>1 | 2 | 4</description>
            /// </summary>
            public byte SizeOfVertexIndex
            {
                get => sizeOfVertexIndex;
                set
                {
                    if (SizeNum.Contains(value))
                        sizeOfVertexIndex = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
            }
            /// <summary>
            /// <term>テクスチャIndexサイズ</term>
            /// <description>1 | 2 | 4</description>
            /// </summary>
            public byte SizeOfTextureIndex
            {
                get => sizeOfTextureIndex;
                set
                {
                    if (SizeNum.Contains(value))
                        sizeOfTextureIndex = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
            }
            /// <summary>
            /// <term>材質Indexサイズ</term>
            /// <description>1 | 2 | 4</description>
            /// </summary>
            public byte SizeOfMaterialIndex
            {
                get => sizeOfMaterialIndex;
                set
                {
                    if (SizeNum.Contains(value))
                        sizeOfMaterialIndex = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
            }
            /// <summary>
            /// <term>ボーンIndexサイズ</term>
            /// <description>1 | 2 | 4</description>
            /// </summary>
            public byte SizeOfBoneIndex
            {
                get => sizeOfBoneIndex;
                set
                {
                    if (SizeNum.Contains(value))
                        sizeOfBoneIndex = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
            }
            /// <summary>
            /// <term>モーフIndexサイズ</term>
            /// <description>1 | 2 | 4</description>
            /// </summary>
            public byte SizeOfMorphIndex
            {
                get => sizeOfMorphIndex;
                set
                {
                    if (SizeNum.Contains(value))
                        sizeOfMorphIndex = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
            }
            /// <summary>
            /// <term>剛体Indexサイズ</term>
            /// <description>1 | 2 | 4</description>
            /// </summary>
            public byte SizeOfBodyIndex
            {
                get => sizeOfBodyIndex;
                set
                {
                    if (SizeNum.Contains(value))
                        sizeOfBodyIndex = value;
                    else
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
