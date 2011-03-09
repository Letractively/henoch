// <copyright file="FileSystemTest.cs" company="Microsoft">Copyright © Microsoft 2010</copyright>
using System;
using System.Moles;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMath;
using Microsoft.Pex.Framework.Generated;

namespace MyMath
{
    /// <summary>This class contains parameterized unit tests for FileSystem</summary>
    [PexClass(typeof(FileSystem))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]

    [TestClass]
    public partial class FileSystemTest
    {
        /// <summary>Test stub for Check()</summary>
        [PexMethod, PexAllowedException(typeof(ApplicationException))]
        public void Check()
        {
            //arrange
            MDateTime.NowGet = () => new DateTime(2000, 1, 1);
            FileSystem.Check();
            // TODO: add assertions to method FileSystemTest.Check()
        }

        /// <summary>Test stub for ReadAllText(String)</summary>
        [PexMethod]
        public string ReadAllText(string fileName)
        {
            string result = FileSystem.ReadAllText(fileName);
            return result;
            // TODO: add assertions to method FileSystemTest.ReadAllText(String)            
        }
        [TestMethod]
        [HostType("Moles")]
        public void CheckThrowsApplicationException162()
        {
            this.Check();
        }
        [TestMethod]
        public void ReadAllText492()
        {
            string s;
            s = this.ReadAllText("\0");
            Assert.AreEqual<string>("", s);
        }
    }
}
