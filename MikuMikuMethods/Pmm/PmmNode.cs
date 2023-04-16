using System.Collections.Immutable;

namespace MikuMikuMethods.Pmm;


public class PmmNode : ICloneable
{
    public string Name { get; internal set; }
    public bool DoesOpen { get; set; } = false;
    public ImmutableArray<IPmmModelElement> Elements { get; init; } = new();

    public override string ToString() => Name;

    internal PmmNode()
    {
        Name = "";
    }

    public PmmNode DeepCopy() => new()
    {
        Name = Name,
        DoesOpen = DoesOpen,
        Elements = ImmutableArray.Create(this.Elements.Select(e => (IPmmModelElement)e.Clone()).ToArray())
    };

    public object Clone() => DeepCopy();
}
