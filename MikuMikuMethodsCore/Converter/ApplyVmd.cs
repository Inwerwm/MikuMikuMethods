using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.Frame;
using MikuMikuMethods.Vmd;
using MikuMikuMethods.Common;

namespace MikuMikuMethods.Converter;
public static class ApplyVmd
{
    public static void ApplyCameraVmd(this PolygonMovieMaker pmm, VocaloidMotionData cameraVmd)
    {
        if (cameraVmd.Kind != VmdKind.Camera) throw new ArgumentException("The Model VMD was passed as the argument where the Camera VMD was expected.");

        foreach (var frame in cameraVmd.CameraFrames)
        {
            pmm.Camera.Frames.AddOrOverWrite(frame.ToPmmFrame(), (left, right) => left.Frame == right.Frame);
        }

        foreach (var frame in cameraVmd.LightFrames)
        {
            pmm.Light.Frames.AddOrOverWrite(frame.ToPmmFrame(), (left, right) => left.Frame == right.Frame);
        }

        foreach (var frame in cameraVmd.ShadowFrames)
        {
            pmm.SelfShadow.Frames.AddOrOverWrite(frame.ToPmmFrame(), (left, right) => left.Frame == right.Frame);
        }
    }

    public static void ApplyModelVmd(this PmmModel model, VocaloidMotionData modelVmd)
    {
        if (modelVmd.Kind != VmdKind.Model) throw new ArgumentException("The Camera VMD was passed as the argument where the Model VMD was expected.");

        foreach (var frame in modelVmd.MotionFrames)
        {
            var targetBone = model.Bones.FirstOrDefault(bone => bone.Name == frame.Name);
            targetBone?.Frames.AddOrOverWrite(frame.ToPmmFrame(), (left, right) => left.Frame == right.Frame);
        }

        foreach (var frame in modelVmd.MorphFrames)
        {
            var targetMorph = model.Morphs.FirstOrDefault(morph => morph.Name == frame.Name);
            targetMorph?.Frames.AddOrOverWrite(frame.ToPmmFrame(), (left, right) => left.Frame == right.Frame);
        }

        foreach (var frame in modelVmd.PropertyFrames)
        {
            model.ConfigFrames.AddOrOverWrite(frame.ToPmmFrame(model.Bones), (left, right) => left.Frame == right.Frame);
        }
    }

    private static PmmCameraFrame ToPmmFrame(this VmdCameraFrame frame) => new()
    {
        Frame = (int)frame.Frame,
        Distance = frame.Distance,
        EyePosition = frame.Position,
        Rotation = frame.Rotation,
        ViewAngle = (int)frame.ViewAngle,
        DisablePerspective = frame.IsPerspectiveOff,
        InterpolationCurves = frame.InterpolationCurves,
    };

    public static PmmLightFrame ToPmmFrame(this VmdLightFrame frame) => new()
    {
        Frame = (int)frame.Frame,
        Color = frame.Color,
        Position = frame.Position,
    };

    public static PmmSelfShadowFrame ToPmmFrame(this VmdShadowFrame frame) => new()
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
        EnableIK = bones.Where(bone => bone.IsIK).ToDictionary(
            ikBone => ikBone,
            ikBone => frame.IKEnabled.ContainsKey(ikBone.Name) ? frame.IKEnabled[ikBone.Name] : true
        ),
    };
}
