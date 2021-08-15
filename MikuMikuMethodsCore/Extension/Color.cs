using System.Drawing;

namespace MikuMikuMethods.Extension
{
    /// <summary>
    /// 色関連拡張メソッド
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// 浮動小数色に変換
        /// </summary>
        public static ColorF ToColorF(this Color color)
        {
            ColorF c = new();
            c.FromColor(color);
            return c;
        }
    }
}
