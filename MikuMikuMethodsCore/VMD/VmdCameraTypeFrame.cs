namespace MikuMikuMethods.Vmd;

/// <summary>
/// カメラ系フレームの抽象クラス
/// </summary>
public abstract class VmdCameraTypeFrame : VmdFrame
{
    /// <summary>
    /// カメラ系フレームか？
    /// </summary>
    public override bool IsCameraType => true;

    /// <summary>
    /// モデル系フレームか？
    /// </summary>
    public override bool IsModelType => false;
}
