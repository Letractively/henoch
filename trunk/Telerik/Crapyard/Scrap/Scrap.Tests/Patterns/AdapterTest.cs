// <copyright file="AdapterTest.cs" company="">Copyright ©  2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrap.Patterns;

namespace Scrap.Patterns
{
    /// <summary>This class contains parameterized unit tests for Adapter</summary>
    [PexClass(typeof(Adapter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class AdapterTest
    {
        /// <summary>Test stub for .ctor(Adaptee, Double)</summary>
        [PexMethod]
        public Adapter Constructor(Adaptee adaptee, double dt)
        {
            Adapter target = new Adapter(adaptee, dt);
            return target;
            // TODO: add assertions to method AdapterTest.Constructor(Adaptee, Double)
        }

        /// <summary>Test stub for .ctor(Distributions)</summary>
        [PexMethod]
        public Adapter Constructor01(Distributions target)
        {
            Adapter target01 = new Adapter(target);
            return target01;
            // TODO: add assertions to method AdapterTest.Constructor01(Distributions)
        }

        /// <summary>Test stub for .ctor()</summary>
        [PexMethod]
        public Adapter Constructor02()
        {
            Adapter target = new Adapter();
            return target;
            // TODO: add assertions to method AdapterTest.Constructor02()
        }

        /// <summary>Test stub for Distributions</summary>
        [PexMethod]
        public void DistributionsGet([PexAssumeUnderTest]Adapter target)
        {
            IDistributions result = target.Distributions;
            // TODO: add assertions to method AdapterTest.DistributionsGet(Adapter)
        }

        /// <summary>Test stub for NormCdf</summary>
        [PexMethod]
        public void NormCdfGet([PexAssumeUnderTest]Adapter target)
        {
            Func<double, double> result = target.NormCdf;
            // TODO: add assertions to method AdapterTest.NormCdfGet(Adapter)
        }
    }
}
