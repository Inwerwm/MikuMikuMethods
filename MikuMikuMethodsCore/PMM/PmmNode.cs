using System.Collections.Immutable;

namespace MikuMikuMethods.Pmm;

public record PmmNode
{
    public string Name { get; set; }
    public bool doesOpen { get; set; } = false;
    public ImmutableArray<IPmmModelElement> Elements { get; init; } = new();

    public override string ToString() => Name;
}
