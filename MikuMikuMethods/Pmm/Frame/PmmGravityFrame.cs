using MikuMikuMethods.Pmm.ElementState;

namespace MikuMikuMethods.Pmm.Frame;

public class PmmGravityFrame : PmmGravityState, IPmmFrame
{
    public int Frame { get; set; } = 0;
    public bool IsSelected { get; set; } = false;

    public new PmmGravityFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        Acceleration = Acceleration,
        Direction = Direction,
        Noize = Noize
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    public override string ToString() => Frame.ToString();
}
