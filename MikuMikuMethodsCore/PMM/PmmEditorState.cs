using System.Collections.Generic;
using System.IO;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// MMD編集画面状態情報
    /// </summary>
    public class PmmEditorState
    {
        #region ViewState
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
        /// 描画情報関連をファイルから読み込み
        /// </summary>
        /// <param name="reader">バイナリファイル</param>
        internal void ReadViewState(BinaryReader reader)
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
        /// 描画情報関連をファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        internal void WriteViewState(BinaryWriter writer)
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
        #endregion

        #region AccessoryState
        /// <summary>
        /// 選択中のアクセサリインデックス
        /// </summary>
        public byte SelectedAccessoryIndex { get; set; }
        /// <summary>
        /// アクセサリの垂直スクロール量
        /// </summary>
        public int VerticalScrollOfAccessoryRow { get; set; }

        /// <summary>
        /// アクセサリ情報関連をファイルから読み込み
        /// </summary>
        /// <param name="reader">バイナリファイル</param>
        internal void ReadAccessoryState(BinaryReader reader)
        {
            SelectedAccessoryIndex = reader.ReadByte();
            VerticalScrollOfAccessoryRow = reader.ReadInt32();
        }

        /// <summary>
        /// アクセサリ情報関連をファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        internal void WriteAccessoryState(BinaryWriter writer)
        {
            writer.Write(SelectedAccessoryIndex);
            writer.Write(VerticalScrollOfAccessoryRow);
        }
        #endregion

        #region FrameState
        /// <summary>
        /// 現在のフレーム位置
        /// </summary>
        public int CurrentFrame { get; set; }
        /// <summary>
        /// キーフレームエディタの水平スクロール量
        /// </summary>
        public int HorizontalScroll { get; set; }
        /// <summary>
        /// キーフレームエディタの横スクロール範囲
        /// </summary>
        public int HorizontalScrollLength { get; set; }
        /// <summary>
        /// 選択中のボーン操作方法
        /// </summary>
        public BoneOperation SelectedBoneOperation { get; set; }

        /// <summary>
        /// フレーム編集関連情報をファイルから読み込み
        /// </summary>
        /// <param name="reader">バイナリファイル</param>
        internal void ReadFrameState(BinaryReader reader)
        {
            CurrentFrame = reader.ReadInt32();
            HorizontalScroll = reader.ReadInt32();
            HorizontalScrollLength = reader.ReadInt32();
            SelectedBoneOperation = (BoneOperation)reader.ReadInt32();
        }

        /// <summary>
        /// フレーム編集関連情報をファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        internal void WriteFrameState(BinaryWriter writer)
        {
            writer.Write(CurrentFrame);
            writer.Write(HorizontalScroll);
            writer.Write(HorizontalScrollLength);
            writer.Write((int)SelectedBoneOperation);
        }

        /// <summary>
        /// MMD上でのボーン操作
        /// </summary>
        public enum BoneOperation : int
        {
            /// <summary>
            /// 選択
            /// </summary>
            Select,
            /// <summary>
            /// 矩形選択
            /// </summary>
            RectangleSelect,
            /// <summary>
            /// 移動
            /// </summary>
            Move,
            /// <summary>
            /// 回転
            /// </summary>
            Rotate,
            /// <summary>
            /// 無選択
            /// </summary>
            None
        }
        #endregion

        #region ViewFollowing
        /// <summary>
        /// 再生時の視点追従のOn/Off
        /// </summary>
        public bool IsViewFollowCamera { get; set; }

        /// <summary>
        /// 再生時の視点追従をファイルから読み込み
        /// </summary>
        /// <param name="reader">バイナリファイル</param>
        internal void ReadViewFollowing(BinaryReader reader)
        {
            IsViewFollowCamera = reader.ReadBoolean();
        }

        /// <summary>
        /// 再生時の視点追従をファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        internal void WriteViewFollowing(BinaryWriter writer)
        {
            writer.Write(IsViewFollowCamera);
        }
        #endregion

        #region FrameLocation
        /// <summary>
        /// 3Dビュー上部のフレーム入力欄の値
        /// </summary>
        public int FrameLocation { get; set; }

        /// <summary>
        /// フレーム入力欄の値をファイルから読み込み
        /// </summary>
        /// <param name="reader">バイナリファイル</param>
        internal void ReadFrameLocation(BinaryReader reader)
        {
            FrameLocation = reader.ReadInt32();
        }

        /// <summary>
        /// フレーム入力欄の値をファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        internal void WriteFrameLocation(BinaryWriter writer)
        {
            writer.Write(FrameLocation);
        }
        #endregion

        #region RangeSelectionTarget
        /// <summary>
        /// 範囲選択対象のセクションが存在しているか
        /// </summary>
        public bool ExistRangeSelectionTargetSection { get; set; }
        /// <summary>
        /// 範囲選択対象
        /// </summary>
        public List<(byte Model, int Target)> RangeSelectionTargetIndices { get; init; } = new();
        #endregion
    }
}
