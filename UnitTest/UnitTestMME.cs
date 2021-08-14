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
            /// ����

            // �e�X�g�f�[�^�̍쐬
            using (StreamWriter writer = new("TestData/EffectSettings.txt", false, Encoding.ShiftJIS))
            {
                writer.WriteLine(@"Default = none");
                writer.WriteLine(@"Pmd1 = ���f��1.fx");
                writer.WriteLine(@"Pmd2 = ���f��2.fx");
                writer.WriteLine(@"Pmd2.show = true");
                writer.WriteLine(@"Pmd4.show = false");
                writer.WriteLine(@"Pmd5 = ���f��5.fx");
                writer.WriteLine(@"Pmd5.show = false");
                writer.WriteLine(@"Pmd5[0] = ���f��5-0.fx");
                writer.WriteLine(@"Pmd5[1] = ���f��5-1.fx");
                writer.WriteLine(@"Pmd5[4] = ���f��5-4.fx");
                writer.WriteLine(@"Pmd5[4].show = false");
                writer.WriteLine(@"Pmd5[6] = ���f��5-6.fx");
                writer.WriteLine(@"Pmd5[6].show = true");
            }

            List<ObjectInfo> keys = new();
            keys.Add(new ModelInfo(1) { Path = "���f��1" });
            keys.Add(new ModelInfo(2) { Path = "���f��2" });
            keys.Add(new ModelInfo(3) { Path = "���f��3" });
            keys.Add(new ModelInfo(4) { Path = "���f��4" });
            keys.Add(new ModelInfo(5) { Path = "���f��5" });

            // �e�X�g�̂��߂̃C���X�^���X�𐶐�
            EffectSettings target = new(keys, EffectCategory.Effect);

            /// �e�X�g���s

            // �e�X�g�f�[�^��ǂݍ���
            using (StreamReader reader = new("TestData/EffectSettings.txt", Encoding.ShiftJIS))
                target.Read(reader);
            // �e�X�g��������
            using (StreamWriter writer = new("TestData/EffectSettings_Result.txt", false, Encoding.ShiftJIS))
                target.Write(writer);

            /// ����

            // �������݌��ʂ�ǂݍ���
            using (StreamReader reader = new("TestData/EffectSettings_Result.txt", Encoding.ShiftJIS))
            {
                Assert.AreEqual(@"Default = none", reader.ReadLine());
                Assert.AreEqual(@"Pmd1 = ���f��1.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd2 = ���f��2.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd2.show = true", reader.ReadLine());
                Assert.AreEqual(@"Pmd4.show = false", reader.ReadLine());
                Assert.AreEqual(@"Pmd5 = ���f��5.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5.show = false", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[0] = ���f��5-0.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[1] = ���f��5-1.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[4] = ���f��5-4.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[4].show = false", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[6] = ���f��5-6.fx", reader.ReadLine());
                Assert.AreEqual(@"Pmd5[6].show = true", reader.ReadLine());
            }
        }

        [TestMethod]
        public void Test_ProjectEffectSettings()
        {
            /// ����

            const string sourcePath = "TestData/���x�V�A_NumberNine.emm";
            const string resultPath = "TestData/Result.emm";

            string allSource;
            using (StreamReader reader = new(sourcePath, Encoding.ShiftJIS))
            {
                allSource = reader.ReadToEnd();
            }

            /// �e�X�g

            EffectMovieMaker emm = new();
            using (StreamReader reader = new(sourcePath, Encoding.ShiftJIS))
            {
                emm.Read(reader);
            }

            using (StreamWriter writer = new(resultPath, false, Encoding.ShiftJIS))
            {
                emm.Write(writer);
            }

            /// ����

            string allResult;
            using (StreamReader reader = new(resultPath, Encoding.ShiftJIS))
            {
                allResult = reader.ReadToEnd();
            }

            Assert.AreEqual(allSource, allResult);
        }
    }
}

