namespace MikuMikuMethods.Pmx;

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
    public float Version
    {
        get => version;
        set
        {
            if (value < 2.0 || 2.1 < value)
                throw new ArgumentOutOfRangeException(nameof(Version), "対応バージョンは2.0～2.1までです。");
            version = value;
        }
    }

    /// <summary>
    /// モデルの構成情報の要素数
    /// </summary>
    public byte ConfigSize => 8;

    private static byte[] ZeroAndOne = { 0, 1 };
    private static byte[] RangeOfAdditionalUV = { 0, 1, 2, 3, 4 };
    private static byte[] IndexSize = { 1, 2, 4 };

    private byte encodingFormat;
    private byte numOfAdditionalUV;
    private byte sizeOfVertexIndex;
    private byte sizeOfTextureIndex;
    private byte sizeOfMaterialIndex;
    private byte sizeOfBoneIndex;
    private byte sizeOfMorphIndex;
    private byte sizeOfBodyIndex;
    private float version;

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
                throw new ArgumentOutOfRangeException(nameof(EncodingFormat));
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
            if (RangeOfAdditionalUV.Contains(value))
                numOfAdditionalUV = value;
            else
                throw new ArgumentOutOfRangeException(nameof(NumOfAdditionalUV));
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
            if (IndexSize.Contains(value))
                sizeOfVertexIndex = value;
            else
                throw new ArgumentOutOfRangeException(nameof(SizeOfVertexIndex));
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
            if (IndexSize.Contains(value))
                sizeOfTextureIndex = value;
            else
                throw new ArgumentOutOfRangeException(nameof(SizeOfTextureIndex));
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
            if (IndexSize.Contains(value))
                sizeOfMaterialIndex = value;
            else
                throw new ArgumentOutOfRangeException(nameof(SizeOfMaterialIndex));
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
            if (IndexSize.Contains(value))
                sizeOfBoneIndex = value;
            else
                throw new ArgumentOutOfRangeException(nameof(SizeOfBoneIndex));
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
            if (IndexSize.Contains(value))
                sizeOfMorphIndex = value;
            else
                throw new ArgumentOutOfRangeException(nameof(SizeOfMorphIndex));
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
            if (IndexSize.Contains(value))
                sizeOfBodyIndex = value;
            else
                throw new ArgumentOutOfRangeException();
        }
    }


    /// <summary>
    /// モデルの使用エンコード方式
    /// </summary>
    public System.Text.Encoding Encoding => EncodingFormat switch
    {
        0 => System.Text.Encoding.Unicode,
        1 => System.Text.Encoding.UTF8,
        _ => throw new FormatException("エンコード情報が不正です。")
    };

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="version">バージョン</param>
    public PmxHeader(float version = 2.1f)
    {
        Version = version;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{FormatName}{Version:0.0}";
}
