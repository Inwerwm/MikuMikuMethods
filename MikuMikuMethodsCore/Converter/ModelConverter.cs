using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmx;
using System.Collections.Immutable;

namespace MikuMikuMethods.Converter;
public static class ModelConverter
{
    public static PmmModel ToPmmModel(this PmxModel pmxModel, string pmxPath)
    {
        var pmmModel = new PmmModel()
        {
            Name = pmxModel.ModelInfo.Name,
            NameEn = pmxModel.ModelInfo.NameEn,
            Path = pmxPath
        };

        pmmModel.Bones.AddRange(pmxModel.Bones.Select(ToPmmBone));
        pmmModel.Morphs.AddRange(pmxModel.Morphs.Select(ToPmmMorph));
        pmmModel.Nodes.AddRange(pmxModel.Nodes.Select(ToPmmNode));

        return pmmModel;
    }

    private static PmmNode ToPmmNode(PmxNode pmxNode) => new()
    {
        Name = pmxNode.Name,
        Elements = pmxNode.Elements.Select(ToPmmModelElement).ToImmutableArray(),
        DoesOpen = false
    };

    private static IPmmModelElement ToPmmModelElement(IPmxNodeElement pmxNodeElement) => pmxNodeElement.Entity switch
    {
        PmxBone bone => ToPmmBone(bone),
        PmxMorph morph => ToPmmMorph(morph),
        _ => throw new ArgumentException("The Entity of IPmxNodeElement has invalid type instance.")
    };

    private static PmmMorph ToPmmMorph(PmxMorph pmxMorph) => new(pmxMorph.Name);

    private static PmmBone ToPmmBone(PmxBone pmxBone) => new(pmxBone.Name)
    {
        CanSetOutsideParent = pmxBone.Movable,
        IsIK = pmxBone.IsIK,
    };
}
