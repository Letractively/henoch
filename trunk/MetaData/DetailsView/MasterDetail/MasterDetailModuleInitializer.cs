using System.Collections.Generic;
using Beheer.BusinessObjects;
using Beheer.BusinessObjects.Dictionary;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;

namespace MetaData.MasterDetail
{
    public class MasterDetailModuleInitializer : ModuleInitializer
    {
        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Services);
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            globalServices.AddNew<MasterDetailService, IMasterDetailService>();
            // TODO: add a service that will be visible to any module
        }

        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
        }
    }

    public interface IMasterDetailService
    {
        void SetDetails(IList<BeheerContextEntity> details);
        IList<BeheerContextEntity> GetDetails();
    }

    public class MasterDetailService : IMasterDetailService
    {
        private IList<BeheerContextEntity> m_Details;
        /// <summary>
        /// De master moet deze aanroepen.
        /// </summary>
        /// <param name="details"></param>
        public void SetDetails(IList<BeheerContextEntity> details)
        {
            m_Details = details;
        }

        public IList<BeheerContextEntity> GetDetails()
        {
            return m_Details;
        }
    }
}
