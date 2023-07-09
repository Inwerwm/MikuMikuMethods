namespace MikuMikuMethods.Common;

/// <summary>
/// 大きさ
/// </summary>
/// <param name="Width">幅</param>
/// <param name="Height">高さ</param>
public record Size(int Width, int Height);

/// <summary>
/// 二次元平面上の <typeparamref name="T"/> の点を表す構造体。
/// </summary>
/// <typeparam name="T">座標の型。</typeparam>
/// <param name="X">X 座標</param>
/// <param name="Y">Y 座標</param>
public record struct Point2<T>(T X, T Y)
{
    /// <summary>
    /// <see cref="Point2{T}"/> を (<typeparamref name="T"/>, <typeparamref name="T"/>) タプルに暗黙的に変換します。
    /// </summary>
    /// <param name="value">変換する <see cref="Point2{T}"/></param>
    public static implicit operator (T X, T Y)(Point2<T> value) => (value.X, value.Y);

    /// <summary>
    /// (<typeparamref name="T"/>, <typeparamref name="T"/>) タプルを <see cref="Point2{T}"/> に暗黙的に変換します。
    /// </summary>
    /// <param name="value">変換する (<typeparamref name="T"/>, <typeparamref name="T"/>) タプル</param>
    public static implicit operator Point2<T>((T X, T Y) value) => new(value.X, value.Y);
}

