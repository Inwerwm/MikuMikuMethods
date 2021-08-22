using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;
using System.Numerics;

namespace MikuMikuMethods.PMM
{
    public class PmmCamera
    {
        /// <summary>
        /// カメラのキーフレーム
        /// </summary>
        public List<PmmCameraFrame> Frames { get; } = new();
        /// <summary>
        /// カメラの編集状態
        /// </summary>
        public TemporaryCameraState Current { get; } = new();

        /// <summary>
        /// 視点追従のOn/Off
        /// </summary>
        public bool EnableViewPointFollowing { get; set; } = false;

        public class TemporaryCameraState
        {
            /// <summary>
            /// カメラ位置
            /// </summary>
            public Vector3 EyePosition { get; set; }
            /// <summary>
            /// カメラ中心の位置
            /// </summary>
            public Vector3 TargetPosition { get; set; }
            /// <summary>
            /// カメラ回転
            /// </summary>
            public Vector3 Rotation { get; set; }
            /// <summary>
            /// パースのOn/Off
            /// </summary>
            public bool EnablePerspective { get; set; }
            /// <summary>
            /// 視点追従先モデル
            /// </summary>
            public PmmModel FollowingModel { get; set; }
            /// <summary>
            /// 視点追従先ボーン
            /// </summary>
            public PmmBone FollowingBone { get; set; }
        }
    }
}