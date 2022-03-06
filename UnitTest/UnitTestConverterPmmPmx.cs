﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        PolygonMovieMaker pmm2 = new(TestData.GetPath("OneModel.pmm"));

        pmm.Write(TestData.GetPath("AddModelByProgram.pmm"));
    }
}
