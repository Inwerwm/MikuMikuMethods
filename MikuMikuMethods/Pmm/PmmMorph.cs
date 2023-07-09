using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// モーフ
/// </summary>
public class PmmMorph : IPmmModelElement
{
    /// <inheritdoc/>
    public string Name { get; set; }

    /// <summary>
    /// フレーム
    /// </summary>
    public List<PmmMorphFrame> Frames { get; private init; } = new();

    /// <summary>
    /// 現在の状態
    /// </summary>
    public PmmMorphState Current { get; private init; } = new();

    /// <inheritdoc/>
    public override string ToString() => Name;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">名前</param>
    public PmmMorph(string name)
    {
        Name = name;
    }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmMorph DeepCopy() => new(Name)
    {
        Frames = this.Frames.Select(f => f.DeepCopy()).ToList(),
        Current = this.Current.DeepCopy()
    };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
