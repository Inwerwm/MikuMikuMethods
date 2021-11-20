using MikuMikuMethods.Mme.Element;
using MikuMikuMethods.Mme.IO;
using System.Text.RegularExpressions;

namespace MikuMikuMethods.Mme;

/// <summary>
/// Emdファイルの内部表現
/// </summary>
public class EmdData
{
    /// <summary>
    /// EMMのバージョン
    /// </summary>
    public int Version { get; internal set; }

    /// <summary>
    /// オブジェクトに適用するエフェクトの情報
    /// </summary>
    public EmmMaterial Material { get; init; }

    /// <summary>
    /// サブセット展開されたときの各材質に適用するエフェクトの情報
    /// </summary>
    public List<EmmMaterial> Subsets { get; init; }

    public EmdData()
    {
        Version = 3;
        Material = new();
        Subsets = new();
    }

    public EmdData(EmmMaterial material)
    {
        Version = 3;
        Material = material;
        Subsets = new();
    }

    public EmdData(string filePath) : this()
    {
        Read(filePath);
    }

    public void Read(string filePath)
    {
        MmeFileReader.ReadEmdData(filePath, this);
    }

    /// <summary>
    /// ファイルに書き出し
    /// </summary>
    /// <param name="filePath">書き出すファイルのパス</param>
    public void Write(string filePath)
    {
        MmeFileWriter.WriteEmdData(filePath, this);
    }
}
