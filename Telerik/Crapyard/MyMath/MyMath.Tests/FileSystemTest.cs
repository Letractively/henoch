// <copyright file="FileSystemTest.cs" company="Microsoft">Copyright © Microsoft 2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMath;

namespace MyMath
{
    /// <summary>This class contains parameterized unit tests for FileSystem</summary>
    [PexClass(typeof(FileSystem))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class FileSystemTest
    {
        /// <summary>Test stub for Check()</summary>
        [PexMethod]
        public void Check()
        {
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
    }
}
