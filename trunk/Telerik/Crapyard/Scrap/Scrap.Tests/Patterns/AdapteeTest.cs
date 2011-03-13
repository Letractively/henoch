// <copyright file="AdapteeTest.cs" company="">Copyright ©  2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrap.Patterns;

namespace Scrap.Patterns
{
    /// <summary>This class contains parameterized unit tests for Adaptee</summary>
    [PexClass(typeof(Adaptee))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class AdapteeTest
    {
        /// <summary>Test stub for NormCdf(Double)</summary>
        [PexMethod]
        public double NormCdf([PexAssumeUnderTest]Adaptee target, double delta)
        {
            double result = target.NormCdf(delta);
            return result;
            // TODO: add assertions to method AdapteeTest.NormCdf(Adaptee, Double)
        }

        /// <summary>Test stub for Precise(Double, Double)</summary>
        [PexMethod]
        public double Precise(
            [PexAssumeUnderTest]Adaptee target,
            double a,
            double b
        )
        {
            double result = target.Precise(a, b);
            return result;
            // TODO: add assertions to method AdapteeTest.Precise(Adaptee, Double, Double)
        }
    }
}
