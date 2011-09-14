using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace MetaData.Audittrail.Tests
{
    /// <summary>
    /// Summary description for AudittrailModuleInitializerFixture
    /// </summary>
    [TestClass]
    public class AudittrailModuleInitializerFixture
    {
        public AudittrailModuleInitializerFixture()
        {
        }

        [TestMethod]
        public void AudittrailGetsRegisteredOnSiteMap()
        {
            TestableModuleInitializer moduleInitializer = new TestableModuleInitializer();
            SiteMapBuilderService siteMapBuilder = new SiteMapBuilderService();

            moduleInitializer.RegisterSiteMapInformation(siteMapBuilder);

            SiteMapNodeInfo node = siteMapBuilder.GetChildren(siteMapBuilder.RootNode.Key)[0];
            Assert.AreEqual("Audittrail", node.Key);
        }

    }

    class TestableModuleInitializer : AudittrailModuleInitializer
    {
        public new void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilder)
        {
            base.RegisterSiteMapInformation(siteMapBuilder);
        }
    }
}
