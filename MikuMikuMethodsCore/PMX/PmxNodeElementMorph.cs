namespace MikuMikuMethods.PMX
{
    public class PmxNodeElementMorph : IPmxNodeElement
    {
        public byte TypeNumber => 1;

        public PmxMorph Entity { get; set; }
        IPmxData IPmxNodeElement.Entity { get => Entity; set => Entity = (PmxMorph)value; }

        public int FindIndex(PmxModel model) => model.Morphs.IndexOf(Entity);

        public override string ToString() => $"Morph : {Entity.Name}";
    }
}
