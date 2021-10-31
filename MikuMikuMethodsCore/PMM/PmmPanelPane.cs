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
    }
}