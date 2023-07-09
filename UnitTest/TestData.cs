using MikuMikuMethods;
using MikuMikuMethods.Common;
using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.IO;
using System.Diagnostics;

namespace UnitTest;

internal static class TestData
{
    public static readonly string TestDataDirectory = "../../TestData/";

    public static string GetPath(string filename) => Path.Combine(TestDataDirectory, filename);

    public static PolygonMovieMaker PmmLoggingRead(string filenameStem, bool withTime = true)
    {
        // 読み込み履歴出力の準備
        string logPath = GetPath(filenameStem + "_Readlog.txt");
        File.Delete(logPath);
        using var log = File.AppendText(logPath);

        var output = (string message) =>
        {
            log.WriteLine(message);
            // switch 式を使うために値を返すようにしてる
            return message;
        };

        return withTime ? ReadPmmWithLogAndTime(filenameStem, output) : ReadPmmWithLog(filenameStem, output);
    }

    private static PolygonMovieMaker ReadPmmWithLog(string filenameStem, Func<string, string> output)
    {
        var sections = new List<DataSection>();

        try
        {
            return PmmReadWithCallback(
                filenameStem,
                output,
                section =>
                {
                    output("");
                    output(section.ToString());
                    sections.Add(section);
                }
            );
        }
        finally
        {
            Console.WriteLine(string.Join(Environment.NewLine, sections));
        }
    }

    private static PolygonMovieMaker ReadPmmWithLogAndTime(string filenameStem, Func<string, string> output)
    {
        var sections = new List<(DataSection, TimeSpan)>();

        var watch = new Stopwatch();
        var previousTime = new TimeSpan(0);
        DataSection previousSection = null;
        try
        {
            watch.Start();
            var pmm = PmmReadWithCallback(
                filenameStem,
                output,
                (section) =>
                {
                    var (current, dif) = getTime(sections, watch, previousTime, previousSection);
                    outTime(output, current, dif);

                    output("");
                    output(section.ToString());

                    previousSection = section;
                    previousTime = current;
                }
            );

            var (current, dif) = getTime(sections, watch, previousTime, previousSection);
            outTime(output, current, dif);

            return pmm;
        }
        finally
        {
            watch?.Stop();
            Console.WriteLine(string.Join(Environment.NewLine, sections));
        }

        static (TimeSpan current, TimeSpan dif) getTime(List<(DataSection, TimeSpan)> sections, Stopwatch watch, TimeSpan previousTime, DataSection previousSection)
        {
            var current = watch.Elapsed;
            var dif = current - previousTime;
            sections.Add((previousSection, dif));
            return (current, dif);
        }

        static void outTime(Func<string, string> output, TimeSpan current, TimeSpan dif)
        {
            output($"{"Time",-9}: {current} : {dif}");
        }
    }

    private static PolygonMovieMaker PmmReadWithCallback(string filenameStem, Func<string, string> valueOutput, PmmFileReader.OnSectionChangeEventHandler sectionOutput)
    {
        PmmFileReader.OnChangeSection += sectionOutput;

        // 履歴出力付きファイル読み込みオブジェクトを作成
        using var file = new FileStream(GetPath(filenameStem + ".pmm"), FileMode.Open);
        using var reader = new ObservableBinaryReader(file, Encoding.ShiftJIS);
        reader.OnRead += (value, type) =>
        {
            var prefix = $"{type.Name}";
            var mid = "| ";
            var makeString = (string value) => $"{prefix,-9}{mid}{value}";
            var _ = value switch
            {
                byte[] values => valueOutput(makeString(string.Join($", ", values))),
                char[] values => valueOutput(makeString(string.Join($", ", values))),
                _ => valueOutput(makeString(value.ToString())),
            };
        };

        // 読み込み
        valueOutput($"# {filenameStem}");
        var pmm = new PolygonMovieMaker();

        try
        {
            PmmFileReader.Read(reader, pmm);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            // 終了
            PmmFileReader.OnChangeSection -= sectionOutput;
        }

        return pmm;
    }
}
