namespace MikuMikuMethods.Pmx;

/// <summary>
/// モーフの表情枠要素
/// </summary>
public class PmxNodeElementMorph : IPmxNodeElement
{
    /// <inheritdoc/>
    public byte TypeNumber => 1;

    /// <summary>
    /// 参照モーフ
    /// </summary>
    public PmxMorph Entity { get; set; }
    IPmxData IPmxNodeElement.Entity { get => Entity; }

    /// <inheritdoc/>
    public int FindIndex(PmxModel model) => model.Morphs.IndexOf(Entity);

    /// <inheritdoc/>
    public override string ToString() => $"Morph : {Entity.Name}";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="entity">参照モーフ</param>
    public PmxNodeElementMorph(PmxMorph entity)
    {
        Entity = entity;
    }
}
