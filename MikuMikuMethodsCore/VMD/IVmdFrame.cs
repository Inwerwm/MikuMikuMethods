namespace MikuMikuMethods.Vmd;

/// <summary>
/// フレームのインターフェイス
/// </summary>
public interface IVmdFrame : IComparable<IVmdFrame>, ICloneable
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    VmdFrameType FrameType { get; }
    /// <summary>
    /// フレームの名前
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// フレームの時間
    /// </summary>
    uint Frame { get; set; }

    /// <summary>
    /// カメラ系フレームか？
    /// </summary>
    bool IsCameraType { get; }
    /// <summary>
    /// モデル系フレームか？
    /// </summary>
    bool IsModelType { get; }
}
