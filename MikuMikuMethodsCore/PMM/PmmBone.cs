using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.PMM
{
    public class PmmBone : IPmmModelElement
    {
        public string Name { get; set; }
        public List<PmmBoneFrame> Frames { get; } = new();
    }
}