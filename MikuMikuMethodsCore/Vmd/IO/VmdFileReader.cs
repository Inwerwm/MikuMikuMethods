using MikuMikuMethods.Extension;

namespace MikuMikuMethods.Vmd.IO;

internal static class VmdFileReader
{
    public static void Read(string filePath, VocaloidMotionData vmd)
    {
        using FileStream stream = new(filePath, FileMode.Open);
        using BinaryReader reader = new(stream, Encoding.ShiftJIS);
        Read(reader, vmd);
    }

    public static void Read(BinaryReader reader, VocaloidMotionData vmd)
    {
        vmd.Header = reader.ReadString(VmdConstants.HeaderLength, Encoding.ShiftJIS, '\0');
        vmd.ModelName = reader.ReadString(VmdConstants.ModelNameLength, Encoding.ShiftJIS, '\0');

        ReadFrames(reader, r => vmd.MotionFrames.Add(new(r)));
        ReadFrames(reader, r => vmd.MorphFrames.Add(new(r)));
        ReadFrames(reader, r => vmd.CameraFrames.Add(new(r)));
        ReadFrames(reader, r => vmd.LightFrames.Add(new(r)));
        ReadFrames(reader, r => vmd.ShadowFrames.Add(new(r)));
        ReadFrames(reader, r => vmd.PropertyFrames.Add(new(r)));
    }

    private static void ReadFrames(BinaryReader reader, Action<BinaryReader> addToList)
    {
        var elementNum = reader.ReadUInt32();
        for (int i = 0; i < elementNum; i++)
            addToList(reader);
    }

}
