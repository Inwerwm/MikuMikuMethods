using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;
using System.Numerics;

namespace MikuMikuMethods.PMM
{
    public class PmmLight
    {
        public List<PmmLightFrame> Frames { get; } = new();
        public TemporaryLightState Current { get; } = new();

        public class TemporaryLightState
        {
            /// <summary>
            /// 色(RGBのみ使用)
            /// </summary>
            public ColorF Color { get; set; } = System.Drawing.Color.FromArgb(154, 154, 154).ToColorF();
            /// <summary>
            /// 位置
            /// </summary>
            public Vector3 Position { get; set; } = new(-0.5f, -1.0f, 0.5f);
        }
    }
}