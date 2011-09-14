using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace MetaData.Beheer.Tests
{
    /// <summary>
    /// Summary description for BeheerModuleInitializerFixture
    /// </summary>
    [TestClass]
    public class BeheerModuleInitializerFixture
    {
        public BeheerModuleInitializerFixture()
        {
        }

        [TestMethod]
        public void BeheerGetsRegisteredOnSiteMap()
        {
            TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();
            SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();

            moduleInitializer.RegisterSiteMapInformation(siteMapBuilder);

            SiteMapNodeInfo node = siteMapBuilder.GetChildren(siteMapBuilder.RootNode.Key)[0];
            Assert.AreEqual("Beheer", node.Key);
        }

    }

    class TestableModuleInitializer : BeheerModuleInitializer
    {
        public new void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilder)
        {
            base.RegisterSiteMapInformation(siteMapBuilder);
        }
    }
}
