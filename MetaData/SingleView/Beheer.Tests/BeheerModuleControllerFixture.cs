using System;
using System.Collections.Generic;
using System.Linq;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Tests.Mocks;
using Microsoft.FSharp.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockDataResource=MetaData.Beheer.Tests.Mocks.MockDataResource;
using MockThemasService=MetaData.Beheer.Tests.Mocks.MockThemasService;
using MetaData.Beheer.Views;

namespace MetaData.Beheer.Tests
{
    /// <summary>
    /// Summary description for BeheerModuleInitializerFixture
    /// </summary>
    [TestClass]
    public class BeheerModuleControllerFixture
    {
        public BeheerModuleControllerFixture()
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
            var controller = new MockBeheerController();

            //Act            
            var businessObject = new BeheerContextEntity { DataKeyValue = "test1" };
            controller.MockBusinessentity.Add(businessObject);
            var presenter = new ThemasPresenter(controller);
            var view = new MockThemasView();
            presenter.View = view;

            //Act            
            presenter.OnViewLoaded();

            //Assert
            Assert.AreEqual(1, view.BusinessEntities.Count);
            Assert.AreSame(businessObject, view.BusinessEntities[view.BusinessEntities.Count - 1]);
        }
        [TestMethod]
        public void HaalThemasOpViaServiceTest()
        {
            //Arrange
            var service = new MockThemasService();

            //Act
            var controller = new BeheerController(service);
            IList<BeheerContextEntity> actual = controller.GetEntities();
            var expected = service.GetEntities();

            //Assert
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);
            Assert.AreEqual(expected[0].DataKeyValue, actual[0].DataKeyValue);
            Assert.AreEqual(expected[1].DataKeyValue, actual[1].DataKeyValue);
            Assert.AreEqual(expected[0].Id, actual[0].Id);
            Assert.AreEqual(expected[1].Id, actual[1].Id);

        }

        [TestMethod]
        public void HaalTrefwoordenOpViaServiceTest()
        {
            //Arrange
            var service = new MockTrefwoordService();

            //Act
            var controller = new BeheerController(service);
            IList<BeheerContextEntity> actual = controller.GetEntities();
            var expected = service.GetEntities();

            //Assert
            Assert.AreEqual(expected[0], actual[0]);
            Assert.AreEqual(expected[1], actual[1]);

        }
        
        [TestMethod]
        public void TuplesTest()
        {
            //Arrange
            DateTime nu = DateTime.Now;
            BeheerContextEntity thema = new BeheerContextEntity();

            //2-way roundtrip
            //setters
            thema.Id = 1;
            thema.DataKeyValue = "themanaam-";
            thema.Attributes.Add("Kolom-datetimetype", new AttributeValue { ValueType = nu });
            thema.Attributes.Add("Kolom-doubletype", new AttributeValue { ValueType = 1.4d});
            thema.Attributes.Add("Kolom-stringtype", new AttributeValue { ValueStringType = "blah" });
            
            //getters
            var roundTrippedId = thema.Id;
            var roundTrippedName = thema.DataKeyValue;
            var roundTrippedList = thema.Attributes;

            //Assert
            Assert.AreEqual(thema.Id, roundTrippedId);
            Assert.AreEqual(thema.DataKeyValue, roundTrippedName);
            Assert.AreEqual(thema.Attributes, roundTrippedList);
        }

        [TestMethod]
        public void ThemaToBusinessTest()
        {            
            //Arrange
            IThema thema = new Thema { DataKeyValue = "thema-1", Id = 1 }; ;

            //Act
            //ObjectContainerDatasource:
            BeheerContextEntity entity = thema as BeheerContextEntity;

            //Assert
            Assert.AreEqual(thema, entity);

        }

        [TestMethod]
        public void ThemaToIBusinessTest()
        {
            IThema thema = new Thema { DataKeyValue = "thema-1", Id = 1 }; ;
            //ObjectContainerDatasource:
            IBeheerContextEntity entity = thema as BeheerContextEntity;

            Assert.AreEqual(thema, entity);
        }
        [TestMethod]
        public void BusinessToThemaTest()
        {
            IBeheerContextEntity entity = new BeheerContextEntity { DataKeyValue = "thema-1", Id = 1 }; ;
            //ObjectContainerDatasource:
            IThema thema = entity as IThema;

            Assert.AreNotSame(thema, entity);
        }
        [TestMethod]
        public void ThemaListToBusinessListTest()
        {
            var themas = new MockDataResource().GetThemaTableStub();
            //ObjectContainerDatasource:
            IList<BeheerContextEntity> entities = new List<BeheerContextEntity>();

            foreach (var thema in themas)
            {
                entities.Add(thema);
            }

            Assert.AreEqual(themas.Count, entities.Count);
        }
    }

    public class MockThemasView : IBusinessEntityView
    {
        public IList<BeheerContextEntity> BusinessEntities { set; get; }
        public IList<BeheerContextEntity> DetailsEntities
        {
            set { throw new NotImplementedException(); }
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

        public bool IsSortable
        {
            set { throw new NotImplementedException(); }
        }

        public bool AllowCrud
        {
            set { throw new NotImplementedException(); }
        }

        public BeheerContextEntity Selected
        {
            set { throw new NotImplementedException(); }
        }

        public bool ShowFooter
        {
            set { throw new NotImplementedException(); }
        }

        public void ShowErrorMessage(string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
