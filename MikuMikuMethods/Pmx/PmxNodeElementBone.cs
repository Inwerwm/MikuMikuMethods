namespace MikuMikuMethods.Pmx;

/// <summary>
/// ボーンの表情枠要素
/// </summary>
public class PmxNodeElementBone : IPmxNodeElement
{
    /// <inheritdoc/>
    public byte TypeNumber => 0;

    /// <summary>
    /// 参照ボーン
    /// </summary>
    public PmxBone Entity { get; set; }
    IPmxData IPmxNodeElement.Entity { get => Entity; }

    /// <inheritdoc/>
    public int FindIndex(PmxModel model) => model.Bones.IndexOf(Entity);

    /// <inheritdoc/>
    public override string ToString() => $"Bone : {Entity.Name}";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="entity">参照ボーン</param>
    public PmxNodeElementBone(PmxBone entity)
    {
        Entity = entity;
    }
}
