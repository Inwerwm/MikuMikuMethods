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
        public string Version { get; internal set; }

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
        /// 謎の値
        /// </summary>
        public PmmUnknown Unknown { get; set; }

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
        /// ファイル読込コンストラクタ
        /// </summary>
        /// <param name="filePath">Pmmファイルのパス</param>
        public PolygonMovieMaker(string filePath) : this()
        {
            Read(filePath);
        }

        /// <summary>
        /// ファイルから読込
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        public void Read(string filePath)
        {
            IO.PmmFileReader.Read(filePath, this);
        }

        /// <summary>
        /// ファイルに書き出し
        /// </summary>
        /// <param name="filePath">書き出すファイルのパス</param>
        public void Write(string filePath)
        {
            using (FileStream file = new(filePath, FileMode.Create))
            using (BinaryWriter writer = new(file, MikuMikuMethods.Encoding.ShiftJIS))
            {
                Write(writer);
            }
        }

        /// <summary>
        /// バイナリデータに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Version, 30, Encoding.ShiftJIS);

            EditorState.WriteViewState(writer);

            writer.Write((byte)Models.Count);
            foreach (var model in Models)
                model.Write(writer);

            Camera.Write(writer);
            Light.Write(writer);

            EditorState.WriteAccessoryState(writer);
            writer.Write((byte)Accessories.Count);
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
            writer.Write(PmmUnknown.Matrix);
            EditorState.WriteViewFollowing(writer);
            writer.Write(Unknown.TruthValue);
            DrawConfig.WriteGroundPhysics(writer);
            EditorState.WriteFrameLocation(writer);

            // 範囲選択対象セクションがないバージョンなら終了
            if (!EditorState.ExistRangeSelectionTargetSection)
                return;

            writer.Write(EditorState.ExistRangeSelectionTargetSection);
            foreach (var index in EditorState.RangeSelectionTargetIndices)
            {
                writer.Write(index.Model);
                writer.Write(index.Target);
            }
        }
    }
}
