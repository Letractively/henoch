// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Pex.Framework.Generated;

namespace Scrap.Patterns
{
    public partial class AdapterTest
    {
[TestMethod]
[PexGeneratedBy(typeof(AdapterTest))]
public void Constructor03()
{
    Adapter adapter;
    adapter = this.Constructor((Adaptee)null, 0);
    Assert.IsNotNull((object)adapter);
    Assert.IsNotNull(adapter.NormCdf);
    Assert.IsNull(adapter.Distributions);
}
    }
}
