using System.Numerics;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// ジョイント
/// </summary>
public class PmxJoint : IPmxData
{
    /// <summary>
    /// 名前
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 英名
    /// </summary>
    public string NameEn { get; set; }

    /// <summary>
    /// ジョイント種別
    /// </summary>
    public JointType Type { get; set; }

    /// <summary>
    /// 関連剛体A
    /// </summary>
    public PmxBody RelationBodyA { get; set; }
    /// <summary>
    /// 関連剛体B
    /// </summary>
    public PmxBody RelationBodyB { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    public Vector3 Position { get; set; }
    /// <summary>
    /// 回転
    /// </summary>
    public Vector3 Rotation { get; set; }

    /// <summary>
    /// 移動制限-下限
    /// </summary>
    public Vector3 MovingLowerLimit { get; set; }
    /// <summary>
    /// 移動制限-上限
    /// </summary>
    public Vector3 MovingUpperLimit { get; set; }
    /// <summary>
    /// 回転制限-下限
    /// </summary>
    public Vector3 RotationLowerLimit { get; set; }
    /// <summary>
    /// 回転制限-上限
    /// </summary>
    public Vector3 RotationUpperLimit { get; set; }

    /// <summary>
    /// バネ定数-移動
    /// </summary>
    public Vector3 MovingSpringConstants { get; set; }
    /// <summary>
    /// バネ定数-回転
    /// </summary>
    public Vector3 RotationSpringConstants { get; set; }

    /// <summary>
    /// ジョイント種別
    /// </summary>
    public enum JointType : byte
    {
        /// <summary>
        /// バネ付6DOF
        /// </summary>
        SixDofWithSpring,
        /// <summary>
        /// 6DOF
        /// </summary>
        SixDof,
        /// <summary>
        /// P2P
        /// </summary>
        P2P,
        /// <summary>
        /// ConeTwist
        /// </summary>
        ConeTwist,
        /// <summary>
        /// Slider
        /// </summary>
        Slider,
        /// <summary>
        /// Hinge
        /// </summary>
        Hinge
    }

    public override string ToString() => $"{Name} - {Position}";
}
