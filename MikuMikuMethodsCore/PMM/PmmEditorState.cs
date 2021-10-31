namespace MikuMikuMethods.PMM
{
    public class PmmEditorState
    {
        /// <summary>
        /// 編集画面の表示幅
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// アクセサリ選択の縦スクロール量
        /// </summary>
        public int VerticalScrollOfAccessory { get; set; }
        /// <summary>
        /// 現在カメラ編集モードか？
        /// </summary>
        public bool IsCameraMode { get; set; }
        /// <summary>
        /// 選択中のモデル
        /// </summary>
        public PmmModel SelectedModel { get; set; }
        /// <summary>
        /// 選択中のアクセサリ
        /// </summary>
        public PmmAccessory SelectedAccessory { get; set; }
        /// <summary>
        /// 選択中のボーン操作方法
        /// </summary>
        public BoneOperation SelectedBoneOperation { get; set; }
        /// <summary>
        /// 現在のフレーム位置
        /// </summary>
        public int CurrentFrame { get; set; }
        /// <summary>
        /// キーフレームエディタの水平スクロール量
        /// </summary>
        public int HorizontalScroll { get; set; }
        /// <summary>
        /// キーフレームエディタの横スクロール可能長
        /// </summary>
        public int HorizontalScrollLength { get; set; }
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
    }
}