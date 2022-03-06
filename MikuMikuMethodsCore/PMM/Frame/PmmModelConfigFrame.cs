﻿using MikuMikuMethods.Pmm.ElementState;

namespace MikuMikuMethods.Pmm.Frame;

public class PmmModelConfigFrame : PmmModelConfigState, IPmmFrame
{
    public int Frame { get; set; }
    public bool IsSelected { get; set; }

    public new PmmModelConfigFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        EnableIK = new(EnableIK),
        OutsideParent = OutsideParent.ToDictionary(p => p.Key, p => p.Value.DeepCopy()),
        Visible = Visible
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    public override string ToString() => Frame.ToString();
}
