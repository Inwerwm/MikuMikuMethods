using System;
using System.Collections.Generic;
using System.Linq;

namespace TestWithConsole
{
    class Program
    {
        const string dataDir = "../../Data/";

        private static string Path(string filename) => dataDir + filename;
        private static void WL(string? value = "") => Console.WriteLine(value);
        private static void WLL(string? value = "") => Console.WriteLine(value + Environment.NewLine);

        static void Main(string[] args)
        {
            var pmm = new MikuMikuMethods.PMM.PolygonMovieMaker(Path("pmm.pmm"));
            WLL(pmm.Models.Print("\n"));

            var miku = pmm.Models[0];
            var haku = pmm.Models[1];
            WLL(pmm.Models.Select(m => (m.RenderConfig.RenderOrder, m.Name)).Print("\n"));

            miku.RenderConfig.RenderOrder = 2;
            haku.RenderConfig.RenderOrder = 1;
            WLL(pmm.Models.Select(m => (m.RenderConfig.RenderOrder, m.Name)).Print("\n"));

            pmm.Write(Path("out.pmm"));
        }
    }

    static class Extension
    {
        public static string Print<T>(this IEnumerable<T> collection) => Print(collection, ", ");
        public static string Print<T>(this IEnumerable<T> collection, string delimiter) => collection.Select(elm => elm.ToString()).Aggregate((acm, elm) => $"{acm}{delimiter}{elm}");
    }
}
