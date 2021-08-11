using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMX
{
    /// <summary>
    /// SoftBody (未実装)
    /// </summary>
    public class PmxSoftBody : IPmxData
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
        /// 形状
        /// </summary>
        public ShapeType Shape { get; set; }

        /// <summary>
        /// 関連材質
        /// </summary>
        public PmxMaterial RelationMaterial { get; set; }

        /// <summary>
        /// グループ
        /// </summary>
        public byte Group { get; set; }
        /// <summary>
        /// 非衝突グループフラグ
        /// </summary>
        public ushort NonCollisionFlag { get; set; }

        /// <summary>
        /// B-Link 作成フラグ
        /// </summary>
        public bool IsCreateBLink { get; set; }
        /// <summary>
        /// クラスタ作成フラグ
        /// </summary>
        public bool IsCreateCluster { get; set; }
        /// <summary>
        /// リンク交雑フラグ
        /// </summary>
        public bool IsLinkCrossing { get; set; }

        /// <summary>
        /// B-Link 作成距離
        /// </summary>
        public int BLinkCreationDistance { get; set; }
        /// <summary>
        /// クラスタ数
        /// </summary>
        public int NumOfCluster { get; set; }

        /// <summary>
        /// 総質量
        /// </summary>
        public float SumOfMass { get; set; }
        /// <summary>
        /// 衝突マージン
        /// </summary>
        public float MarginOfCollision { get; set; }

        public AeroModelType AeroModel { get; set; }

        public ConfigContainer Config { get; } = new();
        public ClusterParameterContainer ClusterParameter { get; } = new();
        public IterationParameterContainer IterationParameter { get; } = new();
        public MaterialParameterContainer MaterialParameter { get; } = new();

        /// <summary>
        /// アンカー剛体
        /// </summary>
        public List<Anchor> Anchors { get; } = new();
        /// <summary>
        /// ピン頂点
        /// </summary>
        public List<PmxVertex> Pins { get; } = new();

        /// <summary>
        /// 形状種別
        /// </summary>
        public enum ShapeType : byte
        {
            /// <summary>
            /// TriMesh
            /// </summary>
            TriMesh,
            /// <summary>
            /// Rope
            /// </summary>
            Rope
        }
        public enum AeroModelType : int
        {
            V_Point,
            V_TwoSided,
            V_OneSided,
            F_TwoSided,
            F_OneSided
        }
        internal enum BitFlag : byte
        {
            /// <summary>
            /// B-Link 作成
            /// </summary>
            CreateBLink = 0x01,
            /// <summary>
            /// クラスタ作成
            /// </summary>
            CreateCluster = 0x02,
            /// <summary>
            /// リンク交雑
            /// </summary>
            LinkCrossing = 0x04
        }

        /// <summary>
        /// アンカー剛体
        /// </summary>
        public class Anchor
        {
            /// <summary>
            /// 関連剛体
            /// </summary>
            public PmxBody RelationBody { get; set; }
            /// <summary>
            /// 関連頂点
            /// </summary>
            public PmxVertex RelationVertex { get; set; }
            /// <summary>
            /// Nearモード
            /// </summary>
            public bool IsNearMode { get; set; }
        }

        public class ConfigContainer
        {
            public float VCF { get; set; }
            public float DP { get; set; }
            public float DG { get; set; }
            public float LF { get; set; }
            public float PR { get; set; }
            public float VC { get; set; }
            public float DF { get; set; }
            public float MT { get; set; }
            public float CHR { get; set; }
            public float KHR { get; set; }
            public float SHR { get; set; }
            public float AHR { get; set; }
        }
        public class ClusterParameterContainer
        {
            public float SRHR_CL { get; set; }
            public float SKHR_CL { get; set; }
            public float SSHR_CL { get; set; }
            public float SR_SPLT_CL { get; set; }
            public float SK_SPLT_CL { get; set; }
            public float SS_SPLT_CL { get; set; }
        }
        public class IterationParameterContainer
        {
            public int V_IT { get; set; }
            public int P_IT { get; set; }
            public int D_IT { get; set; }
            public int C_IT { get; set; }
        }
        public class MaterialParameterContainer
        {
            public float LST { get; set; }
            public float AST { get; set; }
            public float VST { get; set; }
        }
    }
}
