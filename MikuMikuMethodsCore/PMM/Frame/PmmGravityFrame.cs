using MikuMikuMethods.PMM.ElementState;
using System.Numerics;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmGravityFrame : PmmGravityState, IPmmFrame
    {
        public int Frame { get; set; } = 0;
        public bool IsSelected { get; set; } = false;
    }
}
