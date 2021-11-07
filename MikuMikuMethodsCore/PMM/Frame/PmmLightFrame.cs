﻿using MikuMikuMethods.PMM.ElementState;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmLightFrame : PmmLightState, IPmmFrame
    {
        public int Frame { get; set; }
        public bool IsSelected { get; set; }

        public PmmLightFrame DeepCopy() => new()
        {
            Frame = Frame,
            IsSelected = IsSelected,
            Color = Color,
            Position = Position,
        };

        IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();
    }
}