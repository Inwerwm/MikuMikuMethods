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
    /// <param name="frames">追加するフレームのコレクション</param>
    public void AddFrames(IEnumerable<IVmdFrame> frames)
    {
        foreach (var frame in frames)
        {
            AddFrame(frame);
        }
    }

    /// <summary>
    /// フレームを追加する
    /// </summary>
    /// <param name="frame">追加するフレーム</param>
    public void AddFrame(IVmdFrame frame)
    {
        switch (frame)
        {
            case VmdCameraFrame f:
                CameraFrames.Add(f);
                break;
            case VmdLightFrame f:
                LightFrames.Add(f);
                break;
            case VmdShadowFrame f:
                ShadowFrames.Add(f);
                break;
            case VmdPropertyFrame f:
                PropertyFrames.Add(f);
                break;
            case VmdMorphFrame f:
                MorphFrames.Add(f);
                break;
            case VmdMotionFrame f:
                MotionFrames.Add(f);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// <para>全てのフレームコレクションを結合したものを返す</para>
    /// </summary>
    public IEnumerable<IVmdFrame> GetAllFrames() =>
        CameraFrames.Cast<IVmdFrame>()
                    .Concat(LightFrames)
                    .Concat(ShadowFrames)
                    .Concat(PropertyFrames)
                    .Concat(MorphFrames)
                    .Concat(MotionFrames);

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

    public IEnumerator<IVmdFrame> GetEnumerator() => GetAllFrames().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetAllFrames()).GetEnumerator();
}
