using MikuMikuMethods.Mme.Element;
using MikuMikuMethods.Mme.IO;

namespace MikuMikuMethods.Mme;

/// <summary>
/// MMEプロジェクト単位設定ファイル(EMMファイルの内部表現)
/// </summary>
public class EmmData
{
    /// <summary>
    /// EMMのバージョン
    /// </summary>
    public int Version { get; internal set; }

    /// <summary>
    /// 内包オブジェクトのリスト
    /// </summary>
    public List<EmmObject> Objects { get; init; }

    /// <summary>
    /// <para>エフェクト設定のリスト</para>
    /// <para>MMEエフェクト割当画面の各タブに相当</para>
    /// </summary>
    public List<EmmEffectSettings> EffectSettings { get; init; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EmmData()
    {
        Version = 3;
        Objects = new();
        EffectSettings = new();
    }

    /// <summary>
    /// ファイル読み込みコンストラクタ
    /// </summary>
    /// <param name="filePath">読み込むファイルのパス</param>
    public EmmData(string filePath) : this()
    {
        Read(filePath);
    }

    /// <summary>
    /// ファイルから読込
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    public void Read(string filePath)
    {
        MmeFileReader.ReadEmmData(filePath, this);
    }

    /// <summary>
    /// ファイルに書き出し
    /// </summary>
    /// <param name="filePath">書き出すファイルのパス</param>
    public void Write(string filePath)
    {
        MmeFileWriter.WriteEmmData(filePath, this);
    }
}
