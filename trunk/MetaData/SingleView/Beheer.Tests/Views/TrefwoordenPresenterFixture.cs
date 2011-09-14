using System.Text;
using Beheer.BusinessObjects.Dictionary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaData.Beheer.Views;

namespace MetaData.Beheer.Tests
{
    /// <summary>
    /// Summary description for TrefwoordenPresenterTestFixture
    /// </summary>
    [TestClass]
    public class TrefwoordenPresenterTestFixture
    {
        public TrefwoordenPresenterTestFixture()
        {
        }

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
        public void OnViewLoadedSetsBusinessObjectenIntheView()
        {
            //Arrange            
            var controller = new MockTrefwoordController();
            var beheerObject = new BeheerContextEntity { DataKeyValue = "test1" };
            beheerObject.Details.Add(new BeheerContextEntity
                                         {
                                             DataKeyValue = "Test-Detail"
                                         });
            controller.MockBusinessentity.Add(beheerObject);
            controller.AddDetail(beheerObject.Details[0]);

            var presenter = new TrefwoordenPresenter(controller);
            var view = new MockTrefwoordenView();
            presenter.View = view;

            //Act            
            presenter.OnViewLoaded();

            //Assert
            Assert.AreEqual(1, view.BusinessEntities.Count);
            Assert.AreSame(beheerObject, view.BusinessEntities[view.BusinessEntities.Count - 1]);

            Assert.AreEqual(beheerObject.Details, view.Details);
        }
    }
}

