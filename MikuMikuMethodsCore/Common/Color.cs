﻿using System.Drawing;

namespace MikuMikuMethods
{
    /// <summary>
    /// 浮動小数点で値を保持する色構造体
    /// </summary>
    public struct ColorF
    {
        /// <summary>
        /// 赤
        /// </summary>
        public float R { get; set; }
        /// <summary>
        /// 緑
        /// </summary>
        public float G { get; set; }
        /// <summary>
        /// 青
        /// </summary>
        public float B { get; set; }
        /// <summary>
        /// 不透明度
        /// </summary>
        public float A { get; set; }

        /// <summary>
        /// バイト整数で値を保持する色構造体に変換
        /// </summary>
        /// <returns>色</returns>
        public Color ToColor() => Color.FromArgb((int)(A * 255), (int)(R * 255), (int)(G * 255), (int)(B * 255));

        /// <summary>
        /// バイト整数で値を保持する色構造体から変換
        /// </summary>
        /// <param name="color">変換元の色</param>
        public void FromColor(Color color)
        {
            R = color.R / 255.0f;
            G = color.G / 255.0f;
            B = color.B / 255.0f;
            A = color.A / 255.0f;
        }

        /// <summary>
        /// 透明度と元色を指定して色構造体を作成
        /// </summary>
        /// <param name="alpha">透明度</param>
        /// <param name="baseColor">元となる色</param>
        /// <returns>浮動小数色構造体</returns>
        public static ColorF FromARGB(float alpha, ColorF baseColor)
        {
            ColorF resultColor = new();
            resultColor.A = alpha;
            resultColor.R = baseColor.R;
            resultColor.G = baseColor.G;
            resultColor.B = baseColor.B;

            return resultColor;
        }

        /// <summary>
        /// 数値指定で色構造体を作成
        /// </summary>
        /// <param name="alpha">透明度</param>
        /// <param name="red">赤</param>
        /// <param name="green">緑</param>
        /// <param name="blue">青</param>
        /// <returns>浮動小数色構造体</returns>
        public static ColorF FromARGB(float alpha,float red,float green,float blue)
        {
            ColorF resultColor = new();
            resultColor.A = alpha;
            resultColor.R = red;
            resultColor.G = green;
            resultColor.B = blue;
            return resultColor;
        }

        /// <summary>
        /// 数値指定で色構造体を作成
        /// </summary>
        /// <param name="red">赤</param>
        /// <param name="green">緑</param>
        /// <param name="blue">青</param>
        /// <returns>浮動小数色構造体</returns>
        public static ColorF FromARGB(float red, float green, float blue)
        {
            ColorF resultColor = new();
            resultColor.A = 1;
            resultColor.R = red;
            resultColor.G = green;
            resultColor.B = blue;
            return resultColor;
        }
    }
}
