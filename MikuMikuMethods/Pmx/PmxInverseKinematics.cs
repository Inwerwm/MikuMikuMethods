using System.Numerics;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// IK情報
/// </summary>
public class PmxInverseKinematics
{
    /// <summary>
    /// IKのターゲットボーン
    /// </summary>
    public PmxBone? Target { get; set; }
    /// <summary>
    /// ループ回数
    /// </summary>
    public int LoopNum { get; set; }
    /// <summary>
    /// ループ計算時の制限角度 -> ラジアン
    /// </summary>
    public float LimitAngle { get; set; }

    /// <summary>
    /// リンク
    /// </summary>
    public List<PmxIKLink> Links { get; } = new();

    /// <inheritdoc/>
    public override string ToString() => $"{Target?.Name} - {LoopNum} : {LimitAngle:###.00}";
}

/// <summary>
/// IKのリンク情報
/// </summary>
public class PmxIKLink
{
    /// <summary>
    /// 対象ボーン
    /// </summary>
    public PmxBone? Bone { get; set; }

    /// <summary>
    /// 角度制限の有無
    /// </summary>
    public bool EnableAngleLimit { get; set; }
    /// <summary>
    /// 角度制限の上限
    /// </summary>
    public Vector3 UpperLimit { get; set; }
    /// <summary>
    /// 角度制限の下限
    /// </summary>
    public Vector3 LowerLimit { get; set; }

    /// <inheritdoc/>
    public override string ToString() => $"Link: {Bone?.Name}{(EnableAngleLimit ? "(Limited)" : "")}";
}
