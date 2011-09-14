using System.Collections.Generic;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.DataResource;
using MetaData.BeheerThemas.Interface.DataResource;
using MetaData.BeheerThemas.Services;
using MetaData.BeheerThemas.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaData.BeheerThemas.Tests
{
    /// <summary>
    /// Summary description for BeheerThemasModuleInitializerFixture
    /// </summary>
    [TestClass]
    public class BeheerThemasModuleControllerFixture
    {
        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void HaalThemasOpTest()
        {
            //Arrange
            var service = new MockBeheerThemasService();

            //Act
            var controller = new BeheerThemasController(service);
            IList<Thema> actual = controller.GetThemaTable();
            var expected = service.GetThemaTable();

            //Assert
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);

        }
    }
}