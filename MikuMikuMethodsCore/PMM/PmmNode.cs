using System.Collections.Immutable;

namespace MikuMikuMethods.PMM
{
    public record PmmNode
    {
        public string Name { get; init; }
        public bool doesOpen { get; set; } = false;
        public ImmutableArray<IPmmModelElement> Elements { get; init; }

        public PmmNode(string name, bool doesOpen, ImmutableArray<IPmmModelElement> elements)
        {
            Name = name;
            this.doesOpen = doesOpen;
            Elements = elements;
        }
    }
}