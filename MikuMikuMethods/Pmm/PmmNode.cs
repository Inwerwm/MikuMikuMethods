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

    public PmmNode DeepCopy(Dictionary<IPmmModelElement, IPmmModelElement> elementMap) => new()
    {
        Name = Name,
        DoesOpen = DoesOpen,
        Elements = ImmutableArray.Create(Elements.Select(e => elementMap.GetOrDefault(e)).Where(e => e is not null).Cast<IPmmModelElement>().ToArray())
    };
}
