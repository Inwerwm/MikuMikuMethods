namespace MikuMikuMethods.Pmm.ElementState;

/// <summary>
/// モーフの状態
/// </summary>
public class PmmMorphState : ICloneable
{
    /// <summary>
    /// 係数
    /// </summary>
    public float Weight { get; set; }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmMorphState DeepCopy() => new() { Weight = Weight };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
