using MikuMikuMethods.PMM.ElementState;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmAccessoryFrame : PmmAccessoryState, IPmmFrame
    {
        public int Frame { get; set; }
        public bool IsSelected { get; set; }
    }
}