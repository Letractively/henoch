
using MetaData.Beheer.Interface.Services;
using MetaData.Beheer.Views;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Configuration;

namespace MetaData.Beheer
{
    public class BeheerModuleInitializer : ModuleInitializer
    {
        private const string AuthorizationSection = "compositeWeb/authorization";

        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Parent.Services);
            AddModuleServices(container.Services);
            RegisterSiteMapInformation(container.Services.Get<ISiteMapBuilderService>(true));

            container.RegisterTypeMapping<IBeheerController, BeheerController>();
            container.RegisterTypeMapping<ICategorieController, CategorieController>();
            container.RegisterTypeMapping<ITrefwoordController, TrefwoordController>();
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            globalServices.AddNew<CategorieService, ICategorieService>();
            globalServices.AddNew<TrefwoordService, ITrefwoordService>();
            // TODO: add a service that will be visible to any module
        }

        protected virtual void AddModuleServices(IServiceCollection moduleServices)
        {
            moduleServices.AddNew<BeheerService, IBeheerService>();
            moduleServices.AddNew<CategorieService, ICategorieService>();
            moduleServices.AddNew<TrefwoordService, ITrefwoordService>();
            //moduleServices.AddNew<ThemasService, IBeheerService>();
            //moduleServices.AddNew<CategorieService, ICategorieService>();
            //moduleServices.AddNew<TrefwoordService, IBeheerService>();
            // TODO: add a service that will be visible to this module
        }

        protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
        {
            SiteMapNodeInfo moduleNode = new SiteMapNodeInfo("Beheer", "~/Beheer/Default.aspx", "Beheer");
            siteMapBuilderService.AddNode(moduleNode);

            //children
            SiteMapNodeInfo beheerThemasViewNode = new SiteMapNodeInfo("BeheerThemasView",
                                                                       "~/Beheer/Themas.aspx",
                                                                       "Thema Beheer");
            siteMapBuilderService.AddNode(beheerThemasViewNode, moduleNode);
            SiteMapNodeInfo beheerCategrieViewNode = new SiteMapNodeInfo("CategorieView",
                                                           "~/Beheer/Categorieen.aspx",
                                                           "Categorie Beheer");
            siteMapBuilderService.AddNode(beheerCategrieViewNode, moduleNode);

            // TODO: register other site map nodes that Beheer module might provide            
        }

        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
            IAuthorizationRulesService authorizationRuleService = services.Get<IAuthorizationRulesService>();
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
    }
}
