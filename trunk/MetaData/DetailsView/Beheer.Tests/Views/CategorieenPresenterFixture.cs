using System;
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Interface.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaData.Beheer.Views;
using Rhino.Mocks;

namespace MetaData.Beheer.Tests
{
    /// <summary>
    /// Summary description for CategorieenPresenterTestFixture
    /// </summary>
    [TestClass]
    public class CategorieenPresenterTestFixture
    {
        public CategorieenPresenterTestFixture()
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
        public void OnViewLoadedSetsCategorieIntheView()
        {
            // Mock the View
            MockRepository mocks = new MockRepository();
            //Arrange
            var service = mocks.DynamicMock<ICategorieService>();

            //Act
            var controller = mocks.DynamicMock<ICategorieController>();

            // Create Presenter With Mock View and Dummy BlogService
            CategorieService serviceActual = new MockCategorieService();
            var controllerActual = new CategorieController(serviceActual,null,null);
            var presenter = new CategorieenPresenter(controllerActual);
            var cat = new BeheerContextEntity { DataKeyValue = "added" };
            IList<BeheerContextEntity> list= new List<BeheerContextEntity>();
            list.Add(cat);
            //controller.AddBusinessEntity(cat);

            // Set Expectations  
            controller.Expect(action=>action.AddBusinessEntity(cat));
            //presenter.Expect(action => action.OnViewLoaded());
            
            
            // Tell Rhino Mocks We're Done Setting Expectations
            mocks.ReplayAll();
            presenter.OnViewLoaded();
            
            mocks.VerifyAll();


        }
        [TestMethod]
        public void OnViewLoadedSetsBusinessObjectenIntheView()
        {
            //Arrange            
            var controller = new MockCategorieController();
            var beheerObject = new BeheerContextEntity {  DataKeyValue = "test1" };
            controller.MockBusinessentity.Add(beheerObject);
            var presenter = new CategorieenPresenter(controller);
            var view= new MockCategorieenView();
            presenter.View = view;

            //Act            
            presenter.OnViewLoaded();

            //Assert
            Assert.AreEqual(1, view.BusinessEntities.Count);
            Assert.AreSame(beheerObject, view.BusinessEntities[view.BusinessEntities.Count - 1]);
        }
        //[TestMethod]
        //public void OnThemaAddedCallsControlerAddThemaTest()
        //{
        //    //Arrange
        //    var controller = new MockBeheerThemasController();
        //    Thema thema = new Thema { ThemaNaam = "added" };
        //    BeheerThemasPresenter presenter = new BeheerThemasPresenter(controller);
        //    MockBeheerThemasView view = new MockBeheerThemasView();

        //    //Act            
        //    presenter.OnThemasAdded(thema);

        //    //Asserts
        //    Assert.IsTrue(controller.AddThemaCalled);
        //    Assert.AreSame(thema, controller.MockThemas[0]);
        //}
        //[TestMethod]
        //public void OnThemaDeletedCallsControlerDeleteThemaTest()
        //{
        //    //Arrange
        //    var controller = new MockBeheerThemasController();
        //    Thema thema = new Thema { ThemaNaam = "deleted" };
        //    BeheerThemasPresenter presenter = new BeheerThemasPresenter(controller);
        //    MockBeheerThemasView view = new MockBeheerThemasView();

        //    //Act            
        //    presenter.OnThemasDeleted(thema);

        //    //Asserts
        //    Assert.IsTrue(controller.DeleteThemaCalled);
        //    Assert.AreSame(thema, controller.DeletedThema);
        //}

        //[TestMethod]
        //public void OnThemaUpdatedCallsControlerUpdateThemaTest()
        //{
        //    //Arrange
        //    var controller = new MockBeheerThemasController();
        //    Thema thema = new Thema { ThemaNaam = "Updated" };
        //    BeheerThemasPresenter presenter = new BeheerThemasPresenter(controller);
        //    MockBeheerThemasView view = new MockBeheerThemasView();

        //    //Act            
        //    presenter.OnThemasUpdated(thema);

        //    //Asserts
        //    Assert.IsTrue(controller.UpdateThemaCalled);
        //    Assert.AreSame(thema, controller.UpdatedThema);
        //}

        [Ignore]
        public void MockTestPattern()
        {
            // Prepare mock repository
            MockRepository mocks = new MockRepository();
            ICategorieenView view = mocks.StrictMock<ICategorieenView>();

            // Record expectations
            using (mocks.Record())
            {
                //Expect..Action/Call/On  
                
                //voids
                view.SomeHandler(1);
                LastCall.On(view).Repeat.Twice();

            }

            // Replay and validate interaction
            object result = null;
            using (mocks.Playback())
            {
                //IComponent underTest = new ComponentImplementation(dependency);
                //result = underTest.TestMethod();
            }

            // Post-interaction assertions
            Assert.IsNull(result);

        }
        [TestMethod]
        public void TestPatternCallVoids()
        {
            //Arrange:
            // Prepare mock repository
            var mocks = new MockRepository();
            var controller = mocks.Stub<ICategorieController>();
            
            var presenter = new CategorieenPresenter(controller);
            ICategorieenView view = mocks.Stub<ICategorieenView>();
            presenter.View = view;

            //Act:
            // Record expectations/actions/calls 
            using (mocks.Record())
            {
                //Expect..Action/Call/On  
                
                ////voids
                view.SomeHandler(1);
                LastCall.On(view).Repeat.Times(10);

                //TODO: meer instellingen om het gedrag te 'recorden'.
            }
           
            //Assert:            
            // Replay and validate interaction
            object result = null;
            using (mocks.Playback())
            {
                //TODO: testmethods zoals in:
                //IComponent underTest = new ComponentImplementation(dependency);
                //result = underTest.TestMethod();
            }
            // Post-interaction assertions
            Assert.IsNull(result);

            //note: als PlayBack niet voldoende is : 
            mocks.ReplayAll();
            //TODO: testmethods zoals in:
            //IComponent underTest = new ComponentImplementation(dependency);
            //result = underTest.TestMethod();
            //Assert.IsNull(result);
            mocks.VerifyAll();
        }
    }

    public class MockCategorieService : CategorieService
    {

    }

    public class MockCategorieController : CategorieController
    {
        public MockCategorieController()
        {
            MockBusinessentity = new List<BeheerContextEntity>();
        }

        private IList<BeheerContextEntity> m_Entities;
        public List<BeheerContextEntity> MockBusinessentity { get; set; }
        public override IList<BeheerContextEntity> GetEntities()
        {
            m_Entities = MockBusinessentity;
            return MockBusinessentity;
        }
        public override BeheerContextEntity Selected
        {
            get
            {
                return null;
            }
            set
            {
                ;
            }
        }
    }

    class MockCategorieenView : ICategorieenView
    {
        public IList<BeheerContextEntity> BusinessEntities { get; set; }
        public IList<BeheerContextEntity> DetailsEntities
        {
            set { throw new NotImplementedException(); }
        }

        public IList<BeheerContextEntity> VisualDetails
        {
            set { throw new NotImplementedException(); }
        }

        public bool ShowFooter { get; set; }
        public void ShowErrorMessage(string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool IsMasterView
        {
            set { throw new NotImplementedException(); }
        }

        public bool IsInsertingInline
        {
            set { throw new NotImplementedException(); }
        }

        public BeheerContextEntity Master
        {
            set { throw new NotImplementedException(); }
        }

        public bool AllowCrud
        {
            set { throw new NotImplementedException(); }
        }

        public BeheerContextEntity Selected { get; set; }
        
        public bool IsVisibleInsert { get; set; }

        public void SomeHandler(int myPolicy)
        {
            throw new NotImplementedException();
        }

        public bool Ready
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}

