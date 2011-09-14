// <copyright file="DefaultViewPresenterTest.cs" company=""></copyright>
using System;
using MetaData.BeheerThemas;
using MetaData.BeheerThemas.Views;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaData.BeheerThemas.Views
{
    /// <summary>This class contains parameterized unit tests for DefaultViewPresenter</summary>
    [PexClass(typeof(DefaultViewPresenter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class DefaultViewPresenterTest
    {
        /// <summary>Test stub for .ctor(IBeheerThemasController)</summary>
        [PexMethod]
        public DefaultViewPresenter Constructor(IBeheerThemasController controller)
        {
            DefaultViewPresenter target = new DefaultViewPresenter(controller);
            return target;
            // TODO: add assertions to method DefaultViewPresenterTest.Constructor(IBeheerThemasController)
        }
    }
}
