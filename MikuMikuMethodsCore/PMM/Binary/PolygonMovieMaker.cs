using System.Collections.Generic;

namespace MikuMikuMethods.PMM.Binary
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
        /// 大きさを整数で表す
        /// </summary>
        public record Size(int Width, int Height);
        /// <summary>
        /// 出力解像度
        /// </summary>
        public Size Output { get; set; }

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
        public PmmViewConfig ViewConfig { get; init; }

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
            ViewConfig = new();

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
            IO.PmmFileWriter.Write(filePath, this);
        }
    }
}
