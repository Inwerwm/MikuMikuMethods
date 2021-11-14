using MikuMikuMethods.Mme.Element;
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
    public int Version { get; private set; }

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
        using StreamReader reader = new(filePath, Encoding.ShiftJIS);
        Read(reader);
    }

    /// <summary>
    /// EMMファイルから読み込み
    /// </summary>
    /// <param name="reader">エンコードはShiftJISである必要がある</param>
    public void Read(StreamReader reader)
    {
        if (reader.CurrentEncoding != Encoding.ShiftJIS)
            throw new ArgumentException($"EMMファイル読み込みエンコードエラー{Environment.NewLine}エンコーダがShiftJISと違います。");

        string? line;

        // [Info]
        line = reader.ReadLine();
        if (line != "[Info]")
            throw new InvalidDataException("読み込まれたファイル形式がEMDファイルと違います");

        // Version
        line = reader.ReadLine();
        if (line is null) throw new InvalidOperationException("Invalid line reading occurred.");
        Version = int.Parse(Regex.Replace(line, @"[^0-9]", ""));
        if (Version < 3) throw new InvalidDataException("EMDファイルのバージョンが未対応の値です");
        // 改行
        reader.ReadLine();

        // [Effect]
        line = reader.ReadLine();
        if (line != "[Effect]")
            throw new InvalidDataException($"読み込まれたファイル形式がEMDファイルと違います{(line == "[Object]" ? Environment.NewLine + "EMMファイルを読み込んだ可能性があります" : "")}");

        while (!string.IsNullOrEmpty(line = reader.ReadLine()))
        {
            var lineData = line.Split('=', StringSplitOptions.TrimEntries);
            var objectKey = lineData[0];
            var data = lineData[1];

            // .showを分離
            var objKeyShow = objectKey.Split('.');
            objectKey = objKeyShow[0];

            // objKeyShow[1]が存在すればshow設定
            var isShowSetting = objKeyShow.Length > 1;

            // サブセット添字を分離
            var objKeyId = objectKey.Split('[');

            if (objKeyId.Length == 1)
            {
                // サブセット添字が存在しなければオブジェクトに対する設定とみなす

                if (isShowSetting)
                    Material.Show = bool.Parse(data);
                else
                    Material.Path = data;
            }
            else
            {
                // サブセットに対する設定
                var subsetId = int.Parse(objKeyId[1].Replace("]", ""));
                // 設定サブセット添字よりオブジェクトに設定されたエフェクトの数が少ない場合
                // 設定サブセット添字の数まで中身を増やす
                while (Subsets.Count <= subsetId)
                {
                    Subsets.Add(new EmmMaterial());
                }

                if (isShowSetting)
                    Subsets[subsetId].Show = bool.Parse(data);
                else
                    Subsets[subsetId].Path = data;
            }
        }
    }

    /// <summary>
    /// ファイルに書き出し
    /// </summary>
    /// <param name="filePath">書き出すファイルのパス</param>
    public void Write(string filePath)
    {
        using FileStream file = new(filePath, FileMode.Create);
        using StreamWriter writer = new(file, Encoding.ShiftJIS);
        Write(writer);
    }

    /// <summary>
    /// EMDファイルに書き込み
    /// </summary>
    /// <param name="writer">エンコードはShiftJISである必要がある</param>
    public void Write(StreamWriter writer)
    {
        if (writer.Encoding != Encoding.ShiftJIS)
            throw new ArgumentException($"EMMファイル書き込みエンコードエラー{Environment.NewLine}エンコーダがShiftJISと違います。");

        // [Info]
        writer.WriteLine("[Info]");
        writer.WriteLine($"Version = {Version}");
        writer.WriteLine();

        // [Object]
        writer.WriteLine("[Effect]");

        if (!string.IsNullOrEmpty(Material.Path))
            writer.WriteLine($"Obj = {Material.Path}");
        if (Material.Show != null)
            writer.WriteLine($"Obj.show = {Material.Show.Value.ToString().ToLower()}");

        foreach (var sub in Subsets.Select((effect, i) => (effect, i)))
        {
            if (!string.IsNullOrEmpty(sub.effect.Path))
                writer.WriteLine($"Obj[{sub.i}] = {sub.effect.Path}");
            if (sub.effect.Show != null)
                writer.WriteLine($"Obj[{sub.i}].show = {sub.effect.Show.Value.ToString().ToLower()}");
        }
        writer.WriteLine();
    }
}
