using System.Numerics;

namespace MikuMikuMethods.PMM.ElementState
{
    public class PmmCameraState
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
        /// <summary>
        /// 視野角
        /// </summary>
        public int ViewAngle { get; set; } = 30;
    }
}