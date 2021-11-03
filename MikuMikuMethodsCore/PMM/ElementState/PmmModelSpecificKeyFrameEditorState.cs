namespace MikuMikuMethods.PMM.ElementState
{
    /// <summary>
    /// 各モデルにおけるキーフレーム編集画面の状態
    /// </summary>
    public class PmmModelSpecificKeyFrameEditorState
    {
        /// <summary>
        /// 垂直スクロール状態
        /// </summary>
        public int VerticalScrollState { get; set; }

        /// <summary>
        /// 最終フレーム
        /// </summary>
        public int LastFrame { get; set; }
    }
}