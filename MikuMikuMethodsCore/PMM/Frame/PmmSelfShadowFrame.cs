namespace MikuMikuMethods.PMM.Frame
{
    public class PmmSelfShadowFrame : IPmmFrame
    {
        public int Frame { get; set; }
        public bool IsSelected { get; set; }

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

        public bool Equals(IPmmFrame other) => other is PmmSelfShadowFrame f && f.Frame == Frame;
    }
}