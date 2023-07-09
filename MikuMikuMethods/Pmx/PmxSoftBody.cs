namespace MikuMikuMethods.Pmx;

/// <summary>
/// SoftBody (未実装)
/// </summary>
public class PmxSoftBody : IPmxData
{
    /// <summary>
    /// 名前
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// 英名
    /// </summary>
    public string NameEn { get; set; } = "";

    /// <summary>
    /// 形状
    /// </summary>
    public ShapeType Shape { get; set; }

    /// <summary>
    /// 関連材質
    /// </summary>
    public PmxMaterial? RelationMaterial { get; set; }

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

    /// <summary>
    /// エアロモデル
    /// </summary>
    public AeroModelType AeroModel { get; set; }

    /// <summary>
    /// 設定
    /// </summary>
    public ConfigContainer Config { get; } = new();
    /// <summary>
    /// クラスターパラメーター
    /// </summary>
    public ClusterParameterContainer ClusterParameter { get; } = new();
    /// <summary>
    /// イテレーションパラメーター
    /// </summary>
    public IterationParameterContainer IterationParameter { get; } = new();
    /// <summary>
    /// マテリアルパラメーター
    /// </summary>
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
    /// <summary>
    /// エアロモデル種別
    /// </summary>
    public enum AeroModelType : int
    {
        /// <summary>
        /// V Point
        /// </summary>
        V_Point,
        /// <summary>
        /// V Two Sided
        /// </summary>
        V_TwoSided,
        /// <summary>
        /// C One Sided
        /// </summary>
        V_OneSided,
        /// <summary>
        /// F TwoSided
        /// </summary>
        F_TwoSided,
        /// <summary>
        /// F One Sided
        /// </summary>
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
        public PmxBody? RelationBody { get; set; }
        /// <summary>
        /// 関連頂点
        /// </summary>
        public PmxVertex? RelationVertex { get; set; }
        /// <summary>
        /// Nearモード
        /// </summary>
        public bool IsNearMode { get; set; }
    }

    /// <summary>
    /// Represents a container for configuration parameters of a soft body.
    /// </summary>
    public class ConfigContainer
    {
        /// <summary>
        /// Gets or sets the volume conservation coefficient.
        /// </summary>
        public float VCF { get; set; }

        /// <summary>
        /// Gets or sets the dynamic pose matching coefficient.
        /// </summary>
        public float DP { get; set; }

        /// <summary>
        /// Gets or sets the drag coefficient.
        /// </summary>
        public float DG { get; set; }

        /// <summary>
        /// Gets or sets the lift coefficient.
        /// </summary>
        public float LF { get; set; }

        /// <summary>
        /// Gets or sets the pressure coefficient.
        /// </summary>
        public float PR { get; set; }

        /// <summary>
        /// Gets or sets the volume conversation coefficient.
        /// </summary>
        public float VC { get; set; }

        /// <summary>
        /// Gets or sets the dynamic friction coefficient.
        /// </summary>
        public float DF { get; set; }

        /// <summary>
        /// Gets or sets the maximum volume ratio for pose.
        /// </summary>
        public float MT { get; set; }

        /// <summary>
        /// Gets or sets the CHR parameter.
        /// </summary>
        public float CHR { get; set; }

        /// <summary>
        /// Gets or sets the KHR parameter.
        /// </summary>
        public float KHR { get; set; }

        /// <summary>
        /// Gets or sets the SHR parameter.
        /// </summary>
        public float SHR { get; set; }

        /// <summary>
        /// Gets or sets the AHR parameter.
        /// </summary>
        public float AHR { get; set; }
    }

    /// <summary>
    /// Represents a container for cluster parameters of a soft body.
    /// </summary>
    public class ClusterParameterContainer
    {
        /// <summary>
        /// Gets or sets the SRHR_CL parameter.
        /// </summary>
        public float SRHR_CL { get; set; }

        /// <summary>
        /// Gets or sets the SKHR_CL parameter.
        /// </summary>
        public float SKHR_CL { get; set; }

        /// <summary>
        /// Gets or sets the SSHR_CL parameter.
        /// </summary>
        public float SSHR_CL { get; set; }

        /// <summary>
        /// Gets or sets the SR_SPLT_CL parameter.
        /// </summary>
        public float SR_SPLT_CL { get; set; }

        /// <summary>
        /// Gets or sets the SK_SPLT_CL parameter.
        /// </summary>
        public float SK_SPLT_CL { get; set; }

        /// <summary>
        /// Gets or sets the SS_SPLT_CL parameter.
        /// </summary>
        public float SS_SPLT_CL { get; set; }
    }

    /// <summary>
    /// Represents a container for iteration parameters of a soft body.
    /// </summary>
    public class IterationParameterContainer
    {
        /// <summary>
        /// Gets or sets the velocity iterations parameter.
        /// </summary>
        public int V_IT { get; set; }

        /// <summary>
        /// Gets or sets the position iterations parameter.
        /// </summary>
        public int P_IT { get; set; }

        /// <summary>
        /// Gets or sets the drift iterations parameter.
        /// </summary>
        public int D_IT { get; set; }

        /// <summary>
        /// Gets or sets the cluster iterations parameter.
        /// </summary>
        public int C_IT { get; set; }
    }

    /// <summary>
    /// Represents a container for material parameters of a soft body.
    /// </summary>
    public class MaterialParameterContainer
    {
        /// <summary>
        /// Gets or sets the linear stiffness coefficient.
        /// </summary>
        public float LST { get; set; }

        /// <summary>
        /// Gets or sets the area/angular stiffness coefficient.
        /// </summary>
        public float AST { get; set; }

        /// <summary>
        /// Gets or sets the volume stiffness coefficient.
        /// </summary>
        public float VST { get; set; }
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} - {Shape}";
}
