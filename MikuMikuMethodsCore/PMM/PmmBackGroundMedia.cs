using MikuMikuMethods.Common;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// 背景メディア情報
    /// </summary>
    public class PmmBackGroundMedia
    {
        /// <summary>
        /// 音声ファイルへのパス
        /// null ならなし
        /// </summary>
        public string? Audio { get; set; } = null;

        /// <summary>
        /// 背景色は黒か
        /// </summary>
        public bool IsBlack { get; set; } = false;

        /// <summary>
        /// 背景AVIへのパス
        /// null ならなし
        /// </summary>
        public string? Video { get; set; } = null;
        /// <summary>
        /// 背景AVIの表示スケール
        /// </summary>
        public float VideoScale { get; set; } = 1;
        /// <summary>
        /// 背景AVIの表示位置
        /// </summary>
        public Point2<int> VideoOffset { get; set; } = new(0, 0);

        /// <summary>
        /// 背景画像へのパス
        /// null ならなし
        /// </summary>
        public string? Image { get; set; } = null;
        /// <summary>
        /// 背景画像のスケール
        /// </summary>
        public float ImageScale { get; set; } = 1;
        /// <summary>
        /// 背景画像の表示位置
        /// </summary>
        public Point2<int> ImageOffset { get; set; } = new(0, 0);
    }
}