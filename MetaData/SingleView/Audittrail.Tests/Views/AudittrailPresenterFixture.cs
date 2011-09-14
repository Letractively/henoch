using System;
using System.Text;
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetaData.Audittrail.Views;

namespace MetaData.Audittrail.Tests
{
    /// <summary>
    /// Summary description for AudittrailPresenterTestFixture
    /// </summary>
    [TestClass]
    public class AudittrailPresenterTestFixture
    {
        public AudittrailPresenterTestFixture()
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
    }

    class MockAudittrailView : IAudittrailView
    {
        #region Implementation of IAudittrailView

        public IList<AuditItem> BusinessEntities
        {
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}

