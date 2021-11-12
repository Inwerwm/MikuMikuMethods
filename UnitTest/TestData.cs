using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest;

internal static class TestData
{
    public static readonly string TestDataDirectory = "../../TestData/";

    public static string GetPath(string filename) => Path.Combine(TestDataDirectory, filename);
}
