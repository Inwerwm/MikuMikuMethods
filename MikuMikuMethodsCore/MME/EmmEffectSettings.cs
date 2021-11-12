using MikuMikuMethods.Mme.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MikuMikuMethods.Mme;

/// <summary>
/// <para>EMMファイルのエフェクト単位の設定</para>
/// <para>MMEエフェクト割当画面の各タブに相当</para>
/// </summary>
public class EmmEffectSettings
{
    /// <summary>
    /// 設定種別
    /// </summary>
    public bool IsMain { get; init; }

    /// <summary>
    /// <para>設定名</para>
    /// <para>MMEエフェクト割当画面のタブ名に相当</para>
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Mainタブの(default)に設定されたエフェクト
    /// </summary>
    public EmmMaterial? Default { get; set; }
    /// <summary>
    /// <para>エフェクトが依拠するオブジェクト</para>
    /// <para>オブジェクト定義で記述された名前が入る</para>
    /// </summary>
    public EmmObject? Owner { get; set; }

    /// <summary>
    /// 設定対象オブジェクトのリスト
    /// </summary>
    public List<EmmObjectSetting> ObjectSettings { get; init; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">エフェクト設定のタブ名 "Main"でメインタブ扱いになる</param>
    public EmmEffectSettings(string name)
    {
        ObjectSettings = new();
        IsMain = name == "Main";
        Name = name;
    }

    /// <summary>
    /// 読み込み
    /// </summary>
    /// <param name="reader">[.*]の次行が読み込まれる状態であること</param>
    internal void Read(StreamReader reader, List<EmmObject> objects)
    {
        string line;
        while (!string.IsNullOrEmpty(line = reader.ReadLine()))
        {
            var lineData = line.Split('=', StringSplitOptions.TrimEntries);
            var objectKey = lineData[0];
            var data = lineData[1];

            if (objectKey == "Owner")
            {
                Owner = objects.FirstOrDefault(o => o.Name == data);
                continue;
            }
            if (objectKey == "Default")
            {
                Default = new() { Path = data };
                continue;
            }

            // .showを分離
            var objKeyShow = objectKey.Split('.');
            objectKey = objKeyShow[0];

            // objKeyShow[1]が存在すればshow設定
            var isShowSetting = objKeyShow.Length > 1;

            // サブセット添字を分離
            var objKeyId = objectKey.Split('[');
            objectKey = objKeyId[0];

            // サブセット添字が存在しなければオブジェクトに対する設定とみなす
            if (objKeyId.Length == 1)
            {
                var eo = ObjectSettings.FirstOrDefault(o => o.Object.Name == objectKey)
                   ?? new(objects.First(info => info.Name == objectKey));

                if (isShowSetting)
                    eo.Material.Show = bool.Parse(data);
                else
                    eo.Material.Path = data;

                if (!ObjectSettings.Contains(eo))
                    ObjectSettings.Add(eo);
            }
            else
            {
                // サブセットに対する設定
                var eo = ObjectSettings.First(o => o.Object.Name == objectKey);
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

    /// <summary>
    /// 書き込み
    /// </summary>
    /// <param name="writer">書き込みストリーム</param>
    internal void Write(StreamWriter writer)
    {
        // Main なら Default に、そうでなければ Owner には必ず値が入っている
        writer.WriteLine(IsMain ? $"Default = {Default!.Path}" : $"Owner = {Owner!.Name}");

        foreach (var obj in ObjectSettings)
        {
            if (!string.IsNullOrEmpty(obj.Material.Path))
                writer.WriteLine($"{obj.Object.Name} = {obj.Material.Path}");

            if (obj.Material.Show != null)
                writer.WriteLine($"{obj.Object.Name}.show = {obj.Material.Show.Value.ToString().ToLower()}");

            foreach (var sub in obj.Subsets.Select((effect, i) => (effect, i)))
            {
                if (!string.IsNullOrEmpty(sub.effect.Path))
                    writer.WriteLine($"{obj.Object.Name}[{sub.i}] = {sub.effect.Path}");
                if (sub.effect.Show != null)
                    writer.WriteLine($"{obj.Object.Name}[{sub.i}].show = {sub.effect.Show.Value.ToString().ToLower()}");
            }
        }
    }
}
