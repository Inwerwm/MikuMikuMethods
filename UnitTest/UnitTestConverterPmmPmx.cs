using Microsoft.VisualStudio.TestTools.UnitTesting;
using MikuMikuMethods.Converter;
using MikuMikuMethods.Pmm;
using MikuMikuMethods.Pmx;

namespace UnitTest;
[TestClass]
public class UnitTestConverterPmmPmx
{
    [TestMethod]
    public void ModelAddingTest()
    {
        PolygonMovieMaker pmm = new(TestData.GetPath("Blank.pmm"));
        
        string modelPath = TestData.GetPath("PlaneAndIK.pmx");
        PmxModel model = new PmxModel(modelPath);

        pmm.Models.Add(model.ToPmmModel(modelPath));

        pmm.Write(TestData.GetPath("AddModelByProgram.pmm"));

        TestData.PmmLoggingRead("AddModelByProgram", false);
        TestData.PmmLoggingRead("OneModel", false);
    }

    [TestMethod]
    public void ComplexModelAddingTest()
    {
        PolygonMovieMaker pmm = new(TestData.GetPath("Blank.pmm"));

        string modelPath = TestData.GetPath("ツインテ少女.pmx");
        PmxModel model = new PmxModel(modelPath);

        pmm.Models.Add(model.ToPmmModel(modelPath));

        pmm.Write(TestData.GetPath("AddComplexModelByProgram.pmm"));
    }
}
