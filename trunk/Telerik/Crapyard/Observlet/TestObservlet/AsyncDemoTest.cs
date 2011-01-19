using System.Collections.Generic;
using ApplicationTypes.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using Rhino.Mocks;

namespace TestObservlet
{
    
    
    /// <summary>
    ///This is a test class for AsyncDemoTest and is intended
    ///to contain all AsyncDemoTest Unit Tests
    ///</summary>
    [TestClass]
    [TestFixture]
    public class AsyncDemoTest
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
        //[TestFixtureSetUp]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //[TestFixtureTearDown]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //[SetUp]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //[TearDown]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
        
        /// <summary>
        ///A test for CreateDummyQueue
        ///</summary>
        [TestMethod]
        [Test]
        [DeploymentItem("ApplicationTypes.dll")]
        public void CreateDummyQueueTest()
        {
            AsyncDemo_Accessor target = new AsyncDemo_Accessor(); // TODO: Initialize to an appropriate value
            int maxValue = 1000; // TODO: Initialize to an appropriate value
            int sleep = 0; // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            actual = target.CreateDummyQueue(maxValue, sleep);
            Assert.AreEqual(1000, actual.Length);
            //Assert.Fail ("Verify the correctness of this test method.");
        }



    }
}
