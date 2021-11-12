namespace MikuMikuMethods.Pmm.ElementState
{
    public record PmmRangeSelector(int Index)
    {
        public static readonly PmmRangeSelector AllFrames = new PmmRangeSelector(0);
        public static readonly PmmRangeSelector RootBone = new PmmRangeSelector(1);
        public static readonly PmmRangeSelector ConfigFrame = new PmmRangeSelector(2);
        public static readonly PmmRangeSelector SelectedBones = new PmmRangeSelector(3);
        public static readonly PmmRangeSelector SelectedMorphs = new PmmRangeSelector(4);
        public static readonly PmmRangeSelector AllMorphFrames = new PmmRangeSelector(5);

        public PmmRangeSelector DeepCopy() => new(Index);
    }
}