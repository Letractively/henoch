using System;
using DataResource;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataResource.Metadata;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace TestObservlet
{
    
    
    /// <summary>
    ///This is a test class for MyAccessTest and is intended
    ///to contain all MyAccessTest Unit Tests
    ///</summary>
    [TestClass()]
    [TestFixture]
    public class MyAccessTest
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
        ///A test for GetDummyDataSet
        ///</summary>
        [Test]
        public void GetDummyDataSetTest()
        {
            MyAccess target = new MyAccess(); 
            IMetaDataSchema[] expected = null;
            IMetaDataSchema[] actual;
            actual = target.GetDummyDataSet();
            Assert.AreEqual(1000, actual.Length);

            int endLoop = actual.Length;
            for (int i = 0; i < endLoop; i++)
            {
                IMetaDataSchema schema = actual[i];
                Console.Out.WriteLine(schema.TableName);
            }
            
        }
    }
}
