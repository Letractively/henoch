using Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Dictionary.BusinessObjects;

namespace TestRepo
{
    
    
    /// <summary>
    ///This is a test class for ShareHoldersTest and is intended
    ///to contain all ShareHoldersTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ShareHoldersTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AddShareHolders
        ///</summary>
        [Ignore()]
        public void AddShareHoldersTest()
        {
            ShareHolders target = new ShareHolders(true); 
            
            Assert.AreEqual(2, target.GetSubsidiaries("Ahold").Count);

            Assert.AreEqual(2,target.GetShareHolders("123").Count);
            Assert.AreEqual("Ahold", target.GetShareHolders("123")[0]);
            Assert.AreEqual("Unilever", target.GetShareHolders("123")[1]);

            Assert.AreNotEqual(null, target.GetShareHolders("Ahold"));
            Assert.AreEqual(0, target.GetShareHolders("Ahold").Count);
        }
    }
}
