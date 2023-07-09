using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmx;
using System.Collections.Immutable;

namespace MikuMikuMethods.Converter;

/// <summary>
/// PMX モデルを PMM モデルに変換する拡張メソッド
/// </summary>
public static class ModelConverter
{
    /// <summary>
    /// PMX から PMM へモデルを変換する
    /// </summary>
    /// <param name="pmxModel">変換元モデル</param>
    /// <param name="pmxPath">変換元モデルのファイルパス</param>
    /// <returns>PMM のモデルオブジェクト</returns>
    /// <exception cref="FileNotFoundException">指定されたパスにファイルが存在しなかった場合</exception>
    /// <exception cref="ArgumentException">モデルの表示枠に不正な要素が存在した場合</exception>
    public static PmmModel ToPmmModel(this PmxModel pmxModel, string pmxPath)
    {
        string pmxFullPath = Path.GetFullPath(pmxPath);
        if (!File.Exists(pmxFullPath))
        {
            throw new FileNotFoundException($"指定されたモデルが見つかりませんでした: {pmxFullPath}");
        }

        var pmmModel = new PmmModel()
        {
            Name = pmxModel.ModelInfo.Name,
            NameEn = pmxModel.ModelInfo.NameEn,
            Path = pmxFullPath,
            EdgeWidth = 1
        };

        var isPhysics = pmxModel.Bodies.Select(body => (body.RelationBone, isPhysicsBone: body.PhysicsMode != PmxBody.PhysicsModeType.Static))
                                       .Where(p => p.RelationBone is not null).Cast<(PmxBone RelationBone, bool isPhysicsBone)>()
                                       .GroupBy(p => p.RelationBone).ToDictionary(g => g.Key, g => g.Any(p => p.isPhysicsBone));

        pmmModel.Bones.AddRange(pmxModel.Bones.Select(ToPmmBone));
        pmmModel.Morphs.AddRange(pmxModel.Morphs.Select(ToPmmMorph));
        pmmModel.Nodes.AddRange(pmxModel.Nodes.Select(ToPmmNode));

        // IK と外部親はコンフィグ情報を作っておかないと PMM の書き込みに失敗する
        Pmm.Frame.PmmModelConfigFrame configFrame = new();
        pmmModel.ConfigFrames.Add(configFrame);

        foreach (var bone in pmmModel.Bones)
        {
            if (bone.IsIK)
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

        pmmModel.SelectedBone = pmmModel.Bones.FirstOrDefault();

        return pmmModel;

        PmmNode ToPmmNode(PmxNode pmxNode) => new()
        {
            Name = pmxNode.Name,
            Elements = pmxNode.Elements.Select(ToPmmModelElement).ToImmutableArray(),
            DoesOpen = false
        };

        IPmmModelElement ToPmmModelElement(IPmxNodeElement pmxNodeElement) => pmxNodeElement.Entity switch
        {
            PmxBone bone => ToPmmBone(bone),
            PmxMorph morph => ToPmmMorph(morph),
            _ => throw new ArgumentException("The Entity of IPmxNodeElement has invalid type instance.")
        };

        static PmmMorph ToPmmMorph(PmxMorph pmxMorph) => new(pmxMorph.Name);

        PmmBone ToPmmBone(PmxBone pmxBone)
        {
            PmmBone pmmBone = new(pmxBone.Name)
            {
                CanSetOutsideParent = pmxBone.Movable,
                IsIK = pmxBone.IsIK,
            };

            var gotValue = isPhysics.TryGetValue(pmxBone, out var physics);
            pmmBone.Current.EnablePhysic = gotValue ? physics : false;

            return pmmBone;
        }
    }
}
