using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuMethods.Extension;

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
        /// コンストラクタ
        /// </summary>
        public PolygonMovieMaker()
        {
            EditorState = new();
            Models = new();
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル ShiftJISエンコードで読み込むこと</param>
        public void Read(BinaryReader reader)
        {
            Version = reader.ReadString(30, Encoding.ShiftJIS, '\0');

            EditorState.Read(reader);

            var modelCount = reader.ReadByte();
            for (int i = 0; i < modelCount; i++)
            {
                PmmModel model = new();
                model.Read(reader);
                Models.Add(model);
            }
        }
    }
}
