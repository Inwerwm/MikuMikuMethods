using System;
using System.Linq;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// モデルの各種情報
    /// </summary>
    public record PmxModelConfig
    {
        private static byte[] ZeroAndOne = { 0, 1 };
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
                if (ZeroAndOne.Contains(value))
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

        public PmxModelConfig(byte encodingFormat, byte numOfAdditionalUV, byte sizeOfVertexIndex, byte sizeOfTextureIndex, byte sizeOfMaterialIndex, byte sizeOfBoneIndex, byte sizeOfMorphIndex, byte sizeOfBodyIndex)
        {
            EncodingFormat = encodingFormat;
            NumOfAdditionalUV = numOfAdditionalUV;
            SizeOfVertexIndex = sizeOfVertexIndex;
            SizeOfTextureIndex = sizeOfTextureIndex;
            SizeOfMaterialIndex = sizeOfMaterialIndex;
            SizeOfBoneIndex = sizeOfBoneIndex;
            SizeOfMorphIndex = sizeOfMorphIndex;
            SizeOfBodyIndex = sizeOfBodyIndex;
        }

        internal PmxModelConfig(byte[] config)
        {
            EncodingFormat = config[0];
            NumOfAdditionalUV = config[1];
            SizeOfVertexIndex = config[2];
            SizeOfTextureIndex = config[3];
            SizeOfMaterialIndex = config[4];
            SizeOfBoneIndex = config[5];
            SizeOfMorphIndex = config[6];
            SizeOfBodyIndex = config[7];
        }

        public byte[] ToArray() => new[]
        {
            EncodingFormat,
            NumOfAdditionalUV,
            SizeOfVertexIndex,
            SizeOfTextureIndex,
            SizeOfMaterialIndex,
            SizeOfBoneIndex,
            SizeOfMorphIndex,
            SizeOfBodyIndex
        };
    }
}
