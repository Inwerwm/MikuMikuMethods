using MikuMikuMethods.Pmm;
using System;

namespace TestWithConsole;

class Program
{
    const string dataDir = "../../TestData/";

    private static string Path(string filename) => dataDir + filename;

    static void Main(string[] args)
    {
        PolygonMovieMaker pmm = new(Path("GroundParent.pmm"));
        pmm.Write(Path("out_GroundParent.pmm"));
    }
}
