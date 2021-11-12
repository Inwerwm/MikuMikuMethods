namespace MikuMikuMethods.Pmx;

/// <summary>
/// ウェイト変形方式
/// </summary>
public enum PmxWeightType : byte
{
    BDEF1,
    BDEF2,
    BDEF4,
    SDEF,
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
    public PmxBone Bone { get; set; }
    /// <summary>
    /// ウェイト値
    /// </summary>
    public float Value { get; set; }

    public PmxWeight(PmxBone bone, float value)
    {
        Bone = bone;
        Value = value;
    }

    public override string ToString() => $"{Bone.Name} : {Value}";
}
