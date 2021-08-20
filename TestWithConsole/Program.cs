using System;
using System.Collections.Generic;
using System.Linq;

namespace TestWithConsole
{
    class Program
    {
        const string dataDir = "../../Data/";

        private static string Path(string filename) => dataDir + filename;

        static void Main(string[] args)
        {
        }
    }

    static class Extension
    {
        public static string Print<T>(this IEnumerable<T> collection) => Print(collection, ", ");
        public static string Print<T>(this IEnumerable<T> collection, string delimiter) => collection.Select(elm => elm.ToString()).Aggregate((acm, elm) => $"{acm}{delimiter}{elm}");
    }
}
