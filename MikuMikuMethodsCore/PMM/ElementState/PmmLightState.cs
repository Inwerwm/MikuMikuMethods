using MikuMikuMethods.Extension;
using System.Numerics;

namespace MikuMikuMethods.Pmm.ElementState
{
    public class PmmLightState
    {
        /// <summary>
        /// 色(RGBのみ使用)
        /// </summary>
        public ColorF Color { get; set; } = System.Drawing.Color.FromArgb(154, 154, 154).ToColorF();
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; } = new(-0.5f, -1.0f, 0.5f);

        public PmmLightState DeepCopy() => new()
        {
            Color = Color,
            Position = Position
        };
    }
}