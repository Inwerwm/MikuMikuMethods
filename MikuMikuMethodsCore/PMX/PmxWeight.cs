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
    }

    /// <summary>
    /// ウェイト値
    /// </summary>
    public record PmxWeight
    {
        /// <summary>
        /// 関連ボーン
        /// </summary>
        public PmxBone Bone { get; set; }

        /// <summary>
        /// ウェイト値
        /// </summary>
        public float Value { get; set; }
    }
}