using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.Frame;
using MikuMikuMethods.Vmd;

namespace MikuMikuMethods.Converter;

public static class ExtractMotion
{
    /// <summary>
    /// カメラモーション VMD を PMM から抽出して作成する
    /// </summary>
    /// <param name="pmm">もととなる PMM</param>
    /// <param name="options">抽出設定</param>
    /// <returns>カメラモーション VMD</returns>
    public static VocaloidMotionData ExtractCameraMotion(this PolygonMovieMaker pmm, CameraMotionExtractionOptions? options = default)
    {
        // 引数の既定値ではコンパイル時定数しか無理なのでここで null 時の規定値を入れる
        if (options is null) options = new();

        // ラムダ式でキャプチャしないと options が not null であることがフロー解析で確定できない
        Func<IPmmFrame, bool> isFrameInRange = frame => options.StartFrame <= frame.Frame && frame.Frame <= (options.EndFrame ?? uint.MaxValue);

        var vmd = new VocaloidMotionData();
        vmd.ModelName = VocaloidMotionData.CameraTypeVMDName;

        if (options.Camera)
        {
            vmd.CameraFrames.AddRange(pmm.Camera.Frames.Where(isFrameInRange).Select(f => ((PmmCameraFrame)f).ToVmdFrame()));
        }
        if (options.Light)
        {
            vmd.LightFrames.AddRange(pmm.Light.Frames.Where(isFrameInRange).Select(f => ((PmmLightFrame)f).ToVmdFrame()));
        }
        if (options.Shadow)
        {
            vmd.ShadowFrames.AddRange(pmm.SelfShadow.Frames.Where(isFrameInRange).Select(f => ((PmmSelfShadowFrame)f).ToVmdFrame()));
        }

        return vmd;
    }

    /// <summary>
    /// モデルモーション VMD を PMM から抽出して作成する
    /// </summary>
    /// <param name="model">もととなるモデル</param>
    /// <param name="options">抽出設定</param>
    /// <returns>モデルモーション VMD</returns>
    public static VocaloidMotionData ExtractModelMotion(this PmmModel model, ModelMotionExtractionOptions? options = default)
    {
        // 引数の既定値ではコンパイル時定数しか無理なのでここで null 時の規定値を入れる
        if (options is null) options = new();

        // ラムダ式でキャプチャしないと options が not null であることがフロー解析で確定できない
        Func<IPmmFrame, bool> isFrameInRange = frame => options.StartFrame <= frame.Frame && frame.Frame <= (options.EndFrame ?? uint.MaxValue);

        return new()
        {
            ModelName = model.Name,
            MotionFrames = options.Motion ? model.Bones.SelectMany(bone => bone.Frames.Where(isFrameInRange).Select(f => ((PmmBoneFrame)f).ToVmdFrame(bone.Name))).ToList() : new(),
            MorphFrames = options.Morph ? model.Morphs.SelectMany(morph => morph.Frames.Where(isFrameInRange).Select(f => ((PmmMorphFrame)f).ToVmdFrame(morph.Name))).ToList() : new(),
            PropertyFrames = options.Property ? model.ConfigFrames.Where(isFrameInRange).Select(f => ((PmmModelConfigFrame)f).ToVmdFrame()).ToList() : new(),
        };
    }

    public static VmdCameraFrame ToVmdFrame(this PmmCameraFrame frame) => new((uint)frame.Frame)
    {
        Distance = frame.Distance,
        Position = frame.EyePosition,
        Rotation = frame.Rotation,
        ViewAngle = (uint)frame.ViewAngle,
        IsPerspectiveOff = frame.DisablePerspective,
        InterpolationCurves = new(frame.InterpolationCurves),
    };

    public static VmdLightFrame ToVmdFrame(this PmmLightFrame frame) => new((uint)frame.Frame)
    {
        Color = frame.Color,
        Position = frame.Position,
    };

    public static VmdShadowFrame ToVmdFrame(this PmmSelfShadowFrame frame) => new((uint)frame.Frame)
    {
        Mode = frame.ShadowMode,
        Range = frame.ShadowRange,
    };

    public static VmdMotionFrame ToVmdFrame(this PmmBoneFrame frame, string boneName) => new(boneName ,(uint)frame.Frame)
    {
        Position = frame.Movement,
        Rotation = frame.Rotation,
        InterpolationCurves = new(frame.InterpolationCurves),
    };

    public static VmdMorphFrame ToVmdFrame(this PmmMorphFrame frame, string morphName) => new(morphName, (uint)frame.Frame)
    {
        Weight = frame.Weight,
    };

    public static VmdPropertyFrame ToVmdFrame(this PmmModelConfigFrame frame) => new((uint)frame.Frame)
    {
        IsVisible = frame.Visible,
        IKEnabled = frame.EnableIK.ToDictionary(p => p.Key.Name, p => p.Value),
    };
}
