using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace UnitTest;
internal static class MacroAssert
{
    public static void AreElementsSame<TKey, TValue>(IDictionary<TKey, TValue> expected, IDictionary<TKey, TValue> actual, EqualityComparer<TValue> comparer, params Action<TValue, TValue>[] innerActions)
    {
        Assert.AreEqual(expected.Count, actual.Count);

        compareEachElements(expected, actual, "actual", comparer, innerActions);
        compareEachElements(actual, expected, "expected", comparer, innerActions);

        static void compareEachElements<TInnerKey, TInnerValue>(IDictionary<TInnerKey, TInnerValue> source, IDictionary<TInnerKey, TInnerValue> target, string targetName, EqualityComparer<TInnerValue> comparer, params Action<TInnerValue, TInnerValue>[] innerActions)
        {
            foreach (var item in source)
            {
                if (target.TryGetValue(item.Key, out var value))
                {
                    Assert.IsTrue(comparer.Equals(item.Value, value), $"The elements of {item.Key} are not equal.");
                    foreach (var innerAction in innerActions)
                    {
                        innerAction?.Invoke(item.Value, value);
                    }
                }
                else
                {
                    Assert.Fail($"Cannot find value of {item.Key} in the {targetName} dictionary");
                }
            }
        }
    }

    public static void AreElementsSame<T>(IEnumerable<T> expected, IEnumerable<T> actual, EqualityComparer<T> comparer, params Action<T, T>[] innerActions)
    {
        var couldGetExpCount = expected.TryGetNonEnumeratedCount(out var expCount);
        var couldGetActCount = actual.TryGetNonEnumeratedCount(out var actCount);

        Assert.AreEqual(
            couldGetExpCount ? expCount : expected.Count(),
            couldGetActCount ? actCount : actual.Count(),
            "The element counts of the collections are not equals.");

        foreach (var (exp, act, i) in expected.Zip(actual).Select((p, i) => (p.First, p.Second, i)))
        {
            Assert.IsTrue(comparer.Equals(exp, act), $"The elements are not equal at {i}.");
            foreach (var innerAction in innerActions)
            {
                innerAction?.Invoke(exp, act);
            }
        }
    }
}
