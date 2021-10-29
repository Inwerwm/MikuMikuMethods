using MikuMikuMethods.PMM.ElementState;
using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.PMM
{
    public class PmmLight
    {
        public List<PmmLightFrame> Frames { get; } = new();
        public PmmLightState UncomittedState { get; } = new();
    }
}