namespace MikuMikuMethods.Extension;

/// <summary>
/// 範囲内判定系拡張メソッド
/// </summary>
internal static class Range
{
    /// <summary>
    /// 指定された閉区間の範囲内であるかを判断します。
    /// </summary>
    /// <param name="i"></param>
    /// <param name="lower">下限（自身も含む）</param>
    /// <param name="upper">上限（自身も含む）</param>
    /// <returns></returns>
    public static bool IsWithin<T>(this T i, T lower, T upper) where T : IComparable
    {
        if (upper.CompareTo(lower) < 0)
            throw new ArgumentOutOfRangeException("IsWithin<T>:下限値が上限値よりも大きいです。");
        return i.CompareTo(lower) * upper.CompareTo(i) >= 0;
    }

    /// <summary>
    /// 指定された開区間の範囲内であるかを判断します。
    /// </summary>
    /// <param name="i"></param>
    /// <param name="lower">下限（自身は含まない）</param>
    /// <param name="upper">上限（自身は含まない）</param>
    /// <returns></returns>
    public static bool IsInside<T>(this T i, T lower, T upper) where T : IComparable
    {
        if (upper.CompareTo(lower) < 0)
            throw new ArgumentOutOfRangeException("IsInside<T>:下限値が上限値よりも大きいです。");
        return i.CompareTo(lower) * upper.CompareTo(i) > 0;
    }

    /// <summary>
    /// 区間種別
    /// </summary>
    public enum Interval
    {
        /// <summary>
        /// 開区間
        /// </summary>
        Open,
        /// <summary>
        /// 閉区間
        /// </summary>
        Close
    }

    /// <summary>
    /// 指定した区間の範囲内であるかを判断します。
    /// 閉区間：自身を含む
    /// 開区間：自身を含まない
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="i"></param>
    /// <param name="lower">下限</param>
    /// <param name="lowerInterval">左側の開閉</param>
    /// <param name="upper">上限</param>
    /// <param name="upperInterval">右側の開閉</param>
    /// <returns></returns>
    public static bool IsInRangeOf<T>(this T i, T lower, Interval lowerInterval, T upper, Interval upperInterval) where T : IComparable
    {
        if (upper.CompareTo(lower) < 0)
            throw new ArgumentOutOfRangeException("IsInRangeOf<T>:下限値が上限値よりも大きいです。");

        var l = i.CompareTo(lower);
        bool isInRangeLower = lowerInterval == Interval.Close ? l >= 0 : l > 0;
        var u = upper.CompareTo(i);
        bool isInRangeUpper = upperInterval == Interval.Close ? u >= 0 : u > 0;

        return isInRangeLower && isInRangeUpper;
    }
}
