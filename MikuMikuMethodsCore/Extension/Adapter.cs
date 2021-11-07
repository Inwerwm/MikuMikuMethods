using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Extension
{
    static class Adapter
    {
        public static PMM.Binary.PmmModel CreateFrom(PMX.PmxModel pmx, string modelPath)
        {
            throw new NotImplementedException();
            return new()
            {
                Name = pmx.ModelInfo.Name,
                NameEn = pmx.ModelInfo.NameEn,
                Path = modelPath,
                BoneNames = pmx.Bones.Select(b => b.Name).ToList(),

            };
        }
    }
}
