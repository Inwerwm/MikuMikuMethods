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
        public PmmEditorState EditorState { get; init; }
        /// <summary>
        /// 再生設定
        /// </summary>
        public PmmPlayConfig PlayConfig { get; init; }
        /// <summary>
        /// メディア設定
        /// </summary>
        public PmmMediaConfig MediaConfig { get; init; }
        /// <summary>
        /// 描画設定
        /// </summary>
        public PmmDrawConfig DrawConfig { get; init; }

        /// <summary>
        /// モデル
        /// </summary>
        public List<PmmModel> Models { get; init; }
        /// <summary>
        /// アクセサリ
        /// </summary>
        public List<PmmAccessory> Accessories { get; init; }

        /// <summary>
        /// カメラ
        /// </summary>
        public PmmCamera Camera { get; private set; }
        /// <summary>
        /// 照明
        /// </summary>
        public PmmLight Light { get; private set; }
        /// <summary>
        /// 重力
        /// </summary>
        public PmmGravity Gravity { get; private set; }
        /// <summary>
        /// セルフ影
        /// </summary>
        public PmmSelfShadow SelfShadow { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PolygonMovieMaker()
        {
            EditorState = new();
            PlayConfig = new();
            MediaConfig = new();
            DrawConfig = new();

            Models = new();
            Accessories = new();

            Camera = new();
            Light = new();
            Gravity = new();
            SelfShadow = new();
        }

        /// <summary>
        /// バイナリ読込コンストラクタ
        /// </summary>
        /// <param name="reader">PMMファイル</param>
        public PolygonMovieMaker(BinaryReader reader) : this()
        {
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

            EditorState.ReadFrameState(reader);
            PlayConfig.Read(reader);
            MediaConfig.Read(reader);
            DrawConfig.Read(reader);
            Gravity.Read(reader);
            SelfShadow.Read(reader);
            DrawConfig.ReadColorConfig(reader);
            Camera.ReadUncomittedFollowingState(reader);
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

            EditorState.WriteFrameState(writer);
            PlayConfig.Write(writer);
            MediaConfig.Write(writer);
            DrawConfig.Write(writer);
            Gravity.Write(writer);
            SelfShadow.Write(writer);
            DrawConfig.WriteColorConfig(writer);
            Camera.WriteUncomittedFollowingState(writer);
        }
    }
}
