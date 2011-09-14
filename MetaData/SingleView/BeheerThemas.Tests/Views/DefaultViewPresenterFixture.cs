using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaData.BeheerThemas.Views;
using MetaData.BeheerThemas.Tests.Mocks;

namespace MetaData.BeheerThemas.Tests
{
    /// <summary>
    /// Summary description for DefaultViewPresenterTestFixture
    /// </summary>
    [TestClass]
    public class DefaultViewPresenterTestFixture
    {
        public DefaultViewPresenterTestFixture()
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

        // Eg.: this is a unit tests taken from the reference implementation
        //
        //[TestMethod]
        //public void OnViewLoadedCallsControllerGetTransfersAndSetsTransfersInView()
        //{
        //    MockElectronicFundsTransferController controller = new MockElectronicFundsTransferController();
        //    Transfer transfer = new Transfer();
        //    controller.Transfers = new Transfer[] { transfer };
        //    DefaultViewPresenter presenter = new DefaultViewPresenter(controller);
        //    MockDefaultView view = new MockDefaultView();
        //    presenter.View = view;

        //    presenter.OnViewLoaded();

        //    Assert.IsTrue(controller.GetTransfersCalled);
        //    Assert.IsTrue(view.TransfersSet);
        //    Assert.AreEqual(1, view.Transfers.Length);
        //    Assert.AreSame(transfer, view.Transfers[0]);
        //}
    }
    namespace Mocks
    {
        class MockDefaultView : IDefaultView
        {

        }
    }
}
