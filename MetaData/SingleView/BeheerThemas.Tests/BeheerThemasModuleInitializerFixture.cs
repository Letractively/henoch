using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace MetaData.BeheerThemas.Tests
{
    /// <summary>
    /// Summary description for BeheerThemasModuleInitializerFixture
    /// </summary>
    [TestClass]
    public class BeheerThemasModuleInitializerFixture
    {
        public BeheerThemasModuleInitializerFixture()
        {
        }

        [Ignore]//this site has been removed
        public void BeheerThemasGetsRegisteredOnSiteMap()
        {
            TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();
            SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();

            moduleInitializer.RegisterSiteMapInformation(siteMapBuilder);

            SiteMapNodeInfo node = siteMapBuilder.GetChildren(siteMapBuilder.RootNode.Key)[0];
            Assert.AreEqual("BeheerThemas", node.Key);
        }

    }

    class TestableModuleInitializer : BeheerThemasModuleInitializer
    {
        public new void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilder)
        {
            base.RegisterSiteMapInformation(siteMapBuilder);
        }
    }
}
