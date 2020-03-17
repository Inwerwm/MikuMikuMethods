using System;

namespace MikuMikuMethods
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// 指定された開区間の範囲内であるかを判断します。
        /// </summary>
        /// <param name="i"></param>
        /// <param name="lower">下限</param>
        /// <param name="upper">上限</param>
        /// <returns></returns>
        public static bool IsWithin<T>(this T i, T lower, T upper) where T : IComparable
        {
            if (upper.CompareTo(lower) < 0)
                throw new ArgumentOutOfRangeException("下限値が上限値よりも大きいです。");
            return i.CompareTo(lower) * upper.CompareTo(i) > 0;
        }

        /// <summary>
        /// 指定された閉区間の範囲内であるかを判断します。
        /// </summary>
        /// <param name="i"></param>
        /// <param name="lower">下限</param>
        /// <param name="upper">上限</param>
        /// <returns></returns>
        public static bool IsInclude<T>(this T i, T lower, T upper) where T : IComparable
        {
            if (upper.CompareTo(lower) < 0)
                throw new ArgumentOutOfRangeException("下限値が上限値よりも大きいです。");
            return i.CompareTo(lower) * upper.CompareTo(i) >= 0;
        }
    }
}
