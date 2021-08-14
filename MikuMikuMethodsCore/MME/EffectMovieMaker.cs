using MikuMikuMethods.MME.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MikuMikuMethods.MME
{
    /// <summary>
    /// MMEプロジェクト単位設定ファイル(EMMファイルの内部表現)
    /// </summary>
    public class EffectMovieMaker
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
        public EffectMovieMaker()
        {
            Version = 3;
            Objects = new();
            EffectSettings = new();
        }

        /// <summary>
        /// ファイルから読込
        /// </summary>
        /// <param name="path">ファイルパス</param>
        public void Read(string path)
        {
            using (FileStream stream = new(path, FileMode.Open))
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

            string line;
            EmmEffectSettings effect;

            // [Info]
            line = reader.ReadLine();
            if (line != "[Info]")
                throw new FormatException("読み込まれたファイル形式がEMMファイルと違います");

            // Version
            line = reader.ReadLine();
            Version = int.Parse(Regex.Replace(line, @"[^0-9]", ""));
            // 改行
            reader.ReadLine();

            // [Object]
            reader.ReadLine();
            ReadObjects(reader);

            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                // Effect
                effect = !line.Contains("@") ? new(EmmEffectCategory.Effect) : new(line.Split('@')[1].Replace("]", ""));
                effect.Read(reader, Objects);
                EffectSettings.Add(effect);
            }
        }

        private void ReadObjects(StreamReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != "")
            {
                var lineData = line.Split('=').Select(str => str.Trim());
                var objectKey = lineData.ElementAt(0);
                var path = lineData.ElementAt(1);

                int objectIndex = int.Parse(Regex.Replace(objectKey, @"[^0-9]", ""));
                EmmObject obj = Regex.Replace(objectKey, @"[0-9]", "") switch
                {
                    "Pmd" => new EmmModel(objectIndex),
                    "Acs" => new EmmAccessory(objectIndex),
                    "Obj" => new EmdObject(objectIndex),
                    _ => throw new InvalidOperationException("EMMオブジェクト読み込みで不正なオブジェクト読み込みがなされました。")
                };

                obj.Path = path;
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
                string tabName = effect.Category switch
                {
                    EmmEffectCategory.Effect => "[Effect]",
                    EmmEffectCategory.Other => $"[Effect@{effect.Name}]",
                    _ => throw new NotImplementedException(),
                };
                writer.WriteLine(tabName);
                effect.Write(writer);
                writer.WriteLine("");
            }
        }
    }
}
