namespace MikuMikuMethods.Vmd;

/// <summary>
/// モデル系フレームの抽象クラス
/// </summary>
public abstract class VmdModelTypeFrame : VmdFrame
{
    /// <summary>
    /// 名前を指定して <see cref="VmdModelTypeFrame"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="name">モーフ名</param>
    protected VmdModelTypeFrame(string name) : base(name)
    {
    }

    /// <summary>
    /// カメラ系フレームか？
    /// </summary>
    public override bool IsCameraType => false;

    /// <summary>
    /// モデル系フレームか？
    /// </summary>
    public override bool IsModelType => true;
}
