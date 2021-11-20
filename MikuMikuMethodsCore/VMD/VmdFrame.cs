namespace MikuMikuMethods.Vmd;

/// <summary>
/// フレームの抽象クラス
/// </summary>
public abstract class VmdFrame : IVmdFrame
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public abstract VmdFrameType FrameType { get; }
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

    public abstract object Clone();

    protected VmdFrame(string name)
    {
        Name = name;
    }

    /// <summary>
    /// 時間の前後を比較する
    /// </summary>
    /// <param name="other">比較対象フレーム</param>
    /// <returns>
    /// <para>0未満 - このフレームはother以前</para>
    /// <para>0 - このフレームはotherと同時</para>
    /// <para>0超 - このフレームはother以降</para>
    /// </returns>
    public int CompareTo(IVmdFrame? other) => Frame.CompareTo(other?.Frame);
}
