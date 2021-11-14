using MikuMikuMethods.Mme.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MikuMikuMethods.Mme;

/// <summary>
/// MMEプロジェクト単位設定ファイル(EMMファイルの内部表現)
/// </summary>
public class EmmData
{
    /// <summary>
    /// EMMのバージョン
    /// </summary>
    public int Version { get; private set; }

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
        using (FileStream stream = new(filePath, FileMode.Open))
        using (StreamReader reader = new(stream, MikuMikuMethods.Encoding.ShiftJIS))
        {
            Read(reader);
        }
    }

    /// <summary>
    /// EMMファイルから読み込み
    /// </summary>
    /// <param name="reader">エンコードはShiftJISである必要がある</param>
    public void Read(StreamReader reader)
    {
        if (reader.CurrentEncoding != Encoding.ShiftJIS)
            throw new FormatException($"EMMファイル読み込みエンコードエラー{Environment.NewLine}エンコーダがShiftJISと違います。");

        string? line;
        EmmEffectSettings effect;

        // [Info]
        line = reader.ReadLine();
        if (line != "[Info]")
            throw new FormatException("読み込まれたファイル形式がEMMファイルと違います");

        // Version
        line = reader.ReadLine();
        if(line is null) throw new InvalidOperationException("Invalid line reading occurred.");
        Version = int.Parse(Regex.Replace(line, @"[^0-9]", ""));
        if (Version < 3) throw new FormatException("EMMファイルのバージョンが未対応の値です");
        // 改行
        reader.ReadLine();

        // [Object]
        line = reader.ReadLine();
        if (line != "[Object]")
            throw new FormatException($"読み込まれたファイル形式がEMMファイルと違います{(line == "[Effect]" ? Environment.NewLine + "EMDファイルを読み込んだ可能性があります" : "")}");
        ReadObjects(reader);

        while (!string.IsNullOrEmpty(line = reader.ReadLine()))
        {
            // Effect
            effect = !line.Contains("@") ? new("Main") : new(line.Split('@')[1].Replace("]", ""));
            effect.Read(reader, Objects);
            EffectSettings.Add(effect);
        }
    }

    private void ReadObjects(StreamReader reader)
    {
        string? line;
        while ((line = reader.ReadLine()) != "")
        {
            var lineData = line?.Split('=').Select(str => str.Trim());
            var objectKey = lineData?.ElementAt(0);
            var path = lineData?.ElementAt(1);

            if (objectKey is null || path is null)
                throw new InvalidOperationException("Invalid line reading occurred.");

            int objectIndex = int.Parse(Regex.Replace(objectKey, @"[^0-9]", ""));
            EmmObject obj = Regex.Replace(objectKey, @"[0-9]", "") switch
            {
                "Pmd" => new EmmModel(objectIndex, path),
                "Acs" => new EmmAccessory(objectIndex, path),
                _ => throw new InvalidOperationException("Invalid line reading occurred.")
            };
            Objects.Add(obj);
        }
    }

    /// <summary>
    /// ファイルに書き出し
    /// </summary>
    /// <param name="filePath">書き出すファイルのパス</param>
    public void Write(string filePath)
    {
        using (FileStream file = new(filePath, FileMode.Create))
        using (StreamWriter writer = new(file, MikuMikuMethods.Encoding.ShiftJIS))
        {
            Write(writer);
        }
    }

    /// <summary>
    /// EMMファイルに書き込み
    /// </summary>
    /// <param name="writer"></param>
    public void Write(StreamWriter writer)
    {
        if (writer.Encoding != Encoding.ShiftJIS)
            throw new FormatException($"EMMファイル書き込みエンコードエラー{Environment.NewLine}エンコーダがShiftJISと違います。");

        // [Info]
        writer.WriteLine("[Info]");
        writer.WriteLine($"Version = {Version}");
        writer.WriteLine("");

        // [Object]
        writer.WriteLine("[Object]");
        foreach (var obj in Objects)
        {
            writer.WriteLine($"{obj.Name} = {obj.Path}");
        }
        writer.WriteLine("");

        // Effect
        foreach (var effect in EffectSettings)
        {
            writer.WriteLine(effect.IsMain ? "[Effect]" : $"[Effect@{effect.Name}]");
            effect.Write(writer);
            writer.WriteLine("");
        }
    }
}
