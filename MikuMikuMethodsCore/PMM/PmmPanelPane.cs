using MikuMikuMethods.PMM.Panel;

namespace MikuMikuMethods.PMM
{
    public class PmmPanelPane
    {
        /// <summary>
        /// カメラ操作パネル開閉状態
        /// </summary>
        public bool DoesOpenCameraPanel { get; set; } = true;
        /// <summary>
        /// ライト操作パネル開閉状態
        /// </summary>
        public bool DoesOpenLightPanel { get; set; } = true;
        /// <summary>
        /// アクセサリ操作パネル開閉状態
        /// </summary>
        public bool DoesOpenAccessaryPanel { get; set; } = true;
        /// <summary>
        /// ボーン操作パネル開閉状態
        /// </summary>
        public bool DoesOpenBonePanel { get; set; } = true;
        /// <summary>
        /// 表情操作パネル開閉状態
        /// </summary>
        public bool DoesOpenMorphPanel { get; set; } = true;
        /// <summary>
        /// セルフ影操作パネル開閉状態
        /// </summary>
        public bool DoesOpenSelfShadowPanel { get; set; } = true;

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

        public PmmPlayPanel PlayPanel { get; } = new();
    }
}