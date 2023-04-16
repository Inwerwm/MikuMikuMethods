using MikuMikuMethods.Pmm.ElementState;

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

    public new PmmModelConfigFrame DeepCopy(Dictionary<PmmBone, PmmBone> boneMap) => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        EnableIK = EnableIK.Where(p => boneMap.ContainsKey(p.Key)).ToDictionary(p => boneMap[p.Key], p => p.Value),
        OutsideParent = OutsideParent.SelectKeyValue(boneMap, null).ToDictionary(p => p.Key, p => p.Value.DeepCopy()),
        Visible = Visible
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    public override string ToString() => Frame.ToString();
}
