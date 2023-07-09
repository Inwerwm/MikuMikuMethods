using System.Collections.ObjectModel;

namespace MikuMikuMethods.Vmd;

/// <summary>
/// 補間曲線を持つデータであることを示すインターフェイス
/// </summary>
public interface IVmdInterpolatable
{
    /// <summary>
    /// 補間曲線
    /// </summary>
    ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; }
}
