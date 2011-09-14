using MetaData.BeheerThemas.Services;
using MetaData.BeheerThemas.BusinessEntities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;
using MetaData.BeheerThemas.DataResource;
// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
namespace MetaData.BeheerThemas
{
    public partial class BeheerThemasControllerTest
    {
[TestMethod]
[PexGeneratedBy(typeof(BeheerThemasControllerTest))]
public void BeheerThemasServiceGet01()
{
    MockBeheerThemasService mockBeheerThemasService;
    BeheerThemasController beheerThemasController;
    Thema s0 = new Thema();
    s0.Id = 0L;
    s0.ThemaNaam = (string)null;
    mockBeheerThemasService = MockBeheerThemasServiceFactory.Create(s0);
    beheerThemasController = BeheerThemasControllerFactory.Create
        ((IBeheerThemasService)mockBeheerThemasService, false, (Thema)null, 
        (Thema)null, false, false);
    this.BeheerThemasServiceGet(beheerThemasController);
    Assert.IsNotNull((object)beheerThemasController);
    Assert.IsNull(beheerThemasController.ThemaTable);
    Assert.IsNotNull(beheerThemasController.BeheerThemasService);
    Assert.AreEqual<bool>(false, beheerThemasController.AddThemaCalled);
    Assert.IsNull(beheerThemasController.UpdatedThema);
    Assert.IsNull(beheerThemasController.DeletedThema);
    Assert.AreEqual<bool>(false, beheerThemasController.DeleteThemaCalled);
    Assert.AreEqual<bool>(false, beheerThemasController.UpdateThemaCalled);
}
[TestMethod]
[PexGeneratedBy(typeof(BeheerThemasControllerTest))]
public void BeheerThemasServiceGet02()
{
    BeheerThemasService beheerThemasService;
    BeheerThemasController beheerThemasController;
    Thema s0 = new Thema();
    s0.Id = 0L;
    s0.ThemaNaam = (string)null;
    beheerThemasService = BeheerThemasServiceFactory.Create(s0);
    beheerThemasController = BeheerThemasControllerFactory.Create
        ((IBeheerThemasService)beheerThemasService, false, (Thema)null, 
        (Thema)null, false, false);
    this.BeheerThemasServiceGet(beheerThemasController);
    Assert.IsNotNull((object)beheerThemasController);
    Assert.IsNull(beheerThemasController.ThemaTable);
    Assert.IsNotNull(beheerThemasController.BeheerThemasService);
    Assert.AreEqual<bool>(false, beheerThemasController.AddThemaCalled);
    Assert.IsNull(beheerThemasController.UpdatedThema);
    Assert.IsNull(beheerThemasController.DeletedThema);
    Assert.AreEqual<bool>(false, beheerThemasController.DeleteThemaCalled);
    Assert.AreEqual<bool>(false, beheerThemasController.UpdateThemaCalled);
}
    }
}
