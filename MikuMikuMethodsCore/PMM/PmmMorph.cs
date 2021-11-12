using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.Pmm
{
    public class PmmMorph : IPmmModelElement
    {
        public string Name { get; set; }
        public List<PmmMorphFrame> Frames { get; } = new();

        public PmmMorphState Current { get; } = new();

        public override string ToString() => Name;
    }
}