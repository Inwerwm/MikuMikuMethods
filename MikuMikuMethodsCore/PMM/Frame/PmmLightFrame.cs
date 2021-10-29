using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.ElementState;
using System.Numerics;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmLightFrame : PmmLightState, IPmmFrame
    {
        public int Frame { get; set; }
        public bool IsSelected { get; set; }
    }
}