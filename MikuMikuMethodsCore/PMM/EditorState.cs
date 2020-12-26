using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// MMD編集画面状態情報
    /// </summary>
    public struct EditorState
    {
        /// <summary>
        /// 出力解像度幅
        /// </summary>
        public int OutputWidth { get; set; }
        /// <summary>
        /// 出力解像度高
        /// </summary>
        public int OutputHeight { get; set; }

        /// <summary>
        /// キーフレームエディタ(画面左)の幅
        /// </summary>
        public int KeyframeEditorWidth { get; set; }

        /// <summary>
        /// 編集中の視野角
        /// </summary>
        public float CurrentViewAngle { get; set; }

        /// <summary>
        /// 現在カメラ編集モードか？
        /// </summary>
        public bool IsCameraMode { get; set; }

        /// <summary>
        /// カメラ操作パネル開閉状態
        /// </summary>
        public bool doesOpenCameraPanel { get; set; }
        /// <summary>
        /// ライト操作パネル開閉状態
        /// </summary>
        public bool doesOpenLightPanel { get; set; }
        /// <summary>
        /// アクセサリ操作パネル開閉状態
        /// </summary>
        public bool doesOpenAccessaryPanel { get; set; }
        /// <summary>
        /// ボーン操作パネル開閉状態
        /// </summary>
        public bool doesOpenBonePanel { get; set; }
        /// <summary>
        /// 表情操作パネル開閉状態
        /// </summary>
        public bool doesOpenMorphPanel { get; set; }
        /// <summary>
        /// セルフ影操作パネル開閉状態
        /// </summary>
        public bool doesOpenSelfShadowPanel { get; set; }

        /// <summary>
        /// 選択中のモデルインデックス
        /// </summary>
        public byte SelectedModelIndex { get; set; }

        /// <summary>
        /// モデル数
        /// </summary>
        public byte ModelCount { get; set; }

        /// <summary>
        /// ファイルから読み込み
        /// </summary>
        /// <param name="reader">バイナリファイル</param>
        public void Read(BinaryReader reader)
        {
            OutputWidth = reader.ReadInt32();
            OutputHeight = reader.ReadInt32();

            KeyframeEditorWidth = reader.ReadInt32();

            CurrentViewAngle = reader.ReadSingle();

            IsCameraMode = reader.ReadByte() == 1;

            doesOpenCameraPanel = reader.ReadByte() == 1;
            doesOpenLightPanel = reader.ReadByte() == 1;
            doesOpenAccessaryPanel = reader.ReadByte() == 1;
            doesOpenBonePanel = reader.ReadByte() == 1;
            doesOpenMorphPanel = reader.ReadByte() == 1;
            doesOpenSelfShadowPanel = reader.ReadByte() == 1;

            SelectedModelIndex = reader.ReadByte();
            ModelCount = reader.ReadByte();
        }
    }
}
