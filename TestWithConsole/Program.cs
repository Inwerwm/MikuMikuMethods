﻿using System;
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
            WLL(pmm.Models.PrintLn());

            var miku = pmm.Models[0];
            WL(miku.Name);
            Func<MikuMikuMethods.PMM.Frame.PmmBoneFrame, string> boneFrameInfo = f => $"ID:{f.Index}, Time:{f.Frame}, Offset:{f.Offset}, PreID:{f.PreviousFrameIndex}, PostID:{f.NextFrameIndex}";
            WLL(miku.BoneFrames.Select(boneFrameInfo).PrintLn());
            WL($"初期ボーンフレーム数:{miku.InitialBoneFrames.Count}");
            //WLL(miku.InitialBoneFrames.Select(boneFrameInfo).PrintLn());
        }
    }

    static class Extension
    {
        public static string Print<T>(this IEnumerable<T> collection) => Print(collection, ", ");
        public static string PrintLn<T>(this IEnumerable<T> collection) => Print(collection, Environment.NewLine);
        public static string Print<T>(this IEnumerable<T> collection, string delimiter) => collection.Select(elm => elm.ToString()).Aggregate((acm, elm) => $"{acm}{delimiter}{elm}");
    }
}
