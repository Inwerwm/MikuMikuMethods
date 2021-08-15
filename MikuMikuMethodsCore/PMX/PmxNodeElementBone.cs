namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// ボーンの表情枠要素
    /// </summary>
    public class PmxNodeElementBone : IPmxNodeElement
    {
        public byte TypeNumber => 0;

        public PmxBone Entity { get; set; }
        IPmxData IPmxNodeElement.Entity { get => Entity; set => Entity = (PmxBone)value; }

        public int FindIndex(PmxModel model) => model.Bones.IndexOf(Entity);

        public override string ToString() => $"Bone : {Entity.Name}";
    }
}
