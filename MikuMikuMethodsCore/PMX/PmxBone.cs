using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
        /// 変形改装
        /// </summary>
        public int TransformOrder { get; set; }

        /// <summary>
        /// 接続先指定方法
        /// </summary>
        public ConnectionTargetType ConnectionTargetType { get; set; }
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
        /// バイナリ読込コンストラクタ
        /// </summary>
        /// <param name="reader">読み込み対象のリーダー</param>
        public PmxBone(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// データをバイナリから読み込む
        /// </summary>
        /// <param name="reader">読み込み対象のリーダー</param>
        public void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// データをバイナリで書き込む
        /// </summary>
        /// <param name="writer">書き込み対象のライター</param>
        public void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }

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
}