using MikuMikuMethods.Extension;
using System.Collections.Generic;
using System.IO;

namespace MikuMikuMethods.PMM
{
    /*
     * 魚圭糸工氏の解析結果から
     * https://drive.google.com/file/d/0B6jwWdrYAgJTdXZSd1Noa2hKbmM/view
     */

    /// <summary>
    /// MMDプロジェクトファイル
    /// </summary>
    public class PolygonMovieMaker
    {
        /// <summary>
        /// PMMファイルのバージョン情報
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// 編集画面状態情報
        /// </summary>
        public EditorState EditorState { get; init; }

        /// <summary>
        /// モデル
        /// </summary>
        public List<PmmModel> Models { get; init; }

        /// <summary>
        /// カメラ
        /// </summary>
        public PmmCamera Camera { get; private set; }

        /// <summary>
        /// 照明
        /// </summary>
        public PmmLight Light { get; private set; }

        /// <summary>
        /// アクセサリ
        /// </summary>
        public List<PmmAccessory> Accessories { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PolygonMovieMaker()
        {
            EditorState = new();
            Models = new();
            Camera = new();
            Light = new();
        }

        /// <summary>
        /// バイナリ読込コンストラクタ
        /// </summary>
        /// <param name="reader">PMMファイル</param>
        public PolygonMovieMaker(BinaryReader reader)
        {
            EditorState = new();
            Models = new();
            Read(reader);
        }

        /// <summary>
        /// バイナリデータから読込
        /// </summary>
        /// <param name="reader">読み込むファイル ShiftJISエンコードで読み込むこと</param>
        public void Read(BinaryReader reader)
        {
            Version = reader.ReadString(30, Encoding.ShiftJIS);

            EditorState.ReadViewState(reader);

            var modelCount = reader.ReadByte();
            for (int i = 0; i < modelCount; i++)
                Models.Add(new(reader));

            Camera = new(reader);
            Light = new(reader);

            EditorState.ReadAccessoryState(reader);
            var accessoryCount = reader.ReadInt32();
            // アクセサリ名一覧
            // 名前は各アクセサリ領域にも書いてあり、齟齬が出ることは基本無いらしいので読み飛ばす
            _ = reader.ReadBytes(accessoryCount * 100);
            for (int i = 0; i < accessoryCount; i++)
                Accessories.Add(new(reader));

        }

        /// <summary>
        /// バイナリデータに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Version, 30, Encoding.ShiftJIS);

            EditorState.WriteViewState(writer);

            writer.Write(Models.Count);
            foreach (var model in Models)
                model.Write(writer);

            Camera.Write(writer);
            Light.Write(writer);

            EditorState.WriteAccessoryState(writer);
            writer.Write(Accessories.Count);
            foreach (var acs in Accessories)
                writer.Write(acs.Name, 100, Encoding.ShiftJIS);
            foreach (var acs in Accessories)
                acs.Write(writer);

        }
    }
}
