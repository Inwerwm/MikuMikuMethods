using MikuMikuMethods.Extension;

namespace MikuMikuMethods.Vmd.IO;

/// <summary>
/// VMD ファイルへの書き込みを行う静的クラス。
/// </summary>
public static class VmdFileWriter
{
    /// <summary>
    /// 指定されたパスに <see cref="VocaloidMotionData"/> インスタンスを書き込みます。
    /// </summary>
    /// <param name="filePath">書き込み先のファイルパス</param>
    /// <param name="vmd">書き込む <see cref="VocaloidMotionData"/> オブジェクト</param>
    public static void Write(string filePath, VocaloidMotionData vmd)
    {
        using FileStream file = new(filePath, FileMode.Create);
        using BinaryWriter writer = new(file, Encoding.ShiftJIS);
        Write(writer, vmd);
    }

    /// <summary>
    /// 与えられた <see cref="BinaryWriter"/> を用いて <see cref="VocaloidMotionData"/> インスタンスを書き込みます。
    /// </summary>
    /// <param name="writer"><see cref="VocaloidMotionData"/> データを書き込むための <see cref="BinaryWriter"/> オブジェクト</param>
    /// <param name="vmd">書き込む <see cref="VocaloidMotionData"/> オブジェクト</param>
    public static void Write(BinaryWriter writer, VocaloidMotionData vmd)
    {
        writer.Write(vmd.Header, VmdConstants.HeaderLength, Encoding.ShiftJIS);
        writer.Write(vmd.ModelName, VmdConstants.ModelNameLength, Encoding.ShiftJIS);

        WriteFrames(writer, vmd.MotionFrames, WriteMotionFrame);
        WriteFrames(writer, vmd.MorphFrames, WriteMorphFrame);
        WriteFrames(writer, vmd.CameraFrames, WriteCameraFrame);
        WriteFrames(writer, vmd.LightFrames, WriteLightFrame);
        WriteFrames(writer, vmd.ShadowFrames, WriteShadowFrame);
        WriteFrames(writer, vmd.PropertyFrames, WritePropertyFrame);
    }

    private static void WriteFrames<T>(BinaryWriter writer, List<T> frames, Action<BinaryWriter, T> writeAction) where T : IVmdFrame
    {
        writer.Write((uint)frames.Count);
        // 時間で降順に書き込むと読み込みが早くなる(らしい)
        foreach (var f in frames.OrderByDescending(f => f.Frame))
            writeAction(writer, f);
    }

    private static void WriteMotionFrame(BinaryWriter writer, VmdMotionFrame frame)
    {
        writer.Write(frame.Name, VmdConstants.BoneNameLength, Encoding.ShiftJIS);
        writer.Write(frame.Frame);
        writer.Write(frame.Position);
        writer.Write(frame.Rotation);
        writer.Write(InterpolationCurve.CreateVMDFormatBytes(frame.InterpolationCurves, frame.FrameKind));
    }

    private static void WriteMorphFrame(BinaryWriter writer, VmdMorphFrame frame)
    {
        writer.Write(frame.Name, VmdConstants.MorphNameLength, Encoding.ShiftJIS);
        writer.Write(frame.Frame);
        writer.Write(frame.Weight);
    }

    private static void WriteCameraFrame(BinaryWriter writer, VmdCameraFrame frame)
    {
        writer.Write(frame.Frame);
        writer.Write(frame.Distance);
        writer.Write(frame.Position);
        writer.Write(frame.Rotation);
        writer.Write(InterpolationCurve.CreateVMDFormatBytes(frame.InterpolationCurves, frame.FrameKind));
        writer.Write(frame.ViewAngle);
        writer.Write(frame.IsPerspectiveOff);
    }

    private static void WriteLightFrame(BinaryWriter writer, VmdLightFrame frame)
    {
        writer.Write(frame.Frame);
        writer.Write(frame.Color, false);
        writer.Write(frame.Position);
    }

    private static void WriteShadowFrame(BinaryWriter writer, VmdShadowFrame frame)
    {
        writer.Write(frame.Frame);
        writer.Write((byte)frame.Mode);
        writer.Write(frame.Range);
    }

    private static void WritePropertyFrame(BinaryWriter writer, VmdPropertyFrame frame)
    {
        writer.Write(frame.Frame);
        writer.Write(frame.IsVisible);

        writer.Write(frame.IKEnabled.Count);
        foreach (var p in frame.IKEnabled)
        {
            writer.Write(p.Key, VmdConstants.IKNameLength, Encoding.ShiftJIS);
            writer.Write(p.Value);
        }
    }
}
