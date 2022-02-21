using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.Frame;
using MikuMikuMethods.Vmd;

namespace MikuMikuMethods.Converter;
public static class ApplyVmd
{
    public static void ApplyCameraVmd(this PolygonMovieMaker pmm, VocaloidMotionData cameraVmd)
    {
        if (cameraVmd.Kind != VmdKind.Camera) throw new ArgumentException("The Model VMD was passed as the argument where the Camera VMD was expected.");

        pmm.Camera.Frames.AddRange(cameraVmd.CameraFrames.Select(ToPmmFrame));
        pmm.Light.Frames.AddRange(cameraVmd.LightFrames.Select(ToPmmFrame));
        pmm.SelfShadow.Frames.AddRange(cameraVmd.ShadowFrames.Select(ToPmmFrame));
    }

    public static void ApplyModelVmd(this PmmModel model, VocaloidMotionData modelVmd)
    {
        if (modelVmd.Kind != VmdKind.Model) throw new ArgumentException("The Camera VMD was passed as the argument where the Model VMD was expected.");

        foreach (var frame in modelVmd.MotionFrames)
        {
            var targetBone = model.Bones.FirstOrDefault(bone => bone.Name == frame.Name);
            targetBone?.Frames.Add(frame.ToPmmFrame());
        }

        foreach (var frame in modelVmd.MorphFrames)
        {
            var targetMorph = model.Morphs.FirstOrDefault(morph => morph.Name == frame.Name);
            targetMorph?.Frames.Add(frame.ToPmmFrame());
        }

        foreach (var frame in modelVmd.PropertyFrames)
        {
            model.ConfigFrames.Add(frame.ToPmmFrame(model.Bones));
        }
    }

    private static PmmCameraFrame ToPmmFrame(VmdCameraFrame frame) => new()
    {
        Frame = (int)frame.Frame,
        Distance = frame.Distance,
        EyePosition = frame.Position,
        Rotation = frame.Rotation,
        ViewAngle = (int)frame.ViewAngle,
        EnablePerspective = !frame.IsPerspectiveOff,
        InterpolationCurves = frame.InterpolationCurves,
    };

    public static PmmLightFrame ToPmmFrame(VmdLightFrame frame) => new()
    {
        Frame = (int)frame.Frame,
        Color = frame.Color,
        Position = frame.Position,
    };

    public static PmmSelfShadowFrame ToPmmFrame(VmdShadowFrame frame) => new()
    {
        Frame = (int)frame.Frame,
        ShadowMode = frame.Mode,
        ShadowRange = frame.Range,
    };

    public static PmmBoneFrame ToPmmFrame(this VmdMotionFrame frame) => new()
    {
        Frame = (int)frame.Frame,
        Movement = frame.Position,
        Rotation = frame.Rotation,
        InterpolationCurves = new(frame.InterpolationCurves),
    };

    public static PmmMorphFrame ToPmmFrame(this VmdMorphFrame frame) => new()
    {
        Frame = (int)frame.Frame,
        Weight = frame.Weight,
    };

    public static PmmModelConfigFrame ToPmmFrame(this VmdPropertyFrame frame, IEnumerable<PmmBone> bones) => new()
    {
        Frame = (int)frame.Frame,
        Visible = frame.IsVisible,
        EnableIK = frame.IKEnabled.Select(p => 
            (
                Key: bones.FirstOrDefault(b => b.Name == p.Key),
                p.Value
            )
        ).Where(p => p.Key is not null).ToDictionary(p => p.Key!, p => p.Value),
    };
}
