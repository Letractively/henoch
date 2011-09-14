// <copyright file="BeheerThemasModuleInitializerTest.cs" company=""></copyright>
using System;
using System.Configuration;
using MetaData.BeheerThemas;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaData.BeheerThemas
{
    /// <summary>This class contains parameterized unit tests for BeheerThemasModuleInitializer</summary>
    [PexClass(typeof(BeheerThemasModuleInitializer))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class BeheerThemasModuleInitializerTest
    {
        /// <summary>Test stub for Configure(IServiceCollection, Configuration)</summary>
        [PexMethod(MaxRuns = 25)]
        public void Configure(
            [PexAssumeUnderTest]BeheerThemasModuleInitializer target,
            IServiceCollection services,
            global::System.Configuration.Configuration moduleConfiguration
        )
        {
            target.Configure(services, moduleConfiguration);
            // TODO: add assertions to method BeheerThemasModuleInitializerTest.Configure(BeheerThemasModuleInitializer, IServiceCollection, Configuration)
        }

        /// <summary>Test stub for Load(CompositionContainer)</summary>
        [PexMethod(MaxRuns=25)]
        public void Load(
            [PexAssumeUnderTest]BeheerThemasModuleInitializer target,
            CompositionContainer container
        )
        {
            target.Load(container);
            // TODO: add assertions to method BeheerThemasModuleInitializerTest.Load(BeheerThemasModuleInitializer, CompositionContainer)
        }
    }
}
