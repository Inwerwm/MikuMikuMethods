using System.Numerics;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// ボーン
    /// </summary>
    public class PmxBone : IPmxData
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 名前(英語)
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 親ボーン
        /// </summary>
        public PmxBone Parent { get; set; }

        /// <summary>
        /// 変形階層
        /// </summary>
        public int TransformOrder { get; set; }

        /// <summary>
        /// 接続先指定方法
        /// </summary>
        public ConnectionTargetType ConnectionTarget { get; set; }
        /// <summary>
        /// 接続先座標
        /// </summary>
        public Vector3 ConnectionTargetOffset { get; set; }
        /// <summary>
        /// 接続先ボーン
        /// </summary>
        public PmxBone ConnectionTargetBone { get; set; }

        /// <summary>
        /// 回転可能か
        /// </summary>
        public bool Rotatable { get; set; }
        /// <summary>
        /// 移動可能か
        /// </summary>
        public bool Movable { get; set; }

        /// <summary>
        /// 表示するか
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// 操作可能か
        /// </summary>
        public bool Controlable { get; set; }

        /// <summary>
        /// IKか
        /// </summary>
        public bool IsIK { get; set; }
        /// <summary>
        /// IK情報
        /// </summary>
        public PmxInverseKinematics IKInfo { get; set; }

        /// <summary>
        /// ローカル付与か
        /// </summary>
        public bool IsLocalAddition { get; set; }
        /// <summary>
        /// 回転付与するか
        /// </summary>
        public bool IsRotateAddition { get; set; }
        /// <summary>
        /// 移動付与するか
        /// </summary>
        public bool IsMoveAddtion { get; set; }

        /// <summary>
        /// 付与親
        /// </summary>
        public PmxBone AdditionParent { get; set; }
        /// <summary>
        /// 付与率
        /// </summary>
        public float AdditonRatio { get; set; }

        /// <summary>
        /// 軸固定か
        /// </summary>
        public bool IsFixedAxis { get; set; }
        /// <summary>
        /// 軸固定の方向
        /// </summary>
        public Vector3 FixVector { get; set; }

        /// <summary>
        /// ローカル軸か
        /// </summary>
        public bool IsLocalAxis { get; set; }
        /// <summary>
        /// ローカル軸X
        /// </summary>
        public Vector3 LocalAxisX { get; set; }
        /// <summary>
        /// ローカル軸Z
        /// </summary>
        public Vector3 LocalAxisZ { get; set; }

        /// <summary>
        /// 物理後変形か
        /// </summary>
        public bool IsAfterPhysic { get; set; }
        /// <summary>
        /// 外部親を使用するか
        /// MMM用らしい
        /// </summary>
        public bool UseOuterParent { get; set; }
        /// <summary>
        /// 外部親番号
        /// MMM用らしい
        /// </summary>
        public int OuterParentKey { get; set; }

        /// <summary>
        /// ボーンの接続先指定の方法
        /// </summary>
        public enum ConnectionTargetType
        {
            /// <summary>
            /// ボーンで指定
            /// </summary>
            Bone,
            /// <summary>
            /// 座標で指定
            /// </summary>
            Offset
        }

        internal enum BoneFlag : ushort
        {
            ConnectTargetType = 0x0001,
            Rotatable = 0x0002,
            Movable = 0x0004,
            Visible = 0x0008,
            Controlable = 0x0010,
            IsIK = 0x0020,

            AddLocalTarget = 0x0080,
            AddRotation = 0x0100,
            AddMoving = 0x0200,
            FixAxis = 0x0400,
            LocalAxis = 0x0800,
            TrAfterPhysic = 0x1000,
            TrOuterParent = 0x2000
        }

        public override string ToString() => $"{Name} - {Position}";
    }
}