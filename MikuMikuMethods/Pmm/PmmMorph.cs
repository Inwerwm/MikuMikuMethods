using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

public class PmmMorph : IPmmModelElement
{
    public string Name { get; set; }

    public List<PmmMorphFrame> Frames { get; private init; } = new();

    public PmmMorphState Current { get; private init; } = new();

    public override string ToString() => Name;

    public PmmMorph(string name)
    {
        Name = name;
    }

    public PmmMorph DeepCopy() => new(Name)
    {
        Frames = this.Frames.Select(f => f.DeepCopy()).ToList(),
        Current = this.Current.DeepCopy()
    };

    public object Clone() => DeepCopy();
}
