using MyCalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestCalculator
{
    
    
    /// <summary>
    ///This is a test class for NumeriekeModuleTest and is intended
    ///to contain all NumeriekeModuleTest Unit Tests
    ///</summary>
    [TestClass]
    public class NumeriekeModuleTest
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


        /// <summary>
        /// A test for MathFunctions`1 Constructor covering all Interfaces.
        /// This holds invariants for construction.
        ///</summary>
        public void InvariantsForFactoryTest<TCalculator>()
            where TCalculator : ICalculator, new()
        {
            MathFunctions<TCalculator> target = new MathFunctions<TCalculator>();
            ///Assert.Inconclusive("TODO: Implement code to verify target");
            Assert.IsTrue(target.Add(1,1)==2);
        }

        [TestMethod]
        public void NumeriekeModuleConstructorTest()
        {
            //Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of Cal" +
            //        "culator. Please call InvariantsForFactoryTest<TCalculator>() with app" +
            //        "ropriate type parameters.");
            InvariantsForFactoryTest<Calculator>();
            InvariantsForFactoryTest<TertaireStelsel>();
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        public void TelOpTestHelper<TCalculator>()
            where TCalculator : ICalculator, new()
        {
            MathFunctions<TCalculator> target = new MathFunctions<TCalculator>(); // TODO: Initialize to an appropriate value
            double x = 0F; // TODO: Initialize to an appropriate value
            double y = 0F; // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            actual = target.Add(x, y);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [Ignore]
        public void TelOpTest()
        {
            Assert.Inconclusive("No appropriate type parameter is found to satisfies the type constraint(s) of Cal" +
                    "culator. Please call TelOpTestHelper<TCalculator>() with appropriate type paramet" +
                    "ers.");
        }
    }


}
