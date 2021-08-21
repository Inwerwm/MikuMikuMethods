using System.Numerics;

namespace MikuMikuMethods.Binary.PMM.Frame
{
    /// <summary>
    /// 照明フレーム情報
    /// </summary>
    public class PmmLightFrame : PmmFrame
    {
        /// <summary>
        /// 色(RGBのみ使用)
        /// </summary>
        public ColorF Color { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmLightFrame()
        {
            Color = ColorF.FromARGB(154, 154, 154);
        }
    }
}
