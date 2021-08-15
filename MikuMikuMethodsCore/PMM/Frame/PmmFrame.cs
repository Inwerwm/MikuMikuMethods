namespace MikuMikuMethods.PMM.Frame
{
    /// <summary>
    /// フレーム情報
    /// </summary>
    public abstract class PmmFrame
    {
        /// <summary>
        /// <para>フレーム番号</para>
        /// <para>初期フレームには振られないのでnullを入れる</para>
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// 所在フレーム
        /// </summary>
        public int Frame { get; set; }

        /// <summary>
        /// <para>直前のキーフレームのID</para>
        /// <para>存在しなければ0</para>
        /// </summary>
        public int PreviousFrameIndex { get; set; }
        /// <summary>
        /// 直後のキーフレームのID
        /// </summary>
        public int NextFrameIndex { get; set; }

        /// <summary>
        /// 選択されているか
        /// </summary>
        public bool IsSelected { get; set; }
    }
}