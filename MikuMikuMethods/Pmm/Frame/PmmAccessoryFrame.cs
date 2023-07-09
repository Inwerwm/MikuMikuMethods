using MikuMikuMethods.Pmm.ElementState;

namespace MikuMikuMethods.Pmm.Frame;

/// <summary>
/// アクセサリフレーム
/// </summary>
public class PmmAccessoryFrame : PmmAccessoryState, IPmmFrame
{
    /// <inheritdoc/>
    public int Frame { get; set; }
    /// <inheritdoc/>
    public bool IsSelected { get; set; }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public new PmmAccessoryFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        EnableShadow = EnableShadow,
        ParentModel = ParentModel,
        ParentBone = ParentBone,
        Position = Position,
        Rotation = Rotation,
        Scale = Scale,
        TransAndVisible = TransAndVisible,
    };

    /// <summary>
    /// PMM移行用ディープコピー
    /// </summary>
    /// <param name="parentModel">移行対象側の対応する参照親モデル</param>
    /// <param name="parentBone">移行対象側の対応する参照親ボーン</param>
    /// <returns>複製</returns>
    public new PmmAccessoryFrame DeepCopy(PmmModel? parentModel, PmmBone? parentBone) => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        EnableShadow = EnableShadow,
        ParentModel = parentModel,
        ParentBone = parentBone,
        Position = Position,
        Rotation = Rotation,
        Scale = Scale,
        TransAndVisible = TransAndVisible,
    };

    /// <inheritdoc/>
    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    /// <inheritdoc/>
    public override string ToString() => Frame.ToString();
}
