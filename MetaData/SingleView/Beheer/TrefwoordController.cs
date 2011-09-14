using System;
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Interface.Services;
using MetaData.MasterDetail;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;

namespace MetaData.Beheer
{
    public class TrefwoordController : ITrefwoordController
    {
        private BeheerContextEntity m_Master;
        private bool m_AllowCrud;
        public virtual ICategorieService CategorieService { get; private set; }
        public virtual ITrefwoordService TrefwoordService { get; private set; }
        public virtual IMasterDetailService MasterDetailservice { get; private set; }

        public bool AllowCrud
        {
            get
            {
                m_AllowCrud = TrefwoordService.SelectedMaster.Equals(TrefwoordService.GetMaster());
                return m_AllowCrud;
            }     
            set
            {
                m_AllowCrud = value;   
            }
        }
        [InjectionConstructor]
        public TrefwoordController([ServiceDependency]ITrefwoordService beheerService,
             [ServiceDependency] ICategorieService categorieService,
             [ServiceDependency] IMasterDetailService masterDetailService)
        {
            // <pex>
            if (beheerService == null)
                throw new ArgumentNullException("beheerService");
            // </pex>
            TrefwoordService = beheerService;
            CategorieService = categorieService;
            MasterDetailservice = masterDetailService;
        }

        protected TrefwoordController()
        {

        }


        /// <summary>
        /// Deze wordt aangeroepen tijdens de page_load van trefwoord.
        /// </summary>
        /// <returns></returns>
        public virtual IList<BeheerContextEntity> GetEntities()
        {
            if(m_Master==null)
            {
                m_Master = TrefwoordService.GetMaster();
            }

            if (m_Master != null)
            {
                var result = m_Master.Details;
            
                return result;
            }
            return null;
        }

        public virtual bool AddBusinessEntityCalled { get; set; }

        public virtual void AddBusinessEntity(BeheerContextEntity entity)
        {
            //Master info meegeven.
            entity.Parent= new ParentKeyEntity
                               {
                                   DataKeyValue = m_Master.DataKeyValue,
                                   Id = m_Master.Id,
                                   DataKeyName = "categorienaam",
                                   Tablename = "categorie"
                               };
            TrefwoordService.AddBusinessEntity(entity);
        }

        public virtual bool DeleteBusinessEntityCalled { get; set; }

        public virtual void DeleteBusinessEntity(BeheerContextEntity entity)
        {
            entity.Parent = new ParentKeyEntity
            {
                DataKeyValue = m_Master.DataKeyValue,
                Id = m_Master.Id
            };
            TrefwoordService.DeleteBusinessEntity(entity);
        }

        public virtual  bool UpdateBusinessEntityCalled { get; set; }

        public virtual void UpdateBusinessEntity(BeheerContextEntity entity)
        {
            entity.Parent = new ParentKeyEntity
            {
                DataKeyValue = m_Master.DataKeyValue,
                Id = m_Master.Id
            };
            TrefwoordService.UpdateBusinessEntity(entity);
        }

        public BeheerContextEntity Selected { get; set; }

        public virtual BeheerContextEntity GetMaster()
        {
            if (m_Master == null)
            {
                //m_Master = CategorieService.GetMaster();
                m_Master = TrefwoordService.GetMaster();
            }
            return m_Master;
        }

        public virtual void AddDetail(BeheerContextEntity detail)
        {
            m_Master.Details.Add(detail);
        }

        public BeheerContextEntity GetSelectedMaster()
        {
            return TrefwoordService.SelectedMaster;
        }
    }
}
