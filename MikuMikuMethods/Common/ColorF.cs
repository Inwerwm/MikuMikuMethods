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

    /// <summary>
    /// RGB値を-1.0から1.0の範囲で指定して初期化します。透明度は1に設定されます。
    /// </summary>
    public ColorF(float red, float green, float blue)
        : this(1f, red, green, blue)
    {
    }

    /// <summary>
    /// RGB値を-255から255の範囲で指定して初期化します。透明度は1に設定されます。
    /// </summary>
    public ColorF(int red, int green, int blue)
        : this(1f, red / 255.0f, green / 255.0f, blue / 255.0f)
    {
    }

    /// <summary>
    /// 透明度とRGB値を-1.0から1.0の範囲で指定して初期化します。
    /// </summary>
    public ColorF(float alpha, float red, float green, float blue)
    {
        A = Clamp(alpha, -1f, 1f);
        R = Clamp(red, -1f, 1f);
        G = Clamp(green, -1f, 1f);
        B = Clamp(blue, -1f, 1f);
    }

    /// <summary>
    /// 透明度とRGB値を-255から255の範囲で指定して初期化します。
    /// </summary>
    public ColorF(int alpha, int red, int green, int blue)
        : this(alpha / 255.0f, red / 255.0f, green / 255.0f, blue / 255.0f)
    {
    }

    /// <summary>
    /// 指定した値を指定した範囲内に収めます。
    /// </summary>
    /// <param name="value">クランプする値</param>
    /// <param name="min">値の下限</param>
    /// <param name="max">値の上限</param>
    /// <returns>クランプされた値</returns>
    private static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
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
