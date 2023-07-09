namespace MikuMikuMethods.Pmx;

/// <summary>
/// モーフ
/// </summary>
public class PmxMorph : IPmxData
{
    /// <summary>
    /// 名前
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// 名前(英語)
    /// </summary>
    public string NameEn { get; set; } = "";

    /// <summary>
    /// 表示パネル
    /// </summary>
    public MorphPanel Panel { get; set; }

    /// <summary>
    /// モーフの種類
    /// </summary>
    public MorphType Type { get; init; }

    /// <summary>
    /// モーフのオフセット
    /// </summary>
    public List<IPmxOffset> Offsets { get; } = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="type">モーフの種類</param>
    public PmxMorph(MorphType type)
    {
        Type = type;
    }

    /// <summary>
    /// モーフの表示パネル
    /// </summary>
    public enum MorphPanel : byte
    {
        /// <summary>
        /// システム予約
        /// </summary>
        System,
        /// <summary>
        /// 眉(左下)
        /// </summary>
        Brow,
        /// <summary>
        /// 目(左上)
        /// </summary>
        Eye,
        /// <summary>
        /// 口(右上)
        /// </summary>
        Lip,
        /// <summary>
        /// その他(右下)
        /// </summary>
        Other
    }

    /// <summary>
    /// モーフ種類
    /// </summary>
    public enum MorphType : byte
    {
        /// <summary>
        /// グループ
        /// </summary>
        Group,
        /// <summary>
        /// 頂点
        /// </summary>
        Vertex,
        /// <summary>
        /// ボーン
        /// </summary>
        Bone,
        /// <summary>
        /// UV
        /// </summary>
        UV,
        /// <summary>
        /// 追加UV1
        /// </summary>
        AdditionalUV1,
        /// <summary>
        /// 追加UV2
        /// </summary>
        AdditionalUV2,
        /// <summary>
        /// 追加UV3
        /// </summary>
        AdditionalUV3,
        /// <summary>
        /// 追加UV4
        /// </summary>
        AdditionalUV4,
        /// <summary>
        /// 材質
        /// </summary>
        Material,
        /// <summary>
        /// フリップ
        /// </summary>
        Flip,
        /// <summary>
        /// インパルス
        /// </summary>
        Impulse
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} - {Type} Morph";
}
