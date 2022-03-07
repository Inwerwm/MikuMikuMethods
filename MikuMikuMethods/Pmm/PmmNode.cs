using System.Collections.Immutable;

namespace MikuMikuMethods.Pmm;


public class PmmNode
{
    public string Name { get; internal set; }
    public bool DoesOpen { get; set; } = false;
    public ImmutableArray<IPmmModelElement> Elements { get; init; } = new();

    public override string ToString() => Name;

    internal PmmNode()
    {
        Name = "";
    }
}
