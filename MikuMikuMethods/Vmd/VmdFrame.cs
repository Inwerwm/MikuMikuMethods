namespace MikuMikuMethods.Vmd;

/// <summary>
/// フレームの抽象クラス
/// </summary>
public abstract class VmdFrame : IVmdFrame
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public abstract VmdFrameKind FrameKind { get; }
    /// <summary>
    /// フレームの名前
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// フレームの時間
    /// </summary>
    public uint Frame { get; set; }
    /// <summary>
    /// カメラ系フレームか？
    /// </summary>
    public abstract bool IsCameraType { get; }
    /// <summary>
    /// モデル系フレームか？
    /// </summary>
    public abstract bool IsModelType { get; }

    /// <inheritdoc/>
    public abstract object Clone();

    /// <summary>
    /// 名前を指定して <see cref="VmdFrame"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="name">フレームの名前</param>
    protected VmdFrame(string name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public int CompareTo(IVmdFrame? other) => Frame.CompareTo(other?.Frame);
}
