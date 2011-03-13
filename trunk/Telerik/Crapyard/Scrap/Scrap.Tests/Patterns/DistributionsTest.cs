// <copyright file="DistributionsTest.cs" company="">Copyright ©  2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrap.Patterns;

namespace Scrap.Patterns
{
    /// <summary>This class contains parameterized unit tests for Distributions</summary>
    [PexClass(typeof(Distributions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class DistributionsTest
    {
        /// <summary>Test stub for Estimate(Int32)</summary>
        [PexMethod]
        public string Estimate([PexAssumeUnderTest]Distributions target, int i)
        {
            string result = target.Estimate(i);
            return result;
            // TODO: add assertions to method DistributionsTest.Estimate(Distributions, Int32)
        }

        /// <summary>Test stub for NormCdf(Double)</summary>
        [PexMethod]
        public double NormCdf([PexAssumeUnderTest]Distributions target, double x)
        {
            double result = target.NormCdf(x);
            return result;
            // TODO: add assertions to method DistributionsTest.NormCdf(Distributions, Double)
        }
    }
}
