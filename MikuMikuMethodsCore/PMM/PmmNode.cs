using System.Collections.Immutable;

namespace MikuMikuMethods.PMM
{
    public record PmmNode
    {
        public string Name { get; set; }
        public bool doesOpen { get; set; } = false;
        public ImmutableArray<IPmmModelElement> Elements { get; init; } = new();
    }
}