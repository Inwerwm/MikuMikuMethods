using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// カメラ
/// </summary>
public class PmmCamera
{
    /// <summary>
    /// カメラのキーフレーム
    /// </summary>
    public List<PmmCameraFrame> Frames { get; private init; } = new();
    /// <summary>
    /// カメラの編集状態
    /// </summary>
    public PmmCameraState Current { get; private init; } = new();

    /// <summary>
    /// 視点追従のOn/Off
    /// </summary>
    public bool EnableViewPointFollowing { get; set; } = false;

    /// <summary>
    /// PMM移行用ディープコピー
    /// </summary>
    /// <param name="modelMap">移行前モデルと移行先モデルの対応辞書</param>
    /// <param name="boneMap">移行前ボーンと移行先ボーンの対応辞書</param>
    /// <returns>複製</returns>
    public PmmCamera DeepCopy(Dictionary<PmmModel, PmmModel> modelMap, Dictionary<PmmBone, PmmBone> boneMap) => new()
    {
        Frames = this.Frames.Select(f => f.DeepCopy(modelMap.GetOrDefault(f.FollowingModel), boneMap.GetOrDefault(f.FollowingBone))).ToList(),
        Current = this.Current.DeepCopy(modelMap.GetOrDefault(this.Current.FollowingModel), boneMap.GetOrDefault(this.Current.FollowingBone)),
        EnableViewPointFollowing = this.EnableViewPointFollowing,
    };
}
