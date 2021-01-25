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
    public class EditorState
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
        public bool DoesOpenCameraPanel { get; set; }
        /// <summary>
        /// ライト操作パネル開閉状態
        /// </summary>
        public bool DoesOpenLightPanel { get; set; }
        /// <summary>
        /// アクセサリ操作パネル開閉状態
        /// </summary>
        public bool DoesOpenAccessaryPanel { get; set; }
        /// <summary>
        /// ボーン操作パネル開閉状態
        /// </summary>
        public bool DoesOpenBonePanel { get; set; }
        /// <summary>
        /// 表情操作パネル開閉状態
        /// </summary>
        public bool DoesOpenMorphPanel { get; set; }
        /// <summary>
        /// セルフ影操作パネル開閉状態
        /// </summary>
        public bool DoesOpenSelfShadowPanel { get; set; }

        /// <summary>
        /// 選択中のモデルインデックス
        /// </summary>
        public byte SelectedModelIndex { get; set; }

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

            IsCameraMode = reader.ReadBoolean();

            DoesOpenCameraPanel = reader.ReadBoolean();
            DoesOpenLightPanel = reader.ReadBoolean();
            DoesOpenAccessaryPanel = reader.ReadBoolean();
            DoesOpenBonePanel = reader.ReadBoolean();
            DoesOpenMorphPanel = reader.ReadBoolean();
            DoesOpenSelfShadowPanel = reader.ReadBoolean();

            SelectedModelIndex = reader.ReadByte();
        }

        /// <summary>
        /// ファイルに読込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(OutputWidth);
            writer.Write(OutputHeight);

            writer.Write(KeyframeEditorWidth);

            writer.Write(CurrentViewAngle);

            writer.Write(IsCameraMode);

            writer.Write(DoesOpenCameraPanel);
            writer.Write(DoesOpenLightPanel);
            writer.Write(DoesOpenAccessaryPanel);
            writer.Write(DoesOpenBonePanel);
            writer.Write(DoesOpenMorphPanel);
            writer.Write(DoesOpenSelfShadowPanel);

            writer.Write(SelectedModelIndex);
        }
    }
}
