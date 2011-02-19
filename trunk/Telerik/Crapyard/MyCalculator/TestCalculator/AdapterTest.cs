
using MathNet.Numerics.Distributions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DesignPatterns;

namespace TestCalculator
{
    
    
    /// <summary>
    ///This is a test class for AdapterTest and is intended
    ///to contain all AdapterTest Unit Tests
    ///</summary>
    [TestClass]
    public class AdapterTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

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
        [TestCleanup]
        public void MyTestCleanup()
        {
            ///TestContext.WriteLine(LoaderEx);
        }
        
        #endregion


        /// <summary>
        ///A test for NormCdf
        ///</summary>
        [TestMethod]
        public void NormCdfTest()
        {
            new NormalDistribution(0, 1);
            //matlabResult = 0.682689492137086;

            Adaptee adaptee = new Adaptee(); 
            Adapter target = new Adapter(adaptee); 
            Func<double, double> expected = adaptee.NormCdf; 
            Func<double, double> actual;
            actual = target.NormCdf;
            Assert.AreNotEqual(expected, actual,  "Adaptee should be adapted.") ;

            Func<double, double> otherFunction = arg =>
                                             {
                                                 arg = arg - 1;
                                                 return arg;
                                             };
            Assert.AreNotEqual(expected, otherFunction);
            ///Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Adapter Constructor
        ///</summary>
        [TestMethod]
        public void AdapterConstructorAdapteeTest()
        {
            Adaptee adaptee = new Adaptee();
            Adapter target = new Adapter(adaptee);

            //Assert.IsFalse(target.Distributions is Distributions);
            //Assert.AreEqual(target.Distributions , new Distributions());
           
            Func<double, double> expected = adaptee.NormCdf;
            Func<double, double> actual = target.NormCdf;
            Assert.AreNotEqual(expected(1), actual(1), "Should NOT be equal function behaviour.");
            

            Func<double, double> otherFunction = arg =>
            {
                arg = arg - 1;
                return arg;
            };
            Assert.AreNotEqual(expected, otherFunction);
        }

        /// <summary>
        ///A test for Adapter Constructor
        ///</summary>
        [TestMethod]
        public void AdapterConstructorTargetTest()
        {
            Distributions target1 = new Distributions();
            Adapter target = new Adapter(target1);
            Adaptee adaptee = new Adaptee();
            NormalDistribution normCdf = new NormalDistribution(0, 1);

            Func<double, double> expected = target1.NormCdf;
            Func<double, double> actual = target.NormCdf;
            
            Assert.AreNotEqual(expected(1), adaptee.NormCdf(1), "Should NOT be equal function behaviour.");
            Assert.AreEqual(expected(1), actual(1), "Should be equal function behaviour.");
            Assert.AreEqual( normCdf.CumulativeDistribution(1), actual(1), "Should be equal function behaviour.");

            Func<double, double> normCdf2 = arg =>
            {
                arg = arg - 1;
                return arg;
            };
            Assert.AreNotEqual(expected, normCdf2);
        }
        [TestMethod]
        public void AdapterGetDistribTest()
        {
            Distributions target1 = new Distributions();
            Adapter target = new Adapter(target1);
            ///Adaptee adaptee = new Adaptee();

            Assert.AreEqual(target.NormCdf(1), target1.NormCdf(1), 1.0E-8);

            target = new Adapter();
            Assert.AreEqual(1, target.NormCdf(1), 1.0E-8);
        }
    }

}
