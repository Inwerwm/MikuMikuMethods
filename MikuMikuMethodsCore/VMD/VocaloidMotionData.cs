using MikuMikuMethods.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    public string ModelName { get; set; }

    /// <summary>
    /// VMDの種類
    /// </summary>
    public VMDType Type => ModelName == VmdConstants.CameraTypeVMDName ? VMDType.Camera : VMDType.Model;

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
    /// バイナリ読み込みコンストラクタ
    /// </summary>
    /// <param name="reader"></param>
    public VocaloidMotionData(BinaryReader reader) : this()
    {
        Read(reader);
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
        switch (frame.FrameType)
        {
            case VmdFrameType.Camera:
                CameraFrames.Add((VmdCameraFrame)frame);
                break;
            case VmdFrameType.Light:
                LightFrames.Add((VmdLightFrame)frame);
                break;
            case VmdFrameType.Shadow:
                ShadowFrames.Add((VmdShadowFrame)frame);
                break;
            case VmdFrameType.Property:
                PropertyFrames.Add((VmdPropertyFrame)frame);
                break;
            case VmdFrameType.Morph:
                MorphFrames.Add((VmdMorphFrame)frame);
                break;
            case VmdFrameType.Motion:
                MotionFrames.Add((VmdMotionFrame)frame);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ファイルから読込
    /// </summary>
    /// <param name="path">読み込むファイルのパス</param>
    public void Read(string path)
    {
        using (FileStream stream = new(path, FileMode.Open))
        using (BinaryReader reader = new(stream, MikuMikuMethods.Encoding.ShiftJIS))
        {
            Read(reader);
        }
    }

    /// <summary>
    /// 読み込み
    /// </summary>
    /// <param name="reader">VMDファイルのリーダー</param>
    public void Read(BinaryReader reader)
    {
        Header = reader.ReadString(VmdConstants.HeaderLength, Encoding.ShiftJIS, '\0');
        ModelName = reader.ReadString(VmdConstants.ModelNameLength, Encoding.ShiftJIS, '\0');

        ReadFrames(reader, r => MotionFrames.Add(new(r)));
        ReadFrames(reader, r => MorphFrames.Add(new(r)));
        ReadFrames(reader, r => CameraFrames.Add(new(r)));
        ReadFrames(reader, r => LightFrames.Add(new(r)));
        ReadFrames(reader, r => ShadowFrames.Add(new(r)));
        ReadFrames(reader, r => PropertyFrames.Add(new(r)));
    }

    private void ReadFrames(BinaryReader reader, Action<BinaryReader> addToList)
    {
        var elementNum = reader.ReadUInt32();
        for (int i = 0; i < elementNum; i++)
            addToList(reader);
    }

    /// <summary>
    /// ファイルに書き出し
    /// </summary>
    /// <param name="filePath">書き出すファイルのパス</param>
    public void Write(string filePath)
    {
        using (FileStream file = new(filePath, FileMode.Create))
        using (BinaryWriter writer = new(file, MikuMikuMethods.Encoding.ShiftJIS))
        {
            Write(writer);
        }
    }

    /// <summary>
    /// VMD形式で書き出し
    /// </summary>
    public void Write(BinaryWriter writer)
    {
        writer.Write(Header, VmdConstants.HeaderLength, Encoding.ShiftJIS);
        writer.Write(ModelName, VmdConstants.ModelNameLength, Encoding.ShiftJIS);

        WriteFrames(writer, MotionFrames);
        WriteFrames(writer, MorphFrames);
        WriteFrames(writer, CameraFrames);
        WriteFrames(writer, LightFrames);
        WriteFrames(writer, ShadowFrames);
        WriteFrames(writer, PropertyFrames);
    }

    private void WriteFrames<T>(BinaryWriter writer, List<T> frames) where T : IVmdFrame
    {
        writer.Write((uint)frames.Count);
        // 時間で降順に書き込むと読み込みが早くなる(らしい)
        foreach (var f in frames.OrderByDescending(f => f.Frame))
            f.Write(writer);
    }

    public IEnumerator<IVmdFrame> GetEnumerator()
    {
        return Frames.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Frames).GetEnumerator();
    }
}
