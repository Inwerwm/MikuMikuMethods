namespace UnitTest;

internal static class TestData
{
    public static readonly string TestDataDirectory = "../../TestData/";

    public static string GetPath(string filename) => Path.Combine(TestDataDirectory, filename);
}
