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
            var pmm = new MikuMikuMethods.Binary.PMM.PolygonMovieMaker(Path("pmm.pmm"));
            WLL(pmm.Models.PrintLn());

            var miku = pmm.Models[0];
            WL(miku.Name);
            Func<MikuMikuMethods.Binary.PMM.Frame.PmmBoneFrame, string> boneFrameInfo = f => $"ID:{f.Index}, Time:{f.Frame}, Offset:{f.Offset}, PreID:{f.PreviousFrameIndex}, PostID:{f.NextFrameIndex}";
            WLL(miku.BoneFrames.Select(boneFrameInfo).PrintLn());
            WLL($"初期ボーンフレーム数:{miku.InitialBoneFrames.Count}");
            //WLL(miku.InitialBoneFrames.Select(boneFrameInfo).PrintLn());

            WLL($"FPS制限:{pmm.ViewConfig.FPSLimit}");

            MikuMikuMethods.Binary.PMM.PmmAccessory negi = pmm.Accessories[0];
            WL($"アクセサリ表示: {negi.Uncomitted.OpacityAndVisible & 0x1}");
            WLL($"アクセサリ透明度数値: {(100 - (negi.Uncomitted.OpacityAndVisible >> 1)) / 100f}");

        }
    }

    static class Extension
    {
        public static string Print<T>(this IEnumerable<T> collection) => Print(collection, ", ");
        public static string PrintLn<T>(this IEnumerable<T> collection) => Print(collection, Environment.NewLine);
        public static string Print<T>(this IEnumerable<T> collection, string delimiter) => collection.Select(elm => elm.ToString()).Aggregate((acm, elm) => $"{acm}{delimiter}{elm}");
    }
}
