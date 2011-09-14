// <copyright file="BeheerThemasServiceTest.cs" company=""></copyright>
using System;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.DataResource;
using MetaData.BeheerThemas.Interface.DataResource;
using MetaData.BeheerThemas.Services;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MetaData.BeheerThemas.Services
{
    /// <summary>This class contains parameterized unit tests for BeheerThemasService</summary>
    [PexClass(typeof(BeheerThemasService))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class BeheerThemasServiceTest
    {
        /// <summary>Test stub for GetThemaTable()</summary>
        [PexMethod]
        public IList<Thema> GetThemaTable([PexAssumeUnderTest]BeheerThemasService target)
        {
            IList<Thema> result = target.GetThemaTable();
            return result;
            // TODO: add assertions to method BeheerThemasServiceTest.GetThemaTable(BeheerThemasService)
        }
        [PexMethod]
        public virtual void AddThema([PexAssumeUnderTest]BeheerThemasService target, Thema thema)
        {
            target.AddThema(thema);
        }
        [PexMethod]
        public virtual void DeleteThema([PexAssumeUnderTest]BeheerThemasService target, Thema thema)
        {
            target.DeleteThema(thema);

        }
        [PexMethod]
        public virtual void UpdateThema([PexAssumeUnderTest]BeheerThemasService target, Thema thema)
        {
            target.UpdateThema(thema);
        }
        [PexMethod]
        public virtual void AddThema([PexAssumeUnderTest]MockBeheerThemasService target, Thema thema)
        {
            target.AddThema(thema);
        }
        [PexMethod]
        public virtual void DeleteThema([PexAssumeUnderTest]MockBeheerThemasService target, Thema thema)
        {
            target.DeleteThema(thema);

        }
        [PexMethod]
        public virtual void UpdateThema([PexAssumeUnderTest]MockBeheerThemasService target, Thema thema)
        {
            target.UpdateThema(thema);
        }
    }
}
