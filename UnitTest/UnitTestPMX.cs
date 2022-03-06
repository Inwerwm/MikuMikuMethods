using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.Pmx;

namespace UnitTest;

[TestClass]
public class UnitTestPMX
{
    [TestMethod]
    public void PmxIOTest()
    {
        const string testDataDir = "../../TestData/";

        IOCompare("test.pmx", "write.pmx");
        IOCompare("tex.pmx", "texW.pmx");
        IOCompare("ツインテ少女.pmx", "ツインテ少女_w.pmx");
        IOCompare("ブレマートン.pmx", "ブレマートン_w.pmx");

        void IOCompare(string inFilename, string outFilename)
        {
            string i = Path.Combine(testDataDir, inFilename);
            string o = Path.Combine(testDataDir, outFilename);

            PmxModel m = new(i);
            m.Write(o);
            CompareBinary(i, o);
        }
    }

    private void CompareBinary(string filePath1, string filePath2)
    {
        var b1 = File.ReadAllBytes(filePath1);
        var b2 = File.ReadAllBytes(filePath2);

        if (b1.Length != b2.Length)
            Assert.Fail("バイナリデータの長さが違います。");

        foreach (var b in b1.Zip(b2).Select((Value, Index) => (Value, Index)))
        {
            Assert.AreEqual(b.Value.First, b.Value.Second, $"{b.Index:x}番目で異なる値が発見されました。");
        }
    }

    [TestMethod]
    public void PmxSharedToonTextureTest()
    {
        var path = new PmxTexture("toon05.bmp");
        var num = new PmxTexture(5);
        var notShared = new PmxTexture("tex/toon05.bmp");

        Assert.AreEqual((byte)5, path.ToonIndex);
        Assert.AreEqual("toon05.bmp", num.Path);
        Assert.IsNull(notShared.ToonIndex);
        Assert.AreEqual(1, new[] { path, num }.Distinct().Count());
    }
}
