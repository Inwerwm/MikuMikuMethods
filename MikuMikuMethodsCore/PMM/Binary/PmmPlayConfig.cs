namespace MikuMikuMethods.Binary.PMM
{
    /// <summary>
    /// Pmmの再生設定
    /// </summary>
    public class PmmPlayConfig
    {
        /// <summary>
        /// 視点追従対象
        /// </summary>
        public TrackingTarget CameraTrackingTarget { get; set; }

        /// <summary>
        /// 繰り返し再生を行うか
        /// </summary>
        public bool IsRepeat { get; set; }
        /// <summary>
        /// フレ・ストップ
        /// </summary>
        public bool IsMoveCurrentFrameToStopFrame { get; set; }
        /// <summary>
        /// フレ・スタート
        /// </summary>
        public bool IsStartFromCurrentFrame { get; set; }

        /// <summary>
        /// 再生開始フレーム
        /// </summary>
        public int PlayStartFrame { get; set; }
        /// <summary>
        /// 再生停止フレーム
        /// </summary>
        public int PlayStopFrame { get; set; }

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
}
