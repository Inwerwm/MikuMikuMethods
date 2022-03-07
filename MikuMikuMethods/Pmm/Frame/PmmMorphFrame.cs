using MikuMikuMethods.Pmm.ElementState;

namespace MikuMikuMethods.Pmm.Frame;

public class PmmMorphFrame : PmmMorphState, IPmmFrame
{
    public int Frame { get; set; } = 0;
    public bool IsSelected { get; set; } = false;

    public new PmmMorphFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        Weight = Weight
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    public override string ToString() => Frame.ToString();
}
