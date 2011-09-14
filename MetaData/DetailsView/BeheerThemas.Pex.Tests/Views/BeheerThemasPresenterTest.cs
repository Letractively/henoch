// <copyright file="BeheerThemasPresenterTest.cs" company=""></copyright>
using System;
using MetaData.BeheerThemas.Views;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaData.BeheerThemas.Views
{
    /// <summary>This class contains parameterized unit tests for BeheerThemasPresenter</summary>
    [PexClass(typeof(BeheerThemasPresenter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class BeheerThemasPresenterTest
    {
        /// <summary>Test stub for OnViewInitialized()</summary>
        [PexMethod]
        public void OnViewInitialized([PexAssumeUnderTest]BeheerThemasPresenter target)
        {
            target.OnViewInitialized();
            // TODO: add assertions to method BeheerThemasPresenterTest.OnViewInitialized(BeheerThemasPresenter)
        }

        /// <summary>Test stub for OnViewLoaded()</summary>
        [PexMethod(MaxBranches = 20000)]
        public void OnViewLoaded([PexAssumeUnderTest]BeheerThemasPresenter target)
        {
            target.OnViewLoaded();
            // TODO: add assertions to method BeheerThemasPresenterTest.OnViewLoaded(BeheerThemasPresenter)
        }
    }
}
