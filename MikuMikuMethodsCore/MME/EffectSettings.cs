using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.MME
{
    /// <summary>
    /// <para>EMMファイルのエフェクト単位の設定</para>
    /// <para>MMEエフェクト割当画面の各タブに相当</para>
    /// </summary>
    public class EffectSettings
    {
        /// <summary>
        /// 設定種別
        /// </summary>
        public EffectCategory Category { get; init; }

        /// <summary>
        /// <para>設定名</para>
        /// <para>MMEエフェクト割当画面のタブ名に相当</para>
        /// </summary>
        public string Name { get; init; }
        /// <summary>
        /// <para>エフェクトが依拠するオブジェクト</para>
        /// <para>オブジェクト定義で記述された名前が入る</para>
        /// <para>CategoryがEffectの場合はEMMファイル内の"Default"に相当する</para>
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 設定対象オブジェクトのリスト
        /// </summary>
        public List<EffectOnObject> Effects { get; init; }

        private List<ObjectInfo> Objects { get; init; }

        private EffectSettings(List<ObjectInfo> objects)
        {
            Effects = new();
            Objects = objects;
        }

        /// <summary>
        /// [Object]または[Effect]のとき用
        /// </summary>
        /// <param name="keys">設定するオブジェクト定義リスト</param>
        /// <param name="category">設定種別</param>
        public EffectSettings(List<ObjectInfo> keys, EffectCategory category) : this(keys)
        {
            Category = category;
            Name = Category.ToString();
        }

        /// <summary>
        /// [Effect@.*]のとき用
        /// </summary>
        /// <param name="keys">設定するオブジェクト定義リスト</param>
        /// <param name="name">"@"以降の名前</param>
        public EffectSettings(List<ObjectInfo> keys, string name) : this(keys)
        {
            Category = EffectCategory.Other;
            Name = name;
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <param name="reader">[.*]の次行が読み込まれる状態であること</param>
        internal void Read(StreamReader reader)
        {
            string line;
            while(!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                var lineData = line.Split('=', StringSplitOptions.TrimEntries);
                var objectKey = lineData[0];
                var data = lineData[1];

                if(objectKey is "Default" or "Owner")
                {
                    Owner = data;
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

                EffectOnObject eo;

                // サブセット添字が存在しなければオブジェクトに対する設定とみなす
                if (objKeyId.Length == 1)
                {
                    eo = Effects.FirstOrDefault(o => o.Object.Name == objectKey)
                       ?? new(Objects.First(info => info.Name == objectKey));
                    if (isShowSetting)
                        eo.Effect.Show = bool.Parse(data);
                    else
                        eo.Effect.Path = data;

                    if (!Effects.Contains(eo))
                        Effects.Add(eo);
                    continue;
                }

                // サブセットに対する設定
                eo = Effects.First(o => o.Object.Name == objectKey);
                var subsetId = int.Parse(objKeyId[1].Replace("]", ""));
                // 設定サブセット添字よりオブジェクトに設定されたエフェクトの数が少ない場合
                // 設定サブセット添字の数まで中身を増やす
                while(eo.Subsets.Count <= subsetId)
                {
                    eo.Subsets.Add(new FxInfo());
                }

                if (isShowSetting)
                    eo.Subsets[subsetId].Show = bool.Parse(data);
                else
                    eo.Subsets[subsetId].Path = data;


            }
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="writer">書き込みストリーム</param>
        internal void Write(StreamWriter writer)
        {
            var ownerString = Category switch
            {
                EffectCategory.Effect => "Default",
                EffectCategory.Other => "Owner",
                _ => throw new NotImplementedException(),
            };
            writer.WriteLine($"{ownerString} = {Owner}");

            foreach (var obj in Effects)
            {
                if (!string.IsNullOrEmpty(obj.Effect.Path))
                    writer.WriteLine($"{obj.Object.Name} = {obj.Effect.Path}");
                if (obj.Effect.Show != null)
                    writer.WriteLine($"{obj.Object.Name}.show = {obj.Effect.Show.Value.ToString().ToLower()}");

                foreach (var sub in obj.Subsets.Select((effect,i)=> (effect, i)))
                {
                    if (!string.IsNullOrEmpty(sub.effect.Path))
                        writer.WriteLine($"{obj.Object.Name}[{sub.i}] = {sub.effect.Path}");
                    if (sub.effect.Show != null)
                        writer.WriteLine($"{obj.Object.Name}[{sub.i}].show = {sub.effect.Show.Value.ToString().ToLower()}");
                }
            }
        }
    }
}
