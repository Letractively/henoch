// <copyright file="BeheerThemasControllerTest.cs" company=""></copyright>
using System;
using System.Collections.Generic;
using MetaData.BeheerThemas;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.Interface.DataResource;
using MetaData.BeheerThemas.Services;
using MetaData.BeheerThemas.Tests.Mocks;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MetaData.BeheerThemas
{
    /// <summary>This class contains parameterized unit tests for BeheerThemasController</summary>
    [PexClass(typeof(BeheerThemasController))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class BeheerThemasControllerTest
    {
        /// <summary>Test stub for BeheerThemasService</summary>
        [PexMethod(MaxBranches = 20000)]
        public void BeheerThemasServiceGet([PexAssumeUnderTest]BeheerThemasController target)
        {
            IBeheerThemasService result = target.BeheerThemasService;
            // TODO: add assertions to method BeheerThemasControllerTest.BeheerThemasServiceGet(BeheerThemasController)
        }

        /// <summary>Test stub for .ctor(IBeheerThemasService)</summary>
        [PexMethod(MaxBranches = 20000)]
        public BeheerThemasController Constructor(IBeheerThemasService beheerThemasService)
        {
            BeheerThemasController target = new BeheerThemasController(beheerThemasService);
            return target;
            // TODO: add assertions to method BeheerThemasControllerTest.Constructor(IBeheerThemasService)
        }

        /// <summary>Test stub for GetThemaTable()</summary>
        [PexMethod]
        public IList<Thema> GetThemaTable([PexAssumeUnderTest]BeheerThemasController target)
        {
            IList<Thema> result = target.GetThemaTable();
            return result;
            // TODO: add assertions to method BeheerThemasControllerTest.GetThemaTable(BeheerThemasController)
        }
        [PexMethod(MaxBranches = 20000), PexAllowedException(typeof(ArgumentNullException))]
        public virtual void AddThema([PexAssumeUnderTest]BeheerThemasController target, Thema thema)
        {
            target.AddThema(thema);
        }
        [PexMethod(MaxBranches = 20000), PexAllowedException(typeof(ArgumentNullException))]
        public virtual void DeleteThema([PexAssumeUnderTest]BeheerThemasController target, Thema thema)
        {
            target.DeleteThema(thema);

        }
        [PexMethod(MaxBranches = 20000), PexAllowedException(typeof(ArgumentNullException))]
        public virtual void UpdateThema([PexAssumeUnderTest]BeheerThemasController target, Thema thema)
        {
            target.UpdateThema(thema);
        }

        /// <summary>Test stub for GetThemaTable()</summary>
        [PexMethod]
        public IList<Thema> GetThemaTable([PexAssumeUnderTest]MockBeheerThemasController target)
        {
            IList<Thema> result = target.GetThemaTable();
            return result;
            // TODO: add assertions to method BeheerThemasControllerTest.GetThemaTable(BeheerThemasController)
        }
        [PexMethod(MaxBranches = 20000), PexAllowedException(typeof(ArgumentNullException))]
        public virtual void AddThema([PexAssumeUnderTest]MockBeheerThemasController target, Thema thema)
        {
            target.AddThema(thema);
        }
        [PexMethod(MaxBranches = 20000), PexAllowedException(typeof(ArgumentNullException))]
        public virtual void DeleteThema([PexAssumeUnderTest]MockBeheerThemasController target, Thema thema)
        {
            target.DeleteThema(thema);

        }
        [PexMethod(MaxBranches = 20000), PexAllowedException(typeof(ArgumentNullException))]
        public virtual void UpdateThema([PexAssumeUnderTest]MockBeheerThemasController target, Thema thema)
        {
            target.UpdateThema(thema);
        }
    }
}
