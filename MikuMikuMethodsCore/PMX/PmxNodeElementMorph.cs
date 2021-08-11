﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    public class PmxNodeElementMorph : IPmxNodeElement
    {
        public byte TypeNumber => 1;

        public PmxMorph Entity { get; set; }
        IPmxData IPmxNodeElement.Entity { get => Entity; set => Entity = (PmxMorph)value; }

        public int FindIndex(PmxModel model) => model.Morphs.IndexOf(Entity);
    }
}
