// <copyright file="MyMathTest.cs" company="">Copyright ©  2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrap;

namespace Scrap
{
    /// <summary>This class contains parameterized unit tests for MyMath</summary>
    [PexClass(typeof(MyMath))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class MyMathTest
    {
        /// <summary>Test stub for Fac(Int32)</summary>
        [PexMethod, PexAllowedException(typeof(OverflowException))]
        public int Fac(int i)
        {
            int result = MyMath.Fac(i);
            return result;
            // TODO: add assertions to method MyMathTest.Fac(Int32)
        }
        [PexMethod (MaxBranches=2000)]            
        public void PowTest(double x, double y)
        {

            double[][] items = new double[][] {};
            Predicate<double[]> predicate = delegate(double[] scalars)
                                              {
                                                 return true;                                          
                                              };
            PexAssert.TrueForAll<double[]>(items, predicate);

        }

        [TestMethod]
        public void ArrayTest()
        {
            double[][] args = new double[][] {new double[]{1},new double[]{2}};

        }
        [PexMethod]
        public void AddTest(int a, int b)
        {
            int i = MyMath.Add(a, b);
            Assert.AreEqual(a + b, i);
        }

        [PexMethod]
        public void SubTest(int a, int b)
        {
            int i = MyMath.Sub(a, b);
            Assert.AreEqual(a - b, i);
        }

        [TestMethod]
        public void SubTest()
        {
            int i = MyMath.Sub(1, 1);
            Assert.AreEqual(0, i);
        }
        [TestMethod]
        public void AddTest()
        {
            int i = MyMath.Add(1, 1);
            Assert.AreEqual(2, i);
        }
    }
}
