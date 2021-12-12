using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.Frame;
using MikuMikuMethods.Vmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Converter;

public static class PmmAndVmd
{
    public static VocaloidMotionData ExtractCameraMotion(this PolygonMovieMaker pmm, ExtractCameraMotionOptions? options = default)
    {
        // 引数の既定値ではコンパイル時定数しか無理なのでここで null 時の規定値を入れる
        if (options is null) options = new();

        // ラムダ式でキャプチャしないと options が not null であることがフロー解析で確定できない
        Func<IPmmFrame, bool> isFrameInRange = frame => options.StartFrame <= frame.Frame && frame.Frame <= (options.EndFrame ?? uint.MaxValue);

        var vmd = new VocaloidMotionData();
        vmd.ModelName = VocaloidMotionData.CameraTypeVMDName;

        if (options.ExtractCamera)
        {
            vmd.CameraFrames.AddRange(pmm.Camera.Frames.Where(isFrameInRange).Select(f => ((PmmCameraFrame)f).ToVmdFrame()));
        }
        if (options.ExtractLight)
        {
            vmd.LightFrames.AddRange(pmm.Light.Frames.Where(isFrameInRange).Select(f => ((PmmLightFrame)f).ToVmdFrame()));
        }
        if (options.ExtractShadow)
        {
            vmd.ShadowFrames.AddRange(pmm.SelfShadow.Frames.Where(isFrameInRange).Select(f => ((PmmSelfShadowFrame)f).ToVmdFrame()));
        }

        return vmd;
    }

    public static VmdCameraFrame ToVmdFrame(this PmmCameraFrame frame) => new((uint)frame.Frame)
    {
        Distance = frame.Distance,
        Position = frame.TargetPosition,
        Rotation = frame.Rotation,
        ViewAngle = (uint)frame.ViewAngle,
        IsPerspectiveOff = !frame.EnablePerspective,
        InterpolationCurves = frame.InterpolationCurves,
    };

    public static VmdLightFrame ToVmdFrame(this PmmLightFrame frame) => new((uint)frame.Frame)
    {
        Color = frame.Color,
        Position = frame.Position,
    };

    public static VmdShadowFrame ToVmdFrame(this PmmSelfShadowFrame frame) => new((uint)frame.Frame)
    {
        Mode = (byte)frame.ShadowMode,
        Range = frame.ShadowRange,
    };
}
