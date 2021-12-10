using MikuMikuMethods.Pmm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestWithConsole;

class Program
{
    const string dataDir = "../../Data/";

    private static string Path(string filename) => dataDir + filename;

    static void Main(string[] args)
    {
        PolygonMovieMaker pmm = new(Path("RangeSelectorTest.pmm"));

        Console.WriteLine(pmm.Models[0].SpecificEditorState.RangeSelector);
        Console.WriteLine(pmm.Models[1].SpecificEditorState.RangeSelector);
    }
}
