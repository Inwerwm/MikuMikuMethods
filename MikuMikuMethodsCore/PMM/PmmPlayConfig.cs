﻿using System.IO;

namespace MikuMikuMethods.PMM
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
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void Read(BinaryReader reader)
        {
            CameraTrackingTarget = (TrackingTarget)reader.ReadByte();

            IsRepeat = reader.ReadBoolean();
            IsMoveCurrentFrameToStopFrame = reader.ReadBoolean();
            IsStartFromCurrentFrame = reader.ReadBoolean();

            PlayStartFrame = reader.ReadInt32();
            PlayStopFrame = reader.ReadInt32();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write((byte)CameraTrackingTarget);

            writer.Write(IsRepeat);
            writer.Write(IsMoveCurrentFrameToStopFrame);
            writer.Write(IsStartFromCurrentFrame);

            writer.Write(PlayStartFrame);
            writer.Write(PlayStopFrame);
        }

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
