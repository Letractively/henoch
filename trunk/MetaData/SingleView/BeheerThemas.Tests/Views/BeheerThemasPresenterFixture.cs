using System;
using System.Text;
using System.Collections.Generic;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.DataResource;
using MetaData.BeheerThemas.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaData.BeheerThemas.Views;

namespace MetaData.BeheerThemas.Tests
{
    /// <summary>
    /// Summary description for BeheerThemasPresenterTestFixture
    /// </summary>
    [TestClass]
    public class BeheerThemasPresenterTestFixture
    {
        public BeheerThemasPresenterTestFixture()
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

        [Ignore]
        public void OnLoadBeheerThemasViewTest()
        {
            //Arrange
            var controller = new MockBeheerThemasController();            
            BeheerThemasPresenter presenter = new BeheerThemasPresenter(controller);
            MockBeheerThemasView view = new MockBeheerThemasView();

            //Act
            presenter.View = view;
            presenter.OnViewLoaded();

            //Assert
            Assert.AreEqual("thema-0",view.ThemaTable[0].ThemaNaam);
        }

        [TestMethod]
        public void OnViewLoadedSetsTransfersIntheView()
        {
            //Arrange
            var controller = new MockBeheerThemasController();
            Thema thema = new Thema{ThemaNaam="test1"};
            controller.MockThemas.Add(thema);
            BeheerThemasPresenter presenter = new BeheerThemasPresenter(controller);
            MockBeheerThemasView view = new MockBeheerThemasView();
            presenter.View = view;
            
            //Act            
            presenter.OnViewLoaded();

            //Assert
            Assert.AreEqual(1, view.ThemaTable.Count);
            Assert.AreSame(thema, view.ThemaTable[view.ThemaTable.Count-1]);
        }

        [TestMethod]
        public void OnThemaAddedCallsControlerAddThemaTest()
        {
            //Arrange
            var controller = new MockBeheerThemasController();
            Thema thema = new Thema { ThemaNaam = "added" };
            BeheerThemasPresenter presenter = new BeheerThemasPresenter(controller);
            MockBeheerThemasView view = new MockBeheerThemasView();
            
            //Act            
            presenter.OnThemasAdded(thema);
            
            //Asserts
            Assert.IsTrue(controller.AddThemaCalled);
            Assert.AreSame(thema, controller.MockThemas[0]);
        }
        [TestMethod]
        public void OnThemaDeletedCallsControlerDeleteThemaTest()
        {
            //Arrange
            var controller = new MockBeheerThemasController();
            Thema thema = new Thema { ThemaNaam = "deleted" };
            BeheerThemasPresenter presenter = new BeheerThemasPresenter(controller);
            MockBeheerThemasView view = new MockBeheerThemasView();

            //Act            
            presenter.OnThemasDeleted(thema);

            //Asserts
            Assert.IsTrue(controller.DeleteThemaCalled);
            Assert.AreSame(thema, controller.DeletedThema);
        }

        [TestMethod]
        public void OnThemaUpdatedCallsControlerUpdateThemaTest()
        {
            //Arrange
            var controller = new MockBeheerThemasController();
            Thema thema = new Thema { ThemaNaam = "Updated" };
            BeheerThemasPresenter presenter = new BeheerThemasPresenter(controller);
            MockBeheerThemasView view = new MockBeheerThemasView();

            //Act            
            presenter.OnThemasUpdated(thema);

            //Asserts
            Assert.IsTrue(controller.UpdateThemaCalled);
            Assert.AreSame(thema, controller.UpdatedThema);
        }
    }
}

