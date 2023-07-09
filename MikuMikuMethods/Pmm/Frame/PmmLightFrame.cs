using MikuMikuMethods.Pmm.ElementState;

namespace MikuMikuMethods.Pmm.Frame;

/// <summary>
/// ライトフレーム
/// </summary>
public class PmmLightFrame : PmmLightState, IPmmFrame
{
    /// <inheritdoc/>
    public int Frame { get; set; }
    /// <inheritdoc/>
    public bool IsSelected { get; set; }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public new PmmLightFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        Color = Color,
        Position = Position,
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    /// <inheritdoc/>
    public override string ToString() => Frame.ToString();
}
