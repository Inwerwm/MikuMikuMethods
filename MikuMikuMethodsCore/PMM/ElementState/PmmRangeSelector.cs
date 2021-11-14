namespace MikuMikuMethods.Pmm.ElementState;

public record PmmRangeSelector(int Index)
{
    public static readonly PmmRangeSelector AllFrames = new(0);
    public static readonly PmmRangeSelector RootBone = new(1);
    public static readonly PmmRangeSelector ConfigFrame = new(2);
    public static readonly PmmRangeSelector SelectedBones = new(3);
    public static readonly PmmRangeSelector SelectedMorphs = new(4);
    public static readonly PmmRangeSelector AllMorphFrames = new(5);

    public PmmRangeSelector DeepCopy() => new(Index);
}
