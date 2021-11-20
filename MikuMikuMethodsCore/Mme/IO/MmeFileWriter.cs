using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.Mme.IO;

internal static class MmeFileWriter
{
    public static void WriteEmdData(string filePath, EmdData emd)
    {
        using FileStream file = new(filePath, FileMode.Create);
        using StreamWriter writer = new(file, Encoding.ShiftJIS);
        WriteEmdData(writer, emd);
    }

    public static void WriteEmdData(StreamWriter writer, EmdData emd)
    {
        if (writer.Encoding != Encoding.ShiftJIS)
            throw new ArgumentException($"EMMファイル書き込みエンコードエラー{Environment.NewLine}エンコーダがShiftJISと違います。");

        // [Info]
        writer.WriteLine("[Info]");
        writer.WriteLine($"Version = {emd.Version}");
        writer.WriteLine();

        // [Object]
        writer.WriteLine("[Effect]");

        if (!string.IsNullOrEmpty(emd.Material.Path))
            writer.WriteLine($"Obj = {emd.Material.Path}");
        if (emd.Material.Show != null)
            writer.WriteLine($"Obj.show = {emd.Material.Show.Value.ToString().ToLower()}");

        foreach (var sub in emd.Subsets.Select((effect, i) => (effect, i)))
        {
            if (!string.IsNullOrEmpty(sub.effect.Path))
                writer.WriteLine($"Obj[{sub.i}] = {sub.effect.Path}");
            if (sub.effect.Show != null)
                writer.WriteLine($"Obj[{sub.i}].show = {sub.effect.Show.Value.ToString().ToLower()}");
        }
        writer.WriteLine();
    }

    public static void WriteEmmData(string filePath, EmmData emm)
    {
        using FileStream file = new(filePath, FileMode.Create);
        using StreamWriter writer = new(file, Encoding.ShiftJIS);
        WriteEmmData(writer, emm);
    }

    public static void WriteEmmData(StreamWriter writer, EmmData emm)
    {
        if (writer.Encoding != Encoding.ShiftJIS)
            throw new FormatException($"EMMファイル書き込みエンコードエラー{Environment.NewLine}エンコーダがShiftJISと違います。");

        // [Info]
        writer.WriteLine("[Info]");
        writer.WriteLine($"Version = {emm.Version}");
        writer.WriteLine("");

        // [Object]
        writer.WriteLine("[Object]");
        foreach (var obj in emm.Objects)
        {
            writer.WriteLine($"{obj.Name} = {obj.Path}");
        }
        writer.WriteLine("");

        // Effect
        foreach (var effect in emm.EffectSettings)
        {
            writer.WriteLine(effect.IsMain ? "[Effect]" : $"[Effect@{effect.Name}]");
            WriteEffectSettings(writer, effect);
            writer.WriteLine("");
        }
    }

    internal static void WriteEffectSettings(StreamWriter writer, EmmEffectSettings effect)
    {
        // Main なら Default に、そうでなければ Owner には必ず値が入っている
        writer.WriteLine(effect.IsMain ? $"Default = {effect.Default!.Path}" : $"Owner = {effect.Owner!.Name}");

        foreach (var obj in effect.ObjectSettings)
        {
            if (!string.IsNullOrEmpty(obj.Material.Path))
                writer.WriteLine($"{obj.Object.Name} = {obj.Material.Path}");

            if (obj.Material.Show != null)
                writer.WriteLine($"{obj.Object.Name}.show = {obj.Material.Show.Value.ToString().ToLower()}");

            foreach (var sub in obj.Subsets.Select((effect, i) => (effect, i)))
            {
                if (!string.IsNullOrEmpty(sub.effect.Path))
                    writer.WriteLine($"{obj.Object.Name}[{sub.i}] = {sub.effect.Path}");
                if (sub.effect.Show != null)
                    writer.WriteLine($"{obj.Object.Name}[{sub.i}].show = {sub.effect.Show.Value.ToString().ToLower()}");
            }
        }
    }

}
