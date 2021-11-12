using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.Pmm;

public class PmmLight
{
    public List<PmmLightFrame> Frames { get; } = new();
    public PmmLightState Current { get; } = new();
}
