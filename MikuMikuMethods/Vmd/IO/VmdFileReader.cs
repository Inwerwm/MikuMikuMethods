using MikuMikuMethods.Extension;

namespace MikuMikuMethods.Vmd.IO;

/// <summary>
/// VMD ファイル読込
/// </summary>
public static class VmdFileReader
{
    /// <summary>
    /// VMD ファイルの読込
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="vmd">データ書込み先 VMD</param>
    public static void Read(string filePath, VocaloidMotionData vmd)
    {
        using FileStream stream = new(filePath, FileMode.Open);
        using BinaryReader reader = new(stream, Encoding.ShiftJIS);
        Read(reader, vmd);
    }

    /// <summary>
    /// VMD ファイルの読込
    /// </summary>
    /// <param name="reader">バイナリリーダー</param>
    /// <param name="vmd">データ書込み先 VMD</param>
    public static void Read(BinaryReader reader, VocaloidMotionData vmd)
    {
        vmd.Header = reader.ReadString(VmdConstants.HeaderLength, Encoding.ShiftJIS, '\0');
        vmd.ModelName = reader.ReadString(VmdConstants.ModelNameLength, Encoding.ShiftJIS, '\0');

        ReadFrames(reader, r => vmd.MotionFrames.Add(ReadMotionFrame(r)));
        ReadFrames(reader, r => vmd.MorphFrames.Add(ReadMorphFrame(r)));
        ReadFrames(reader, r => vmd.CameraFrames.Add(ReadCameraFrame(r)));
        ReadFrames(reader, r => vmd.LightFrames.Add(ReadLightFrame(r)));
        ReadFrames(reader, r => vmd.ShadowFrames.Add(ReadShadowFrame(r)));
        ReadFrames(reader, r => vmd.PropertyFrames.Add(ReadPropertyFrame(r)));
    }

    private static void ReadFrames(BinaryReader reader, Action<BinaryReader> addToList)
    {
        if (reader.BaseStream.Position >= reader.BaseStream.Length)
        {
            return;
        }

        var elementNum = reader.ReadUInt32();
        for (int i = 0; i < elementNum; i++)
            addToList(reader);
    }

    private static VmdMotionFrame ReadMotionFrame(BinaryReader reader) => new(reader.ReadString(VmdConstants.BoneNameLength, Encoding.ShiftJIS, '\0'), reader.ReadUInt32())
    {
        Position = reader.ReadVector3(),
        Rotation = reader.ReadQuaternion(),
        InterpolationCurves = InterpolationCurve.CreateByVMDFormat(reader.ReadBytes(64), VmdFrameKind.Motion)
    };

    private static VmdMorphFrame ReadMorphFrame(BinaryReader reader) => new(reader.ReadString(VmdConstants.MorphNameLength, Encoding.ShiftJIS, '\0'), reader.ReadUInt32())
    {
        Weight = reader.ReadSingle()
    };

    private static VmdCameraFrame ReadCameraFrame(BinaryReader reader) => new()
    {
        Frame = reader.ReadUInt32(),
        Distance = reader.ReadSingle(),
        Position = reader.ReadVector3(),
        Rotation = reader.ReadVector3(),
        InterpolationCurves = InterpolationCurve.CreateByVMDFormat(reader.ReadBytes(24), VmdFrameKind.Camera),
        ViewAngle = reader.ReadUInt32(),
        IsPerspectiveOff = reader.ReadBoolean()
    };

    private static VmdLightFrame ReadLightFrame(BinaryReader reader) => new()
    {
        Frame = reader.ReadUInt32(),
        Color = reader.ReadSingleRGB(),
        Position = reader.ReadVector3()
    };

    private static VmdShadowFrame ReadShadowFrame(BinaryReader reader) => new()
    {
        Frame = reader.ReadUInt32(),
        Mode = (Common.SelfShadow)reader.ReadByte(),
        Range = reader.ReadSingle()
    };

    private static VmdPropertyFrame ReadPropertyFrame(BinaryReader reader)
    {
        var frame = new VmdPropertyFrame(reader.ReadUInt32())
        {
            IsVisible = reader.ReadBoolean()
        };

        var ikCount = reader.ReadUInt32();
        for (int i = 0; i < ikCount; i++)
        {
            frame.IKEnabled.Add(reader.ReadString(VmdConstants.IKNameLength, Encoding.ShiftJIS, '\0'), reader.ReadBoolean());
        }

        return frame;
    }
}
