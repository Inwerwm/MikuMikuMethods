using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// PMXモデル
/// </summary>
public class PmxModel : IPmxData
{
    /// <summary>
    /// ヘッダ
    /// </summary>
    public PmxHeader Header { get; }
    /// <summary>
    /// モデル情報
    /// </summary>
    public PmxModelInfo ModelInfo { get; }
    /// <summary>
    /// 剛体
    /// </summary>
    public List<PmxBody> Bodies { get; }
    /// <summary>
    /// ボーン
    /// </summary>
    public List<PmxBone> Bones { get; }
    /// <summary>
    /// <para>面の一覧</para>
    /// <para>面の追加は材質の面プロパティから行う</para>
    /// </summary>
    public ReadOnlyCollection<PmxFace> Faces => Materials.SelectMany(m => m.Faces).ToList().AsReadOnly();
    /// <summary>
    /// ジョイント
    /// </summary>
    public List<PmxJoint> Joints { get; }
    /// <summary>
    /// 材質
    /// </summary>
    public List<PmxMaterial> Materials { get; }
    /// <summary>
    /// モーフ
    /// </summary>
    public List<PmxMorph> Morphs { get; }
    /// <summary>
    /// 表情枠
    /// </summary>
    public List<PmxNode> Nodes { get; }
    /// <summary>
    /// 頂点
    /// </summary>
    public List<PmxVertex> Vertices { get; }
    /// <summary>
    /// ソフトボディ
    /// </summary>
    public List<PmxSoftBody> SoftBodies { get; }

    /// <summary>
    /// 空のモデルを生成するコンストラクタ
    /// </summary>
    public PmxModel()
    {
        Header = new();
        ModelInfo = new();
        Bodies = new();
        Bones = new();
        Joints = new();
        Materials = new();
        Morphs = new();
        Nodes = new();
        Vertices = new();
        SoftBodies = new();
    }

    /// <summary>
    /// ファイルからモデルを読み込む
    /// </summary>
    /// <param name="filePath">読み込むファイルパス</param>
    public PmxModel(string filePath) : this() => Read(filePath);

    /// <summary>
    /// ファイルからモデルを読み込む
    /// </summary>
    /// <param name="filePath">読み込むファイルパス</param>
    public void Read(string filePath) => IO.PmxFileReader.ReadModel(filePath, this);

    /// <summary>
    /// ファイルにモデルを書き込む
    /// </summary>
    /// <param name="filePath">書き込むファイルパス</param>
    public void Write(string filePath) => IO.PmxFileWriter.WriteModel(filePath, this);

    /// <summary>
    /// このモデルのバージョン番号を最小化する
    /// </summary>
    public void ValidateVersion()
    {
        Header.Version = SoftBodies.Any() ||
                         Materials.Any(m => m.EnableVertexColor || (m.Primitive == PmxMaterial.PrimitiveType.Point) || (m.Primitive == PmxMaterial.PrimitiveType.Line)) ||
                         Joints.Any(j => j.Type != PmxJoint.JointType.SixDofWithSpring) ||
                         Morphs.Any(m => (m.Type == PmxMorph.MorphType.Flip) || (m.Type == PmxMorph.MorphType.Impulse)) ||
                         Vertices.Any(v => v.WeightType == PmxWeightType.QDEF)
                       ? 2.1f : 2.0f;
    }

    public override string ToString() => $"{Header} : {ModelInfo}";
}
