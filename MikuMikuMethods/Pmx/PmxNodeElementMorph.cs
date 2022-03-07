namespace MikuMikuMethods.Pmx;

public class PmxNodeElementMorph : IPmxNodeElement
{
    public byte TypeNumber => 1;

    public PmxMorph Entity { get; set; }
    IPmxData IPmxNodeElement.Entity { get => Entity; }

    public int FindIndex(PmxModel model) => model.Morphs.IndexOf(Entity);

    public override string ToString() => $"Morph : {Entity.Name}";

    public PmxNodeElementMorph(PmxMorph entity)
    {
        Entity = entity;
    }
}
