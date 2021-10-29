using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.PMM
{
    public class PmmMorph : IPmmModelElement
    {
        public string Name { get; set; }
        public List<PmmMorphFrame> Frames { get; } = new();
    }
}