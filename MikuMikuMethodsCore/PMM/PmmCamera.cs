using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.Pmm
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
        public PmmCameraState Current { get; } = new();

        /// <summary>
        /// 視点追従のOn/Off
        /// </summary>
        public bool EnableViewPointFollowing { get; set; } = false;
    }
}