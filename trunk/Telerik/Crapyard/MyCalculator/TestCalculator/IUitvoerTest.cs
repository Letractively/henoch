using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCalculator;
namespace TestCalculator
{
    
    
    /// <summary>
    ///This is a test class for IUitvoerTest and is intended
    ///to contain all IUitvoerTest Unit Tests
    ///</summary>
    [TestClass]
    public class UitvoerTest
    {


        private TestContext TestContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return TestContextInstance;
            }
            set
            {
                TestContextInstance = value;
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


        internal virtual void CreateIUitvoer<TCalculator>()
            where TCalculator : ICalculator, new()
        {
            // TODO: Instantiate an appropriate concrete class.
            IOutput target = new Output<TCalculator>();
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.ReadInput();
// ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.AreEqual(expected, actual);
// ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        /// <summary>
        ///A test for ReadInput
        ///</summary>
        [TestMethod]
        public void LeesInvoerTest()
        {
            CreateIUitvoer<Calculator>();
            CreateIUitvoer<TertaireStelsel>();
            ///Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
