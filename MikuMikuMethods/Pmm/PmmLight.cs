using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

public class PmmLight : ICloneable
{
    public List<PmmLightFrame> Frames { get; private init; } = new();
    public PmmLightState Current { get; private init; } = new();

    public object Clone() => DeepCopy();

    public PmmLight DeepCopy() => new()
    {
        Frames = this.Frames.Select(f => f.DeepCopy()).ToList(),
        Current = this.Current.DeepCopy()
    };
}
