namespace MikuMikuMethods.Pmx;

/// <summary>
/// ウェイト変形方式
/// </summary>
public enum PmxWeightType : byte
{
    /// <summary>
    /// BDEF1
    /// </summary>
    BDEF1,

    /// <summary>
    /// BDEF2
    /// </summary>
    BDEF2,

    /// <summary>
    /// BDEF4
    /// </summary>
    BDEF4,

    /// <summary>
    /// SDEF
    /// </summary>
    SDEF,

    /// <summary>
    /// QDEF
    /// </summary>
    QDEF
}

/// <summary>
/// ウェイト値
/// </summary>
public class PmxWeight : IPmxData
{
    /// <summary>
    /// ボーン
    /// </summary>
    public PmxBone? Bone { get; set; }
    /// <summary>
    /// ウェイト値
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="bone">ボーン</param>
    /// <param name="value">重み</param>
    public PmxWeight(PmxBone? bone, float value)
    {
        Bone = bone;
        Value = value;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Bone?.Name ?? ""} : {Value}";
}
