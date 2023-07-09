using MikuMikuMethods.Mme.Element;
using System.Text.RegularExpressions;

namespace MikuMikuMethods.Mme.IO;

/// <summary>
/// MME ファイル読込
/// </summary>
public static class MmeFileReader
{
    /// <summary>
    /// EMD ファイルを読み込む
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="emd">書込み先 EMD</param>
    public static void ReadEmdData(string filePath, EmdData emd)
    {
        using StreamReader reader = new(filePath, Encoding.ShiftJIS);
        ReadEmdData(reader, emd);
    }

    /// <summary>
    /// EMD ファイルを読み込む
    /// </summary>
    /// <param name="reader">ストリームリーダー</param>
    /// <param name="emd">書込み先 EMD</param>
    public static void ReadEmdData(StreamReader reader, EmdData emd)
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
        emd.Version = int.Parse(Regex.Replace(line, @"[^0-9]", ""));
        if (emd.Version < 3) throw new InvalidDataException("EMDファイルのバージョンが未対応の値です");
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

            // objKeyShow[1]が存在すれば show 設定
            var isShowSetting = objKeyShow.Length > 1;

            // サブセット添字を分離
            var objKeyId = objectKey.Split('[');

            if (objKeyId.Length == 1)
            {
                // サブセット添字が存在しなければオブジェクトに対する設定とみなす

                if (isShowSetting)
                    emd.Material.Show = bool.Parse(data);
                else
                    emd.Material.Path = data;
            }
            else
            {
                // サブセットに対する設定
                var subsetId = int.Parse(objKeyId[1].Replace("]", ""));
                // 設定サブセット添字よりオブジェクトに設定されたエフェクトの数が少ない場合
                // 設定サブセット添字の数まで中身を増やす
                while (emd.Subsets.Count <= subsetId)
                {
                    emd.Subsets.Add(new EmmMaterial());
                }

                if (isShowSetting)
                    emd.Subsets[subsetId].Show = bool.Parse(data);
                else
                    emd.Subsets[subsetId].Path = data;
            }
        }
    }

    /// <summary>
    /// EMM ファイルを読み込む
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <param name="emm">書込み先 EMM</param>

    public static void ReadEmmData(string filePath, EmmData emm)
    {
        using StreamReader reader = new(filePath, Encoding.ShiftJIS);
        ReadEmmData(reader, emm);
    }

    /// <summary>
    /// EMM ファイルを読み込む
    /// </summary>
    /// <param name="reader">ストリームリーダー</param>
    /// <param name="emm">書込み先 EMM</param>

    public static void ReadEmmData(StreamReader reader, EmmData emm)
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
        if (line is null) throw new InvalidOperationException("Invalid line reading occurred.");
        emm.Version = int.Parse(Regex.Replace(line, @"[^0-9]", ""));
        if (emm.Version < 3) throw new FormatException("EMMファイルのバージョンが未対応の値です");
        // 改行
        reader.ReadLine();

        // [Object]
        line = reader.ReadLine();
        if (line != "[Object]")
            throw new FormatException($"読み込まれたファイル形式がEMMファイルと違います{(line == "[Effect]" ? Environment.NewLine + "EMDファイルを読み込んだ可能性があります" : "")}");
        ReadObjects(reader, emm);

        while (!string.IsNullOrEmpty(line = reader.ReadLine()))
        {
            // Effect
            effect = !line.Contains('@') ? new("Main") : new(line.Split('@')[1].Replace("]", ""));
            ReadEffectSettings(reader, effect, emm.Objects);
            emm.EffectSettings.Add(effect);
        }
    }

    private static void ReadObjects(StreamReader reader, EmmData emm)
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
            emm.Objects.Add(obj);
        }
    }

    /// <summary>
    /// 読み込み
    /// </summary>
    /// <param name="reader">[.*]の次行が読み込まれる状態であること</param>
    /// <param name="effect">書込み先設定情報</param>
    /// <param name="objects">オブジェクトのリスト</param>
    internal static void ReadEffectSettings(StreamReader reader, EmmEffectSettings effect, List<EmmObject> objects)
    {
        string? line;
        while (!string.IsNullOrEmpty(line = reader.ReadLine()))
        {
            var lineData = line.Split('=', StringSplitOptions.TrimEntries);
            var objectKey = lineData[0];
            var data = lineData[1];

            if (objectKey == "Owner")
            {
                effect.Owner = data.Contains('@') ?
                    new EmmObjectReference(objects.First(o => o.Name == data.Split('@')[0]), data.Split('@')[1]) :
                    objects.First(o => o.Name == data);
                continue;
            }
            if (objectKey == "Default")
            {
                effect.Default = new() { Path = data };
                continue;
            }

            // .showを分離
            var objKeyShow = objectKey.Split('.');
            objectKey = objKeyShow[0];

            // objKeyShow[1]が存在すれば show 設定
            var isShowSetting = objKeyShow.Length > 1;

            // サブセット添字を分離
            var objKeyId = objectKey.Split('[');
            objectKey = objKeyId[0];

            // サブセット添字が存在しなければオブジェクトに対する設定とみなす
            if (objKeyId.Length == 1)
            {
                var eo = effect.ObjectSettings.FirstOrDefault(o => o.Object.Name == objectKey)
                   ?? new(objects.First(info => info.Name == objectKey));

                if (isShowSetting)
                    eo.Material.Show = bool.Parse(data);
                else
                    eo.Material.Path = data;

                if (!effect.ObjectSettings.Contains(eo))
                    effect.ObjectSettings.Add(eo);
            }
            else
            {
                // サブセットに対する設定
                var eo = effect.ObjectSettings.First(o => o.Object.Name == objectKey);
                var subsetId = int.Parse(objKeyId[1].Replace("]", ""));

                // 設定サブセット添字よりオブジェクトに設定されたエフェクトの数が少ない場合
                // 設定サブセット添字の数まで中身を増やす
                while (eo.Subsets.Count <= subsetId)
                {
                    eo.Subsets.Add(new EmmMaterial());
                }

                if (isShowSetting)
                    eo.Subsets[subsetId].Show = bool.Parse(data);
                else
                    eo.Subsets[subsetId].Path = data;
            }
        }
    }
}
