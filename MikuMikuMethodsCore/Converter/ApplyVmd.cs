using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.Frame;
using MikuMikuMethods.Vmd;

namespace MikuMikuMethods.Converter;
public static class ApplyVmd
{
    public static void ApplyCameraVmd(this PolygonMovieMaker pmm, VocaloidMotionData cameraVmd, uint offset = 0, CameraMotionOptions? options = default)
    {
        if (cameraVmd.Kind != VmdKind.Camera) throw new ArgumentException("The Model VMD was passed as the argument where the Camera VMD was expected.");

        if (options is null) options = new CameraMotionOptions()
        {
            Camera = true,
            Light = true,
            Shadow = true,
        };

        if (options.Camera)
        {
            foreach (var frame in cameraVmd.CameraFrames)
            {
                pmm.Camera.Frames.AddOrOverWrite(frame.ToPmmFrame(offset), (left, right) => left.Frame == right.Frame);
            }
        }

        if (options.Light)
        {
            foreach (var frame in cameraVmd.LightFrames)
            {
                pmm.Light.Frames.AddOrOverWrite(frame.ToPmmFrame(offset), (left, right) => left.Frame == right.Frame);
            }
        }

        if (options.Shadow)
        {
            foreach (var frame in cameraVmd.ShadowFrames)
            {
                pmm.SelfShadow.Frames.AddOrOverWrite(frame.ToPmmFrame(offset), (left, right) => left.Frame == right.Frame);
            }
        }
    }

    public static void ApplyModelVmd(this PmmModel model, VocaloidMotionData modelVmd, uint offset = 0, ModelMotionOptions? options = default)
    {
        if (modelVmd.Kind != VmdKind.Model) throw new ArgumentException("The Camera VMD was passed as the argument where the Model VMD was expected.");

        if (options is null) options = new ModelMotionOptions()
        {
            Motion = true,
            Morph = true,
            Property = true,
        };

        if (options.Motion)
        {
            foreach (var frame in modelVmd.MotionFrames)
            {
                var targetBone = model.Bones.FirstOrDefault(bone => bone.Name == frame.Name);
                targetBone?.Frames.AddOrOverWrite(frame.ToPmmFrame(offset), (left, right) => left.Frame == right.Frame);
            }
        }

        if (options.Morph)
        {
            foreach (var frame in modelVmd.MorphFrames)
            {
                var targetMorph = model.Morphs.FirstOrDefault(morph => morph.Name == frame.Name);
                targetMorph?.Frames.AddOrOverWrite(frame.ToPmmFrame(offset), (left, right) => left.Frame == right.Frame);
            }
        }

        if (options.Property)
        {
            foreach (var frame in modelVmd.PropertyFrames)
            {
                model.ConfigFrames.AddOrOverWrite(frame.ToPmmFrame(model.Bones, offset), (left, right) => left.Frame == right.Frame);
            }
        }
    }

    private static PmmCameraFrame ToPmmFrame(this VmdCameraFrame frame, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        Distance = frame.Distance,
        EyePosition = frame.Position,
        Rotation = frame.Rotation,
        ViewAngle = (int)frame.ViewAngle,
        DisablePerspective = frame.IsPerspectiveOff,
        InterpolationCurves = frame.InterpolationCurves,
    };

    public static PmmLightFrame ToPmmFrame(this VmdLightFrame frame, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        Color = frame.Color,
        Position = frame.Position,
    };

    public static PmmSelfShadowFrame ToPmmFrame(this VmdShadowFrame frame, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        ShadowMode = frame.Mode,
        ShadowRange = frame.Range,
    };

    public static PmmBoneFrame ToPmmFrame(this VmdMotionFrame frame, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        Movement = frame.Position,
        Rotation = frame.Rotation,
        InterpolationCurves = new(frame.InterpolationCurves),
        EnablePhysic = true
    };

    public static PmmMorphFrame ToPmmFrame(this VmdMorphFrame frame, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        Weight = frame.Weight,
    };

    public static PmmModelConfigFrame ToPmmFrame(this VmdPropertyFrame frame, IEnumerable<PmmBone> bones, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        Visible = frame.IsVisible,
        EnableIK = bones.Where(bone => bone.IsIK).ToDictionary(
            ikBone => ikBone,
            ikBone => frame.IKEnabled.ContainsKey(ikBone.Name) ? frame.IKEnabled[ikBone.Name] : true
        ),
    };
}
