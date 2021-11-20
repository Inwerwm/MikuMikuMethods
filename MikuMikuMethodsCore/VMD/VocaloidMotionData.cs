using MikuMikuMethods.Extension;
using MikuMikuMethods.Vmd.IO;
using System.Collections;

namespace MikuMikuMethods.Vmd;

/// <summary>
/// VMDファイルの内部表現
/// </summary>
public class VocaloidMotionData : IEnumerable<IVmdFrame>
{
    /// <summary>
    /// ヘッダー
    /// </summary>
    public string Header { get; set; }

    /// <summary>
    /// モデル名
    /// </summary>
    public string ModelName { get; set; } = "";

    /// <summary>
    /// VMDの種類
    /// </summary>
    public VmdKind Kind => ModelName == VmdConstants.CameraTypeVMDName ? VmdKind.Camera : VmdKind.Model;

    /// <summary>
    /// <para>全てのフレーム</para>
    /// <para>各種フレームの結合体なのでAddしても無駄</para>
    /// </summary>
    public IEnumerable<IVmdFrame> Frames =>
        CameraFrames.Cast<IVmdFrame>()
        .Concat(LightFrames)
        .Concat(ShadowFrames)
        .Concat(PropertyFrames)
        .Concat(MorphFrames)
        .Concat(MotionFrames);

    /// <summary>
    /// カメラフレーム
    /// </summary>
    public List<VmdCameraFrame> CameraFrames { get; init; }
    /// <summary>
    /// 照明フレーム
    /// </summary>
    public List<VmdLightFrame> LightFrames { get; init; }
    /// <summary>
    /// セルフ影フレーム
    /// </summary>
    public List<VmdShadowFrame> ShadowFrames { get; init; }

    /// <summary>
    /// プロパティフレーム
    /// </summary>
    public List<VmdPropertyFrame> PropertyFrames { get; init; }
    /// <summary>
    /// モーフフレーム
    /// </summary>
    public List<VmdMorphFrame> MorphFrames { get; init; }
    /// <summary>
    /// モーションフレーム
    /// </summary>
    public List<VmdMotionFrame> MotionFrames { get; init; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public VocaloidMotionData()
    {
        Header = VmdConstants.HeaderString;

        CameraFrames = new();
        LightFrames = new();
        ShadowFrames = new();

        PropertyFrames = new();
        MorphFrames = new();
        MotionFrames = new();
    }

    /// <summary>
    /// ファイル読み込みコンストラクタ
    /// </summary>
    /// <param name="filePath">読み込むファイルのパス</param>
    public VocaloidMotionData(string filePath) : this()
    {
        Read(filePath);
    }

    /// <summary>
    /// 内容の初期化
    /// </summary>
    public void Clear()
    {
        ModelName = "";

        CameraFrames.Clear();
        LightFrames.Clear();
        ShadowFrames.Clear();

        PropertyFrames.Clear();
        MorphFrames.Clear();
        MotionFrames.Clear();
    }

    /// <summary>
    /// フレームを追加する
    /// </summary>
    /// <param name="frame">追加するフレーム</param>
    public void AddFrame(IVmdFrame frame)
    {
        switch (frame.FrameKind)
        {
            case VmdFrameKind.Camera:
                CameraFrames.Add((VmdCameraFrame)frame);
                break;
            case VmdFrameKind.Light:
                LightFrames.Add((VmdLightFrame)frame);
                break;
            case VmdFrameKind.Shadow:
                ShadowFrames.Add((VmdShadowFrame)frame);
                break;
            case VmdFrameKind.Property:
                PropertyFrames.Add((VmdPropertyFrame)frame);
                break;
            case VmdFrameKind.Morph:
                MorphFrames.Add((VmdMorphFrame)frame);
                break;
            case VmdFrameKind.Motion:
                MotionFrames.Add((VmdMotionFrame)frame);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ファイルから読込
    /// </summary>
    /// <param name="filePath">読み込むファイルのパス</param>
    public void Read(string filePath)
    {
        VmdFileReader.Read(filePath, this);
    }

    /// <summary>
    /// ファイルに書き出し
    /// </summary>
    /// <param name="filePath">書き出すファイルのパス</param>
    public void Write(string filePath)
    {
        VmdFileWriter.Write(filePath, this);
    }

    public IEnumerator<IVmdFrame> GetEnumerator() => Frames.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Frames).GetEnumerator();
}
