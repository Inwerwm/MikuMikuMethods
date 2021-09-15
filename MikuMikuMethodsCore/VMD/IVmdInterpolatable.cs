using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.VMD
{
    public interface IVmdInterpolatable
    {
        /// <summary>
        /// 補間曲線
        /// </summary>
        Dictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; }
    }
}
