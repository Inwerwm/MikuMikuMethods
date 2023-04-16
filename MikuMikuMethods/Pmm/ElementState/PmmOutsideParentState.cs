namespace MikuMikuMethods.Pmm.ElementState;

public record PmmOutsideParentState : ICloneable
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

    public PmmOutsideParentState DeepCopy() => this with { };
    public PmmOutsideParentState DeepCopy(PmmModel? parentModel, PmmBone? parentBone) => this with { ParentModel = parentModel, ParentBone = parentBone };

    object ICloneable.Clone() => DeepCopy();
}
