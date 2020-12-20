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
    public class ProjectEffectSettings
    {
        /// <summary>
        /// EMMのバージョン
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// 内包オブジェクトのリスト
        /// </summary>
        public List<ObjectInfo> Objects { get; init; }

        /// <summary>
        /// <para>エフェクト設定のリスト</para>
        /// <para>MMEエフェクト割当画面の各タブに相当</para>
        /// </summary>
        public List<EffectSettings> Effects { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProjectEffectSettings()
        {
            Version = 3;
            Objects = new();
            Effects = new();
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
            EffectSettings effect;

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
                effect = !line.Contains("@") ? new(Objects, EffectCategory.Effect) : new(Objects, line.Split('@')[1].Replace("]", ""));
                effect.Read(reader);
                Effects.Add(effect);
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

                var type = Regex.Replace(objectKey, @"[0-9]", "") switch
                {
                    "Pmd" => ObjectType.Model,
                    "Acs" => ObjectType.Accessory,
                    "Obj" => ObjectType.Object,
                    _ => throw new InvalidOperationException("EMMオブジェクト読み込みで不正なオブジェクト読み込みがなされました。")
                };

                Objects.Add(new(type, int.Parse(Regex.Replace(objectKey, @"[^0-9]", ""))));
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
            foreach (var effect in Effects)
            {
                string tabName = effect.Category switch
                {
                    EffectCategory.Effect => "[Effect]",
                    EffectCategory.Other => $"[Effect@{effect.Name}]",
                    _ => throw new NotImplementedException(),
                };
                writer.WriteLine(tabName);
                effect.Write(writer);
                writer.WriteLine("");
            }
        }
    }
}
