using Maintenance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCalculator;

namespace TestCalculator
{
    /// <summary>
    /// Summary description for TestCalculator
    /// </summary>
    [TestClass]
    public class TestCalculator
    {
// ReSharper disable EmptyConstructor
        public TestCalculator()
// ReSharper restore EmptyConstructor
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        public void TestCreateACalculator()
        {
            Calculator eenCalculator = new Calculator();
            double bigInt = 1;
            Assert.AreEqual(bigInt, eenCalculator.Add(0, 1), "0 + 1 is in decimale Stelsel  1 ");
            
            ///Invariant introduceren...
            for (int i = 0; i < 1000; i++)///int.MaxValue=> te lang?
            {
                Assert.AreEqual((double)i + 1, eenCalculator.Add(i, 1));
            }

            #region simple calculator


            bigInt = 10;
            Assert.AreEqual(bigInt, eenCalculator.TelOp(new double[] { 1, 2, 3, 4 }));
            bigInt = 10;
            Assert.AreEqual(bigInt, eenCalculator.PersistResults());

            #endregion

            var eenComputer = new Computer<TertaireStelsel>();
            IMathFunctions andereCalculator = eenComputer.MathFunctions;
            IOutput anderUItvoer = eenComputer.Output;
            
            bigInt = 2;
            Assert.AreEqual(bigInt, andereCalculator.Add(1, 1), "1+1=2 ook in tertaire Stelsel.");
            Assert.AreEqual(bigInt, anderUItvoer.PersistResults());

            
            ///eenCalculator.GaInTertaireStelselModus();///???
            /// ///eenCalculator.GaInTertaireStelselModus();///???
            /// ///eenCalculator.GaInXStelselModus();///???
            /// ///eenCalculator.GaInTertaireStelselModus(X);///???
            /// ///eenCalculator.GaInStelselModus(Y);///???
            #region uitbreiding stelsel (functionaliteit) naar ander uitvoer.

            //var andereModus = new Computer<TertaireStelsel>();
            //IMathFunctions andereCalculator = andereModus.MathFunctions;
            //IOutput anderUItvoer = andereModus.Output;

            //bigInt = 1;
            //Assert.AreEqual(bigInt, andereCalculator.Add(1, 2), "review tertaire Stelsel.");
            //Assert.AreEqual(bigInt, anderUItvoer.PersistResults());

            //MathFunctions<TCalculator> calc = new MathFunctions<TCalculator>();
            
            //Assert.AreEqual(bigInt, andereCalculator.Add(353, 445), "(1242) review quartaire Stelsel.");
            
            //Assert.AreEqual("", eenCalculator.Add(0, 1), "review kwintaire Stelsel.");
            //Assert.AreEqual("", eenCalculator.Add(0, 1), "review sextaire Stelsel."); 


            #endregion

            #region Uitbreiding onbekende functionaliteit



            Log.ConsoleWriteline(ulong.MaxValue.ToString());
            Log.ConsoleWriteline(double.MaxValue.ToString());
            Log.ConsoleWriteline(long.MaxValue.ToString());
            double getal = 18446744073709551615 - 1;
            Log.ConsoleWriteline(getal.ToString());
            #endregion
            
        }
    }


}
