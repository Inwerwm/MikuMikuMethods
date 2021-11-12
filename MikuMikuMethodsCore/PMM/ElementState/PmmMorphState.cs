using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Pmm.ElementState;

public class PmmMorphState
{
    /// <summary>
    /// 係数
    /// </summary>
    public float Weight { get; set; }

    public PmmMorphState DeepCopy() => new() { Weight = Weight };
}
