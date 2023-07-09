using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.Frame;
using MikuMikuMethods.Vmd;

namespace MikuMikuMethods.Converter;

/// <summary>
/// PMM に対して VMD を適用する拡張メソッド
/// </summary>
public static class ApplyVmd
{
    /// <summary>
    /// カメラモーションを PMM に適用する
    /// </summary>
    /// <param name="pmm">対象 PMM データ</param>
    /// <param name="cameraVmd">カメラモーション</param>
    /// <param name="options">適用オプション</param>
    /// <exception cref="ArgumentException">カメラモーションではない VMD が渡された場合に投げられる</exception>
    public static void ApplyCameraVmd(this PolygonMovieMaker pmm, VocaloidMotionData cameraVmd, CameraMotionApplyingOptions? options = default)
    {
        if (cameraVmd.Kind != VmdKind.Camera) throw new ArgumentException("The Model VMD was passed as the argument where the Camera VMD was expected.");

        if (options is null) options = new CameraMotionApplyingOptions();

        if (options.Camera)
        {
            foreach (var frame in cameraVmd.CameraFrames)
            {
                pmm.Camera.Frames.AddOrOverWrite(frame.ToPmmFrame(options.Offset), (left, right) => left.Frame == right.Frame);
            }
        }

        if (options.Light)
        {
            foreach (var frame in cameraVmd.LightFrames)
            {
                pmm.Light.Frames.AddOrOverWrite(frame.ToPmmFrame(options.Offset), (left, right) => left.Frame == right.Frame);
            }
        }

        if (options.Shadow)
        {
            foreach (var frame in cameraVmd.ShadowFrames)
            {
                pmm.SelfShadow.Frames.AddOrOverWrite(frame.ToPmmFrame(options.Offset), (left, right) => left.Frame == right.Frame);
            }
        }
    }

    /// <summary>
    /// モデルモーションを PMM に適用する
    /// </summary>
    /// <param name="model">適用対象モデル</param>
    /// <param name="modelVmd">モデルモーション</param>
    /// <param name="options">適用オプション</param>
    /// <exception cref="ArgumentException"></exception>
    public static void ApplyModelVmd(this PmmModel model, VocaloidMotionData modelVmd, ModelMotionApplyingOptions? options = default)
    {
        if (modelVmd.Kind != VmdKind.Model) throw new ArgumentException("The Camera VMD was passed as the argument where the Model VMD was expected.");

        if (options is null) options = new ModelMotionApplyingOptions();

        if (options.Motion)
        {
            foreach (var frame in modelVmd.MotionFrames)
            {
                var targetBone = model.Bones.FirstOrDefault(bone => bone.Name == frame.Name);
                targetBone?.Frames.AddOrOverWrite(frame.ToPmmFrame(options.Offset), (left, right) => left.Frame == right.Frame);
            }
        }

        if (options.Morph)
        {
            foreach (var frame in modelVmd.MorphFrames)
            {
                var targetMorph = model.Morphs.FirstOrDefault(morph => morph.Name == frame.Name);
                targetMorph?.Frames.AddOrOverWrite(frame.ToPmmFrame(options.Offset), (left, right) => left.Frame == right.Frame);
            }
        }

        if (options.Property)
        {
            foreach (var frame in modelVmd.PropertyFrames)
            {
                model.ConfigFrames.AddOrOverWrite(frame.ToPmmFrame(model.Bones, options.Offset), (left, right) => left.Frame == right.Frame);
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

    /// <summary>
    /// VMD のフレーム情報を PMM のフレーム情報に変換する
    /// </summary>
    /// <param name="frame">変換対象フレーム</param>
    /// <param name="offset">フレーム時間の適用オフセット</param>
    /// <returns>PMM のフレームオブジェクト</returns>
    public static PmmLightFrame ToPmmFrame(this VmdLightFrame frame, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        Color = frame.Color,
        Position = frame.Position,
    };

    /// <summary>
    /// VMD のフレーム情報を PMM のフレーム情報に変換する
    /// </summary>
    /// <param name="frame">変換対象フレーム</param>
    /// <param name="offset">フレーム時間の適用オフセット</param>
    /// <returns>PMM のフレームオブジェクト</returns>
    public static PmmSelfShadowFrame ToPmmFrame(this VmdShadowFrame frame, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        ShadowMode = frame.Mode,
        ShadowRange = frame.Range,
    };

    /// <summary>
    /// VMD のフレーム情報を PMM のフレーム情報に変換する
    /// </summary>
    /// <param name="frame">変換対象フレーム</param>
    /// <param name="offset">フレーム時間の適用オフセット</param>
    /// <returns>PMM のフレームオブジェクト</returns>
    public static PmmBoneFrame ToPmmFrame(this VmdMotionFrame frame, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        Movement = frame.Position,
        Rotation = frame.Rotation,
        InterpolationCurves = new(frame.InterpolationCurves),
        EnablePhysic = true
    };

    /// <summary>
    /// VMD のフレーム情報を PMM のフレーム情報に変換する
    /// </summary>
    /// <param name="frame">変換対象フレーム</param>
    /// <param name="offset">フレーム時間の適用オフセット</param>
    /// <returns>PMM のフレームオブジェクト</returns>
    public static PmmMorphFrame ToPmmFrame(this VmdMorphFrame frame, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        Weight = frame.Weight,
    };

    /// <summary>
    /// VMD のフレーム情報を PMM のフレーム情報に変換する
    /// </summary>
    /// <param name="frame">変換対象フレーム</param>
    /// <param name="bones">変換対象 <see cref="PmmModel"/> に属するボーンのコレクション</param>
    /// <param name="offset">フレーム時間の適用オフセット</param>
    /// <returns>PMM のフレームオブジェクト</returns>
    public static PmmModelConfigFrame ToPmmFrame(this VmdPropertyFrame frame, IEnumerable<PmmBone> bones, uint offset) => new()
    {
        Frame = (int)(frame.Frame + offset),
        Visible = frame.IsVisible,
        EnableIK = bones.Where(bone => bone.IsIK).ToDictionary(
            ikBone => ikBone,
            ikBone => !frame.IKEnabled.ContainsKey(ikBone.Name) || frame.IKEnabled[ikBone.Name]
        ),
    };
}
