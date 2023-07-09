namespace MikuMikuMethods.Pmm.ElementState;

/// <summary>
/// 外部親の状態
/// </summary>
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

    /// <summary>
    /// 外部親のモデル
    /// </summary>
    public PmmModel? ParentModel { get; set; }
    /// <summary>
    /// 外部親のボーン
    /// </summary>
    public PmmBone? ParentBone { get; set; }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmOutsideParentState DeepCopy() => this with { };
    /// <summary>
    /// モデル移行用ディープコピー
    /// </summary>
    /// <param name="parentModel">移行先の対応する参照親モデル</param>
    /// <param name="parentBone">移行先の対応する参照親ボーン</param>
    /// <returns>複製</returns>
    public PmmOutsideParentState DeepCopy(PmmModel? parentModel, PmmBone? parentBone) => this with { ParentModel = parentModel, ParentBone = parentBone };

    object ICloneable.Clone() => DeepCopy();
}
