using System.Numerics;

namespace MikuMikuMethods.Pmx
{
    /// <summary>
    /// ボーンモーフのオフセット
    /// </summary>
    public class PmxOffsetBone : IPmxOffset
    {
        /// <summary>
        /// 対象ボーン
        /// </summary>
        public PmxBone Target { get; set; }

        /// <summary>
        /// 移動量
        /// </summary>
        public Vector3 Offset { get; set; }
        /// <summary>
        /// 回転量
        /// </summary>
        public Quaternion Rotate { get; set; }

        public override string ToString() => $"{Target.Name} : {{{Offset} - {Rotate}}}";
    }
}
