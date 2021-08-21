namespace MikuMikuMethods.Binary.PMM.Frame
{
    /// <summary>
    /// セルフ影フレーム情報
    /// </summary>
    public class PmmSelfShadowFrame : PmmFrame
    {
        /// <summary>
        /// 影モード
        /// </summary>
        public Shadow ShadowMode { get; set; }
        /// <summary>
        /// 影範囲
        /// </summary>
        public float ShadowRange { get; set; }

        /// <summary>
        /// 影のモード
        /// </summary>
        public enum Shadow : byte
        {
            /// <summary>
            /// 影なし
            /// </summary>
            None,
            /// <summary>
            /// モード1
            /// </summary>
            Mode1,
            /// <summary>
            /// モード2
            /// </summary>
            Mode2,
        }
    }
}
