using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

public class PmmLight
{
    public List<PmmLightFrame> Frames { get; } = new();
    public PmmLightState Current { get; } = new();
}
