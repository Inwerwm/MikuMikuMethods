using MikuMikuMethods.Pmm.ElementState;

namespace MikuMikuMethods.Pmm.Frame;

public class PmmAccessoryFrame : PmmAccessoryState, IPmmFrame
{
    public int Frame { get; set; }
    public bool IsSelected { get; set; }

    public new PmmAccessoryFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        EnableShadow = EnableShadow,
        ParentModel = ParentModel,
        ParentBone = ParentBone,
        Position = Position,
        Rotation = Rotation,
        Scale = Scale,
        TransAndVisible = TransAndVisible,
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    public override string ToString() => Frame.ToString();
}
