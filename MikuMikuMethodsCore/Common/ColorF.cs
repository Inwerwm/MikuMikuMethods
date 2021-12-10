using System.Drawing;

namespace MikuMikuMethods;

/// <summary>
/// 浮動小数点で値を保持する色構造体
/// </summary>
public record ColorF
{
    /// <summary>
    /// 赤
    /// </summary>
    public float R { get; init; }
    /// <summary>
    /// 緑
    /// </summary>
    public float G { get; init; }
    /// <summary>
    /// 青
    /// </summary>
    public float B { get; init; }
    /// <summary>
    /// 不透明度
    /// </summary>
    public float A { get; init; }

    public ColorF(float red, float green, float blue)
    {
        A = 1;
        R = red;
        G = green;
        B = blue;
    }
    public ColorF(int red, int green, int blue)
    {
        A = 1;
        R = red / 255.0f;
        G = green / 255.0f;
        B = blue / 255.0f;
    }
    public ColorF(float alpha, float red, float green, float blue)
    {
        A = alpha;
        R = red;
        G = green;
        B = blue;
    }
    public ColorF(int alpha, int red, int green, int blue)
    {
        A = alpha / 255.0f;
        R = red / 255.0f;
        G = green / 255.0f;
        B = blue / 255.0f;
    }

    /// <summary>
    /// バイト整数で値を保持する色構造体に変換
    /// </summary>
    /// <returns>色</returns>
    public Color ToColor() => Color.FromArgb((int)(A * 255), (int)(R * 255), (int)(G * 255), (int)(B * 255));

    /// <summary>
    /// バイト整数で値を保持する色構造体から変換
    /// </summary>
    /// <param name="color">変換元の色</param>
    public static ColorF FromColor(Color color) => new(color.A, color.R, color.G, color.B);

    /// <summary>
    /// 透明度と元色を指定して色構造体を作成
    /// </summary>
    /// <param name="alpha">透明度</param>
    /// <param name="baseColor">元となる色</param>
    /// <returns>浮動小数色構造体</returns>
    public static ColorF FromARGB(float alpha, ColorF baseColor) => new(alpha, baseColor.R, baseColor.G, baseColor.B);

    /// <summary>
    /// 数値指定で色構造体を作成
    /// </summary>
    /// <param name="alpha">透明度</param>
    /// <param name="red">赤</param>
    /// <param name="green">緑</param>
    /// <param name="blue">青</param>
    /// <returns>浮動小数色構造体</returns>
    public static ColorF FromARGB(float alpha, float red, float green, float blue) => new(alpha, red, green, blue);

    /// <summary>
    /// 数値指定で色構造体を作成
    /// </summary>
    /// <param name="red">赤</param>
    /// <param name="green">緑</param>
    /// <param name="blue">青</param>
    /// <returns>浮動小数色構造体</returns>
    public static ColorF FromARGB(float red, float green, float blue) => new(red, green, blue);
}
