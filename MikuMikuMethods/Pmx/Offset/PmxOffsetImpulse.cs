using System.Numerics;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// インパルスモーフのオフセット
/// </summary>
public class PmxOffsetImpulse : IPmxOffset
{
    /// <summary>
    /// 対象剛体
    /// </summary>
    public PmxBody Target { get; set; }
    /// <summary>
    /// ローカルフラグ
    /// </summary>
    public bool IsLocal { get; set; }
    /// <summary>
    /// 移動速度
    /// </summary>
    public Vector3 MovingSpeed { get; set; }
    /// <summary>
    /// 回転トルク
    /// </summary>
    public Vector3 RotationTorque { get; set; }

    /// <inheritdoc/>
    public override string ToString() => $"{Target.Name} : {{{MovingSpeed} - {RotationTorque}}}";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="target">対象剛体</param>
    /// <param name="isLocal">ローカルフラグ</param>
    /// <param name="movingSpead">移動速度</param>
    /// <param name="rotationTorque">回転トルク</param>
    public PmxOffsetImpulse(PmxBody target, bool isLocal = default, Vector3 movingSpead = default, Vector3 rotationTorque = default)
    {
        Target = target;
        IsLocal = isLocal;
        MovingSpeed = movingSpead;
        RotationTorque = rotationTorque;
    }

    internal PmxOffsetImpulse()
    {
        Target = null!;
    }
}
