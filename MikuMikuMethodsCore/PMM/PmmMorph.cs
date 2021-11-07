﻿using MikuMikuMethods.PMM.ElementState;
using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.PMM
{
    public class PmmMorph : IPmmModelElement
    {
        public string Name { get; set; }
        public List<PmmMorphFrame> Frames { get; } = new();

        public PmmMorphState Current { get; } = new();

        public override string ToString() => Name;
    }
}