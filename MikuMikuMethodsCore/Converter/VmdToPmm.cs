using MikuMikuMethods.Common;
using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.Frame;
using MikuMikuMethods.Vmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Converter;
public static class VmdToPmm
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
}
