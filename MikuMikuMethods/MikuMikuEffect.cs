using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MikuMikuMethods.MME
{
    public class EMMData
    {
        public int Version { get; private set; }
        public List<EMMEffectType> Effects { get; set; }

        public EMMData()
        {
            Version = 3;
            Effects = new List<EMMEffectType>();
        }

        public EMMData(StreamReader reader)
        {
            Effects = new List<EMMEffectType>();
            Read(reader);
        }

        /// <param name="reader">エンコードはシフトJISに設定すること</param>
        public void Read(StreamReader reader)
        {
            if (reader.CurrentEncoding != Encoding.GetEncoding("shift_jis"))
                throw new ArgumentException("EMMData Read エンコードエラー" + Environment.NewLine + "StreamReaderのエンコードをシフトJISに設定してください");

            if (reader.ReadLine() != "[Info]")
                throw new FormatException("読み込まれたファイル形式がEMMファイルと違います");

            string str;

            // Version
            str = reader.ReadLine();
            // strから数値のみ抽出してVersionに代入
            Version = int.Parse(Regex.Replace(str, @"[^0-9]", ""));
            //改行
            reader.ReadLine();

            // [Object]
            reader.ReadLine();
            // 内容
            Effects.Add(new EMMEffectType("Object", reader));

            // [Effect]
            reader.ReadLine();
            // 内容
            Effects.Add(new EMMEffectType("", reader));

            while ((str = reader.ReadLine()) != null)
            {
                // [Effect@tabName]
                var tabName = str.Split('@')[1].Replace("]", "");
                // 内容
                Effects.Add(new EMMEffectType(tabName, reader));
            }

            // IsModelの整合性を確保
            for (int i = 1; i < Effects.Count; i++)
            {
                for (int j = 0; j < Effects[i].ObjectSettings.Count; j++)
                {
                    Effects[i].ObjectSettings[j].IsModel = Effects[0].ObjectSettings[j].IsModel;
                }
            }
        }

        /// <param name="writer">エンコードはシフトJISに設定すること</param>
        public void Write(StreamWriter writer)
        {
            if (writer.Encoding != Encoding.GetEncoding("shift_jis"))
                throw new ArgumentException("EMMData Write エンコードエラー" + Environment.NewLine + "StreamWriterのエンコードをシフトJISに設定してください");

            writer.WriteLine("[Info]");
            writer.WriteLine(GetValueString("Version", Version.ToString()));
            writer.WriteLine("");

            foreach (var item in Effects)
            {
                item.Write(writer);
                writer.WriteLine("");
            }
        }

        private string GetValueString(string name, string value)
        {
            return name + " = " + value;
        }
    }

    public class EMMEffectType
    {
        /// <summary>
        /// "" => [Effect], "Object" => [Object], "???" => [Effect@???]
        /// </summary>
        public string Name { get; set; }
        public string Owner { get; set; }
        public int Count { get { return ObjectSettings.Count; } }
        public List<EMMObjectSettings> ObjectSettings { get; set; }

        /// <param name="name">"" => [Effect], "Object" => [Object], "???" => [Effect@???]</param>
        public EMMEffectType(string name = "", string owner = "none")
        {
            Name = name;
            Owner = owner;
            ObjectSettings = new List<EMMObjectSettings>();
        }

        /// <param name="name">"" => [Effect], "Object" => [Object], "???" => [Effect@???]</param>
        public EMMEffectType(string name, StreamReader reader)
        {
            Name = name;
            ObjectSettings = new List<EMMObjectSettings>();
            Read(reader);
        }

        public void Read(StreamReader reader)
        {
            if (reader.CurrentEncoding != Encoding.GetEncoding("shift_jis"))
                throw new ArgumentException("EMMData Read エンコードエラー" + Environment.NewLine + "StreamReaderのエンコードをシフトJISに設定してください");

            string[] str;

            while (true)
            {
                var line = reader.ReadLine();
                //読み込んだ行が改行文字のみであった場合ループから抜ける
                if (line == "")
                    break;
                // {[0]：Typ?, [1]：Value}
                str = line.Split('=');
                //値クラスタ最初の空白文字を削除
                str[1] = str[1].Remove(0, 1);

                // 種類クラスタの文字で種類を判断
                if (str[0][0] == 'D' || str[0][1] == 'w')
                    Owner = str[1];
                else if (str[0].Contains("["))
                {
                    //サブセット
                    //str[0]を種類IDとサブセット添字に分割
                    //{[0]：ObjID, [1]：SubID}
                    var numji = str[0].Split('[');
                    int objID = int.Parse(Regex.Replace(numji[0], @"[^0-9]", "")) - 1;
                    int subID = int.Parse(Regex.Replace(numji[1], @"[^0-9]", ""));

                    while (ObjectSettings[objID].SubsetSettings.Count <= subID)
                    {
                        ObjectSettings[objID].SubsetSettings.Add(new EMMEffectSetting());
                    }

                    // 種類クラスタが"."を含むならShow設定
                    if (str[0].Contains("."))
                        ObjectSettings[objID].SubsetSettings[subID].Show = bool.Parse(str[1]);
                    else
                        ObjectSettings[objID].SubsetSettings[subID].Path = str[1];
                }
                else
                {
                    //オブジェクト
                    var i = int.Parse(Regex.Replace(str[0], @"[^0-9]", "")) - 1;
                    while (ObjectSettings.Count <= i)
                    {
                        var s = new EMMObjectSettings(str[0][0] == 'P');
                        s.IsObj = str[0][0] == 'O';
                        ObjectSettings.Add(s);
                    }

                    // 種類クラスタが"."を含むならShow設定
                    if (str[0].Contains("."))
                        ObjectSettings[i].EffectSetting.Show = bool.Parse(str[1]);
                    // そうでなければパス
                    else
                        ObjectSettings[i].EffectSetting.Path = str[1];
                }
            }
        }

        public void Write(StreamWriter writer)
        {
            string tabName = Name == "" ? "Effect"
                           : Name == "Object" ? "Object"
                           : "Effect@" + Name;
            writer.WriteLine("[" + tabName + "]");

            if (Name != "Object")
            {
                if (Name == "")
                    writer.WriteLine(GetValueString("Default", Owner));
                else
                    writer.WriteLine(GetValueString("Owner", Owner));
            }

            // オブジェクト書込ループ
            for (int i = 0; i < Count; i++)
            {
                // モデルかアクセか
                var left = ObjectSettings[i].IsModel ? "Pmd"
                         : ObjectSettings[i].IsObj ? "Obj"
                         : "Acs";
                left += i + 1;

                // パス
                if (ObjectSettings[i].EffectSetting.ExistPath)
                    writer.WriteLine(GetValueString(left, ObjectSettings[i].EffectSetting.Path));

                // [Object] 以外の場合
                if (Name != "Object")
                {
                    // show設定
                    if (ObjectSettings[i].EffectSetting.ExistShow)
                        writer.WriteLine(GetValueString(left + ".show", ObjectSettings[i].EffectSetting.Show));

                    // サブセット展開されている場合
                    if (ObjectSettings[i].SubsetSettings.Count > 0)
                    {
                        for (int j = 0; j < ObjectSettings[i].SubsetSettings.Count; j++)
                        {
                            if (ObjectSettings[i].SubsetSettings[j].ExistPath)
                                writer.WriteLine(GetValueString(left + "[" + j + "]", ObjectSettings[i].SubsetSettings[j].Path));
                            if (ObjectSettings[i].SubsetSettings[j].ExistShow)
                                writer.WriteLine(GetValueString(left + "[" + j + "].show", ObjectSettings[i].SubsetSettings[j].Show));
                        }
                    }
                }
            }
        }

        private string GetValueString(string name, string value)
        {
            return name + " = " + value;
        }

        private string GetValueString(string name, bool value)
        {
            return GetValueString(name, value.ToString().ToLower());
        }
    }

    public class EMMObjectSettings
    {
        public bool IsModel { get; set; }
        public bool IsObj { get; set; }
        public EMMEffectSetting EffectSetting { get; set; }
        public int Count { get { return SubsetSettings.Count; } }
        public List<EMMEffectSetting> SubsetSettings { get; set; }

        public EMMObjectSettings(bool isModel, string path, bool show)
        {
            IsModel = isModel;
            IsObj = false;
            EffectSetting = new EMMEffectSetting(path, show);
            SubsetSettings = new List<EMMEffectSetting>();
        }
        public EMMObjectSettings(bool isModel, string path)
        {
            IsModel = isModel;
            IsObj = false;
            EffectSetting = new EMMEffectSetting(path);
            SubsetSettings = new List<EMMEffectSetting>();
        }
        public EMMObjectSettings(bool isModel, bool show)
        {
            IsModel = isModel;
            IsObj = false;
            EffectSetting = new EMMEffectSetting(show);
            SubsetSettings = new List<EMMEffectSetting>();
        }
        public EMMObjectSettings(bool isModel)
        {
            IsModel = isModel;
            IsObj = false;
            EffectSetting = new EMMEffectSetting();
            SubsetSettings = new List<EMMEffectSetting>();
        }
    }

    public class EMMEffectSetting
    {
        bool show;
        public bool Show
        {
            get
            {
                if (!ExistShow)
                    throw new InvalidOperationException("Showが存在しないEMMEffectSettingsからShowが呼び出されました");
                else
                    return show;
            }
            set
            {
                show = value;
                ExistShow = true;
            }
        }
        public bool ExistShow { get; private set; }

        string path;
        public string Path
        {
            get
            {
                if (!ExistPath)
                    throw new InvalidOperationException("Pathが存在しないEMMEffectSettingsからPathが呼び出されました");
                else
                    return path;
            }
            set
            {
                path = value;
                ExistPath = true;
            }
        }
        public bool ExistPath { get; private set; }

        public EMMEffectSetting()
        {
            ExistShow = false;
            ExistPath = false;
        }
        public EMMEffectSetting(string path, bool show)
        {
            ExistShow = true;
            ExistPath = true;
            Show = show;
            Path = path;
        }
        public EMMEffectSetting(string path)
        {
            ExistShow = false;
            ExistPath = true;
            Path = path;
        }
        public EMMEffectSetting(bool show)
        {
            ExistShow = true;
            ExistPath = false;
            Show = show;
        }

        public void ShowUnset()
        {
            ExistShow = false;
        }
        public void PathUnset()
        {
            ExistPath = false;
        }
    }
}