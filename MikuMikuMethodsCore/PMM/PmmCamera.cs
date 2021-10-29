using MikuMikuMethods.PMM.ElementState;
using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;

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
        public PmmCameraState UncomittedState { get; } = new();

        /// <summary>
        /// 視点追従のOn/Off
        /// </summary>
        public bool EnableViewPointFollowing { get; set; } = false;
    }
}