using System;
using Assert = NUnit.Framework.Assert;

namespace TestObservlet.Extensions
{
    public static class NunitExtensionMethods
    {
        public static Assert Inconclusive(this Assert assert, string message)
        {
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Inconclusive(message);
            return assert;
        }
    }
}