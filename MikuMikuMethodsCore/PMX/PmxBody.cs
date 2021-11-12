using System.Numerics;

namespace MikuMikuMethods.Pmx
{
    /// <summary>
    /// 剛体
    /// </summary>
    public class PmxBody : IPmxData
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 英名
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// 関連ボーン
        /// </summary>
        public PmxBone RelationBone { get; set; }

        /// <summary>
        /// グループ
        /// </summary>
        public byte Group { get; set; }
        /// <summary>
        /// 非衝突グループフラグ
        /// </summary>
        public ushort NonCollisionFlag { get; set; }

        /// <summary>
        /// 形状
        /// </summary>
        public ShapeType Shape { get; set; }

        /// <summary>
        /// 大きさ
        /// </summary>
        public Vector3 Size { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// 回転
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// 質量
        /// </summary>
        public float Mass { get; set; }
        /// <summary>
        /// 移動減衰
        /// </summary>
        public float MovingDecay { get; set; }
        /// <summary>
        /// 回転減衰
        /// </summary>
        public float RotationDecay { get; set; }
        /// <summary>
        /// 反発力
        /// </summary>
        public float Resiliency { get; set; }
        /// <summary>
        /// 摩擦力
        /// </summary>
        public float Friction { get; set; }

        /// <summary>
        /// 物理モード
        /// </summary>
        public PhysicsModeType PhysicsMode { get; set; }

        /// <summary>
        /// 形状種別
        /// </summary>
        public enum ShapeType : byte
        {
            /// <summary>
            /// 球
            /// </summary>
            Spere,
            /// <summary>
            /// 箱
            /// </summary>
            Box,
            /// <summary>
            /// カプセル
            /// </summary>
            Capsure
        }

        /// <summary>
        /// 物理モード
        /// </summary>
        public enum PhysicsModeType : byte
        {
            /// <summary>
            /// ボーン追従
            /// </summary>
            Static,
            /// <summary>
            /// 物理演算
            /// </summary>
            Dynamic,
            /// <summary>
            /// 物理演算+ボーン位置合わせ
            /// </summary>
            Register
        }

        public override string ToString() => $"{Name} - {Position}";
    }
}
