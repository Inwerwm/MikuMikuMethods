namespace MikuMikuMethods.Mme.Element;
internal class EmmObjectReference : EmmObject
{
    public EmmObject Reference { get; }
    public string ReferenceName { get; }

    public override string Name => $"{Reference.Name}@{ReferenceName}";

    /// <summary>
    /// 他の <see cref="EmmObject"/> への参照を持つオブジェクト
    /// </summary>
    /// <param name="reference">参照対象オブジェクト</param>
    /// <param name="referenceName">参照名</param>
    public EmmObjectReference(EmmObject reference, string referenceName) : base(reference.Index, reference.Path)
    {
        Reference = reference;
        ReferenceName = referenceName;
    }
}
