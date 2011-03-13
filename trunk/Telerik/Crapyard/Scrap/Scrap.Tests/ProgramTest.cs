// <copyright file="ProgramTest.cs" company="">Copyright ©  2010</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrap;

namespace Scrap
{
    /// <summary>This class contains parameterized unit tests for Program</summary>
    [PexClass(typeof(Program))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ProgramTest
    {
        /// <summary>Test stub for Main(String[])</summary>
        [PexMethod(MaxConstraintSolverTime = 4)]
        public int Main(string[] args)
        {
            int result = Program.Main(args);
            return result;
            // TODO: add assertions to method ProgramTest.Main(String[])
        }
        

    }
}
