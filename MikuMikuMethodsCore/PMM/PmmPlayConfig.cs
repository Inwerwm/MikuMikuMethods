namespace MikuMikuMethods.Pmm;

public class PmmPlayConfig
{
    /// <summary>
    /// 繰り返し再生を行うか
    /// </summary>
    public bool EnableRepeat { get; set; } = false;
    /// <summary>
    /// フレ・ストップ
    /// </summary>
    public bool EnableMoveCurrentFrameToPlayStopping { get; set; } = false;
    /// <summary>
    /// フレ・スタート
    /// </summary>
    public bool EnableStartFromCurrentFrame { get; set; } = false;

    /// <summary>
    /// 再生開始フレーム
    /// </summary>
    public int PlayStartFrame { get; set; } = 0;
    /// <summary>
    /// 再生停止フレーム
    /// </summary>
    public int PlayStopFrame { get; set; } = 0;

    /// <summary>
    /// 再生時のカメラ追従が有効か
    /// </summary>
    public bool EnableFollowCamera { get; set; }
    /// <summary>
    /// 視点追従対象
    /// </summary>
    public TrackingTarget CameraTrackingTarget { get; set; } = TrackingTarget.None;


    /// <summary>
    /// 視点追従対象
    /// </summary>
    public enum TrackingTarget : byte
    {
        /// <summary>
        /// なし
        /// </summary>
        None,
        /// <summary>
        /// モデル
        /// </summary>
        Model,
        /// <summary>
        /// ボーン
        /// </summary>
        Bone,
    }
}
