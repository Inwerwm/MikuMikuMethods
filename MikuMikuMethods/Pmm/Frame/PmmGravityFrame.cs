using MikuMikuMethods.Pmm.ElementState;

namespace MikuMikuMethods.Pmm.Frame;

/// <summary>
/// 重力フレーム
/// </summary>
public class PmmGravityFrame : PmmGravityState, IPmmFrame
{
    /// <inheritdoc/>
    public int Frame { get; set; } = 0;
    /// <inheritdoc/>
    public bool IsSelected { get; set; } = false;

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public new PmmGravityFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        Acceleration = Acceleration,
        Direction = Direction,
        Noize = Noize
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    /// <inheritdoc/>
    public override string ToString() => Frame.ToString();
}
