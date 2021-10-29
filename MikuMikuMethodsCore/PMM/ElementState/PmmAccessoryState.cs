using MikuMikuMethods.Extension;
using System;
using System.Numerics;

namespace MikuMikuMethods.PMM.ElementState
{
    public class PmmAccessoryState
    {
        private float transparency;

        internal byte TransAndVisible
        {
            get => CreateTransAndVisible(Transparency, Visible);
            set => (Visible, Transparency) = SeparateTransAndVisible(value);
        }

        public bool Visible { get; set; } = true;
        public float Transparency
        {
            get => transparency;
            set
            {
                if (value.IsWithin(0, 1))
                    transparency = value;
                else
                    throw new ArgumentOutOfRangeException("透明度は [0, 1] の範囲である必要があります。");
            }
        }

        /// <summary>
        /// 親モデル
        /// </summary>
        public PmmModel ParentModel { get; set; }
        /// <summary>
        /// 親ボーン
        /// </summary>
        public PmmBone ParentBone { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// 回転
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// 拡縮
        /// </summary>
        public float Scale { get; set; }
        /// <summary>
        /// 影のOn/Off
        /// </summary>
        public bool EnableShadow { get; set; }

        internal static (bool Visible, float Transparency) SeparateTransAndVisible(byte value) =>
            ((value & 0x1) == 0x1, (100 - (value >> 1)) / 100f);
        internal static byte CreateTransAndVisible(float transparency, bool visible)
        {
            var tr = 100 - (byte)Math.Round(transparency * 100);
            return (byte)((tr << 1) | (visible ? 1 : 0));
        }
    }
}