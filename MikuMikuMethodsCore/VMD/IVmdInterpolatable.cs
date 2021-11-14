namespace MikuMikuMethods.Vmd;

public interface IVmdInterpolatable
{
    /// <summary>
    /// 補間曲線
    /// </summary>
    Dictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; }
}
