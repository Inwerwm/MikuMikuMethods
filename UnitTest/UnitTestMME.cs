using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods;
using MikuMikuMethods.MME;
using System.Collections.Generic;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class UnitTestMME
    {
        [TestMethod]
        public void Test_EffectSettings()
        {
            /// 準備

            // テストデータの作成
            using (StreamWriter writer = new("TestData/EffectSettings.txt", false, Encoding.ShiftJIS))
            {
                writer.WriteLine(@"Default = none");
                writer.WriteLine(@"Pmd1 = モデル1.fx");
                writer.WriteLine(@"Pmd2 = モデル2.fx");
                writer.WriteLine(@"Pmd2.show = true");
                writer.WriteLine(@"Pmd4.show = false");
                writer.WriteLine(@"Pmd5 = モデル5.fx");
                writer.WriteLine(@"Pmd5.show = false");
                writer.WriteLine(@"Pmd5[0] = モデル5-0.fx");
                writer.WriteLine(@"Pmd5[1] = モデル5-1.fx");
                writer.WriteLine(@"Pmd5[4] = モデル5-4.fx");
                writer.WriteLine(@"Pmd5[4].show = false");
                writer.WriteLine(@"Pmd5[6] = モデル5-6.fx");
                writer.WriteLine(@"Pmd5[6].show = true");
            }

            List<ObjectInfo> keys = new();
            keys.Add(new ModelInfo(1) { Path = "モデル1" });
            keys.Add(new ModelInfo(2) { Path = "モデル2" });
            keys.Add(new ModelInfo(3) { Path = "モデル3" });
            keys.Add(new ModelInfo(4) { Path = "モデル4" });
            keys.Add(new ModelInfo(5) { Path = "モデル5" });

            // テストのためのインスタンスを生成
            EffectSettings target = new(keys, EffectCategory.Effect);

            /// テスト実行

            // テストデータを読み込み
            using (StreamReader reader = new("TestData/EffectSettings.txt", Encoding.ShiftJIS))
                target.Read(reader);
            // テスト書き込み
            using (StreamWriter writer = new("TestData/EffectSettings_Result.txt", false, Encoding.ShiftJIS))
                target.Write(writer);

            /// 結果

            // 書き込み結果を読み込み
            using (StreamReader reader = new("TestData/EffectSettings_Result.txt", Encoding.ShiftJIS))
            {
                Assert.AreEqual(@"Default = none", reader.ReadLine());
                Assert.AreEqual(@"Pmd1 = モデル1.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd2 = モデル2.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd2.show = true", reader.ReadLine());
                Assert.AreEqual(@"Pmd4.show = false", reader.ReadLine());
                Assert.AreEqual(@"Pmd5 = モデル5.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5.show = false", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[0] = モデル5-0.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[1] = モデル5-1.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[4] = モデル5-4.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[4].show = false", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[6] = モデル5-6.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[6].show = true", reader.ReadLine());
            }
        }

        [TestMethod]
        public void Test_ProjectEffectSettings()
        {
            /// 準備

            const string sourcePath = "TestData/ルベシア_NumberNine.emm";
            const string resultPath = "TestData/Result.emm";

            string allSource;
            using (StreamReader reader = new(sourcePath, Encoding.ShiftJIS))
            {
                allSource = reader.ReadToEnd();
            }

            /// テスト

            EffectMovieMaker emm = new();
            using (StreamReader reader = new(sourcePath, Encoding.ShiftJIS))
            {
                emm.Read(reader);
            }

            using (StreamWriter writer = new(resultPath, false, Encoding.ShiftJIS))
            {
                emm.Write(writer);
            }

            /// 結果

            string allResult;
            using (StreamReader reader = new(resultPath, Encoding.ShiftJIS))
            {
                allResult = reader.ReadToEnd();
            }

            Assert.AreEqual(allSource, allResult);
        }
    }
}

