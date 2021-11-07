using System.Collections.Generic;

namespace MikuMikuMethods.PMM.Binary
{
    /// <summary>
    /// MMD編集画面状態情報
    /// </summary>
    public class PmmEditorState
    {
        /// <summary>
        /// 現在カメラ編集モードか？
        /// </summary>
        public bool IsCameraMode { get; set; }
        /// <summary>
        /// 編集中の視野角
        /// </summary>
        public float CurrentViewAngle { get; set; }

        /// <summary>
        /// 現在のフレーム位置
        /// </summary>
        public int CurrentFrame { get; set; }
        /// <summary>
        /// キーフレームエディタ(画面左)の幅
        /// </summary>
        public int KeyframeEditorWidth { get; set; }
        /// <summary>
        /// キーフレームエディタの水平スクロール量
        /// </summary>
        public int HorizontalScroll { get; set; }
        /// <summary>
        /// キーフレームエディタの横スクロール量
        /// </summary>
        public int HorizontalScrollLength { get; set; }
        /// <summary>
        /// アクセサリの垂直スクロール量
        /// </summary>
        public int VerticalScrollOfAccessoryRow { get; set; }

        /// <summary>
        /// 選択中のモデルインデックス
        /// </summary>
        public byte SelectedModelIndex { get; set; }
        /// <summary>
        /// 選択中のアクセサリインデックス
        /// </summary>
        public byte SelectedAccessoryIndex { get; set; }

        /// <summary>
        /// 選択中のボーン操作方法
        /// </summary>
        public BoneOperation SelectedBoneOperation { get; set; }
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

        /// <summary>
        /// 範囲選択対象のセクションが存在しているか
        /// </summary>
        public bool ExistRangeSelectionTargetSection { get; set; }
        /// <summary>
        /// 範囲選択対象
        /// </summary>
        public List<(PmmModel Model, int Target)> RangeSelectionTargetIndices { get; init; } = new();

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
    }
}
