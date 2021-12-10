using System.Collections.ObjectModel;

namespace MikuMikuMethods.Vmd;

public interface IVmdInterpolatable
{
    /// <summary>
    /// 補間曲線
    /// </summary>
    ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; }
}
