using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmx;
using System.Collections.Immutable;

namespace MikuMikuMethods.Converter;
public static class ModelConverter
{
    public static PmmModel ToPmmModel(this PmxModel pmxModel, string pmxPath)
    {
        string pmxFullPath = Path.GetFullPath(pmxPath);
        if(!File.Exists(pmxFullPath))
        {
            throw new FileNotFoundException($"指定されたモデルが見つかりませんでした: {pmxFullPath}");
        }

        var pmmModel = new PmmModel()
        {
            Name = pmxModel.ModelInfo.Name,
            NameEn = pmxModel.ModelInfo.NameEn,
            Path = pmxFullPath
        };

        pmmModel.Bones.AddRange(pmxModel.Bones.Select(ToPmmBone));
        pmmModel.Morphs.AddRange(pmxModel.Morphs.Select(ToPmmMorph));
        pmmModel.Nodes.AddRange(pmxModel.Nodes.Select(ToPmmNode));

        // IK と外部親はコンフィグ情報を作っておかないと PMM の書き込みに失敗する
        Pmm.Frame.PmmModelConfigFrame configFrame = new();
        pmmModel.ConfigFrames.Add(configFrame);

        foreach (var bone in pmmModel.Bones)
        {
            if(bone.IsIK)
            {
                pmmModel.CurrentConfig.EnableIK.Add(bone, true);
                configFrame.EnableIK.Add(bone, true);
            }

            if (bone.CanSetOutsideParent)
            {
                pmmModel.CurrentConfig.OutsideParent.Add(bone, new());
                configFrame.OutsideParent.Add(bone, new());
            }
        }

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
