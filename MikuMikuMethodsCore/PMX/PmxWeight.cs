namespace MikuMikuMethods.PMX
{
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
    public record PmxWeight(PmxBone Bone, float Value);
}