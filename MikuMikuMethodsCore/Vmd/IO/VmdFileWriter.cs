using MikuMikuMethods.Extension;

namespace MikuMikuMethods.Vmd.IO;

public static class VmdFileWriter
{
    public static void Write(string filePath, VocaloidMotionData vmd)
    {
        using FileStream file = new(filePath, FileMode.Create);
        using BinaryWriter writer = new(file, Encoding.ShiftJIS);
        Write(writer, vmd);
    }

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

    public static void WriteMotionFrame(BinaryWriter writer, VmdMotionFrame frame)
    {
        writer.Write(frame.Name, VmdConstants.BoneNameLength, Encoding.ShiftJIS);
        writer.Write(frame.Frame);
        writer.Write(frame.Position);
        writer.Write(frame.Rotation);
        writer.Write(InterpolationCurve.CreateVMDFormatBytes(frame.InterpolationCurves, frame.FrameKind));
    }

    public static void WriteMorphFrame(BinaryWriter writer, VmdMorphFrame frame)
    {
        writer.Write(frame.Name, VmdConstants.MorphNameLength, Encoding.ShiftJIS);
        writer.Write(frame.Frame);
        writer.Write(frame.Weight);
    }

    public static void WriteCameraFrame(BinaryWriter writer, VmdCameraFrame frame)
    {
        writer.Write(frame.Frame);
        writer.Write(frame.Distance);
        writer.Write(frame.Position);
        writer.Write(frame.Rotation);
        writer.Write(InterpolationCurve.CreateVMDFormatBytes(frame.InterpolationCurves, frame.FrameKind));
        writer.Write(frame.ViewAngle);
        writer.Write(frame.IsPerspectiveOff);
    }

    public static void WriteLightFrame(BinaryWriter writer, VmdLightFrame frame)
    {
        writer.Write(frame.Frame);
        writer.Write(frame.Color, false);
        writer.Write(frame.Position);
    }

    public static void WriteShadowFrame(BinaryWriter writer, VmdShadowFrame frame)
    {
        writer.Write(frame.Frame);
        writer.Write((byte)frame.Mode);
        writer.Write(frame.Range);
    }

    public static void WritePropertyFrame(BinaryWriter writer, VmdPropertyFrame frame)
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
