﻿using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        return pmmModel;
    }

    private static PmmBone ToPmmBone(PmxBone pmxBone)
    {
        var pmmBone = new PmmBone(pmxBone.Name)
        {
            CanSetOuterParent = pmxBone.Movable
        };

        return pmmBone;
    }
}
