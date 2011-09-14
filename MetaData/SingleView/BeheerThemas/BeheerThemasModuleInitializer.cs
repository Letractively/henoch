using System;
using System.Collections.Generic;
using System.Text;
using MetaData.BeheerThemas.Services;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;

namespace MetaData.BeheerThemas
{
    public class BeheerThemasModuleInitializer : ModuleInitializer
    {
        private const string AuthorizationSection = "compositeWeb/authorization";

        public override void Load(CompositionContainer container)
        {
            if (container == (CompositionContainer)null)
                throw new ArgumentNullException("container", 
                    "Controleer of CompositionContainer beschikbaar is.");

            base.Load(container);

            AddGlobalServices(container.Parent.Services);
            AddModuleServices(container.Services);
            RegisterSiteMapInformation(container.Services.Get<ISiteMapBuilderService>(true));

            container.RegisterTypeMapping<IBeheerThemasController, BeheerThemasController>();
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            // TODO: add a service that will be visible to any module
        }

        protected virtual void AddModuleServices(IServiceCollection moduleServices)
        {
            moduleServices.AddNew<BeheerThemasService, IBeheerThemasService>();
            // TODO: add a service that will be visible to this module
        }

        protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
        {
            //SiteMapNodeInfo moduleNode = new SiteMapNodeInfo("BeheerThemas", "~/BeheerThemas/Default.aspx", "BeheerThemas");
            //siteMapBuilderService.AddNode(moduleNode);

            //SiteMapNodeInfo beheerThemasViewNode = new SiteMapNodeInfo("BeheerThemasView",
            //                                                           "~/BeheerThemas/BeheerThemas.aspx",
            //                                                           "Thema Beheer");
            //siteMapBuilderService.AddNode(beheerThemasViewNode, moduleNode);

            // TODO: register other site map nodes that BeheerThemas module might provide            
        }

        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
            IAuthorizationRulesService authorizationRuleService = GetAuthorizationService(services);// services.Get<IAuthorizationRulesService>();
            if (authorizationRuleService != null)
            {
                AuthorizationConfigurationSection authorizationSection = moduleConfiguration.GetSection(AuthorizationSection) as AuthorizationConfigurationSection;
                if (authorizationSection != null)
                {
                    foreach (AuthorizationRuleElement ruleElement in authorizationSection.ModuleRules)
                    {
                        authorizationRuleService.RegisterAuthorizationRule(ruleElement.AbsolutePath, ruleElement.RuleName);
                    }
                }
            }
        }

        private static IAuthorizationRulesService GetAuthorizationService(IServiceCollection services)
        {           
            try
            {
                var enumerator = services.GetEnumerator();
                enumerator.MoveNext();
                return services.Get<IAuthorizationRulesService>();
            }
            catch (NullReferenceException ex)
            {
                throw new ArgumentNullException("services",
                    "Er zijn geen services beschikbaar. Controleer de service beschikbaarheid.");
            }            
        }
    }
}
