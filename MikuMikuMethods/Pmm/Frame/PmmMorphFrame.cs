using MikuMikuMethods.Pmm.ElementState;

namespace MikuMikuMethods.Pmm.Frame;

/// <summary>
/// モーフフレーム
/// </summary>
public class PmmMorphFrame : PmmMorphState, IPmmFrame
{
    /// <inheritdoc/>
    public int Frame { get; set; } = 0;
    /// <inheritdoc/>
    public bool IsSelected { get; set; } = false;

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public new PmmMorphFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        Weight = Weight
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    /// <inheritdoc/>
    public override string ToString() => Frame.ToString();
}
