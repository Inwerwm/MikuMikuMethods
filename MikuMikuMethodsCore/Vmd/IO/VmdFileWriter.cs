using MikuMikuMethods.Extension;

namespace MikuMikuMethods.Vmd.IO;

internal static class VmdFileWriter
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

        WriteFrames(writer, vmd.MotionFrames);
        WriteFrames(writer, vmd.MorphFrames);
        WriteFrames(writer, vmd.CameraFrames);
        WriteFrames(writer, vmd.LightFrames);
        WriteFrames(writer, vmd.ShadowFrames);
        WriteFrames(writer, vmd.PropertyFrames);
    }

    private static void WriteFrames<T>(BinaryWriter writer, List<T> frames) where T : IVmdFrame
    {
        writer.Write((uint)frames.Count);
        // 時間で降順に書き込むと読み込みが早くなる(らしい)
        foreach (var f in frames.OrderByDescending(f => f.Frame))
            f.Write(writer);
    }
}
