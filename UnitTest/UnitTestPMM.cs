using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.PMM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public class UnitTestPMM
    {
        PolygonMovieMaker testPmm;

        public UnitTestPMM()
        {
            testPmm = new();
        }

        [TestMethod]
        public void Test_IO()
        {
            using (FileStream stream = new("TestData/testProject.pmm", FileMode.Open))
            using (BinaryReader reader = new(stream, MikuMikuMethods.Encoding.ShiftJIS))
            using (FileStream outStream = new("TestData/output.pmm", FileMode.Create))
            using (BinaryWriter writer = new(outStream, MikuMikuMethods.Encoding.ShiftJIS))
            {
                testPmm.Read(reader);
                testPmm.Write(writer);

                // 比較のため巻き戻し
                reader.BaseStream.Position = 0;
                writer.BaseStream.Position = 0;

                using(BinaryReader outReader = new(outStream, MikuMikuMethods.Encoding.ShiftJIS))
                {
                    var outPmm = new PolygonMovieMaker(outReader);

                    //もとのPMMと書込読込PMMのインスタンスをシリアライズして
                    //テキストで差分を見れるようにする
                    using (FileStream inJson = new("TestData/originPmm.json", FileMode.Create))
                    using(FileStream outJson = new("TestData/outputPmm.json", FileMode.Create))
                    {
                        DataContractJsonSerializer serializer = new(typeof(PolygonMovieMaker));
                        serializer.WriteObject(inJson, testPmm);
                        serializer.WriteObject(outJson, outPmm);
                    }

                        //Assert.AreEqual(reader.BaseStream.Length, outReader.BaseStream.Length, "入力ファイルと出力ファイルの長さが異なっている。");
                        for (int i = 0; reader.BaseStream.Position < reader.BaseStream.Length; i++)
                    {
                        Assert.AreEqual(reader.ReadByte(), outReader.ReadByte(), $"{i} byte目で齟齬が発生");
                    }
                }
            }
        }
    }
}
