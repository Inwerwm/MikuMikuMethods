using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;
using System.Numerics;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// PMM内のカメラ情報
    /// </summary>
    public class PmmCamera
    {
        /// <summary>
        /// 初期位置のカメラフレーム
        /// </summary>
        public PmmCameraFrame InitialFrame { get; set; }
        /// <summary>
        /// カメラのキーフレーム
        /// </summary>
        public List<PmmCameraFrame> Frames { get; init; }

        /// <summary>
        /// 未確定編集状態
        /// </summary>
        public TemporaryCameraEditState Uncomitted { get; init; }

        /// <summary>
        /// 未確定カメラ追従状態
        /// </summary>
        public TemporaryCameraFollowingState UncomittedFollowing { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmCamera()
        {
            InitialFrame = new();
            Frames = new();
            Uncomitted = new();
            UncomittedFollowing = new();
        }
    }

    /// <summary>
    /// 未確定のカメラ編集状態
    /// </summary>
    public class TemporaryCameraEditState
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
        /// コンストラクタ
        /// </summary>
        public TemporaryCameraEditState()
        {
            EnablePerspective = true;
        }
    }

    /// <summary>
    /// 未確定のカメラ追従状態
    /// </summary>
    public class TemporaryCameraFollowingState
    {
        /// <summary>
        /// モデルのインデックス
        /// </summary>
        public int ModelIndex { get; set; }
        /// <summary>
        /// ボーンのインデックス
        /// </summary>
        public int BoneIndex { get; set; }
    }
}
