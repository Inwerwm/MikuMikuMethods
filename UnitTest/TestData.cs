using MikuMikuMethods;
using MikuMikuMethods.Common;
using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmm.IO;

namespace UnitTest;

internal static class TestData
{
    public static readonly string TestDataDirectory = "../../TestData/";

    public static string GetPath(string filename) => Path.Combine(TestDataDirectory, filename);

    public static PolygonMovieMaker PmmLoggingRead(string filenameStem)
    {
        // 読み込み履歴出力の準備
        string logPath = GetPath(filenameStem + "_Readlog.txt");
        File.Delete(logPath);
        using var log = File.AppendText(logPath);
        var sections = new List<DataSection>();

        var output = (string message) =>
        {
            log.WriteLine(message);
            // switch 式を使うために値を返すようにしてる
            return message;
        };
        void onChangeSection(DataSection section)
        {
            output("");
            output(section.ToString());
            sections.Add(section);
        }
        PmmFileReader.OnChangeSection += onChangeSection;

        // 履歴出力付きファイル読み込みオブジェクトを作成
        using var file = new FileStream(GetPath(filenameStem + ".pmm"), FileMode.Open);
        using var reader = new BinaryReaderWithEvent(file, Encoding.ShiftJIS);
        reader.OnRead += (value, type) =>
        {
            var prefix = $"{type.Name}";
            var mid = "| ";
            var makeString = (string value) => $"{prefix,-9}{mid}{value}";
            var _ = value switch
            {
                byte[] values => output(makeString(string.Join($", ", values))),
                char[] values => output(makeString(string.Join($", ", values))),
                _ => output(makeString(value.ToString())),
            };
        };

        // 読み込み
        output($"# {filenameStem}");
        var pmm = new PolygonMovieMaker();

        try
        {
            PmmFileReader.Read(reader, pmm);
        }
        catch (Exception)
        {
            Console.WriteLine(string.Join(Environment.NewLine, sections));
            throw;
        }

        // 終了
        PmmFileReader.OnChangeSection -= onChangeSection;

        return pmm;
    }
}
