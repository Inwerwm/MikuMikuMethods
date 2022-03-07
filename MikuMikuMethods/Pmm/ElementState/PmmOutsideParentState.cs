namespace MikuMikuMethods.Pmm.ElementState;

public record PmmOutsideParentState
{
    /// <summary>
    /// 外部親が設定されたフレーム
    /// </summary>
    public int? StartFrame { get; set; }
    /// <summary>
    /// 外部親が変化する直前のフレーム
    /// </summary>
    public int? EndFrame { get; set; }

    public PmmModel? ParentModel { get; set; }
    public PmmBone? ParentBone { get; set; }

    public PmmOutsideParentState DeepCopy() => new()
    {
        StartFrame = StartFrame,
        EndFrame = EndFrame,
        ParentModel = ParentModel,
        ParentBone = ParentBone
    };
}
