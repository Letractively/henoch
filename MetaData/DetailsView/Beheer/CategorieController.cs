using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beheer.BusinessObjects;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Interface.Services;
using MetaData.Beheer.Views;
using MetaData.MasterDetail;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;

namespace MetaData.Beheer
{
    public class CategorieController : ICategorieController
    {
        private bool m_AllowCrud;
        public virtual ITrefwoordService TrefwoordService { get; private set; }
        public virtual ICategorieService CategorieService { get; private set; }
        public virtual IMasterDetailService MasterDetailservice { get; private set; }
        public bool AllowCrud
        {
            get
            {
                return m_AllowCrud;
            }
            set
            {                
                m_AllowCrud = value;
            }
        }
        public CategorieController()
        {

        }
        [InjectionConstructor]
        public CategorieController([ServiceDependency]ICategorieService beheerService,
            [ServiceDependency]ITrefwoordService trefwoordService,
             [ServiceDependency] IMasterDetailService masterDetailService)
        {
            // <pex>
            if (beheerService == null)
                throw new ArgumentNullException("beheerService");
            // </pex>
            CategorieService = beheerService;
            TrefwoordService = trefwoordService;
            MasterDetailservice=masterDetailService;
        }

        //public IList<Thema> GetThemas()
        //{
        //    return CategorieService.GetEntities() as IList<Thema>;
        //}
        public virtual IList<BeheerContextEntity> GetEntities()
        {
            return CategorieService.GetEntities();
        }

        public virtual  bool AddBusinessEntityCalled { get; set; }

        public void AddBusinessEntity(BeheerContextEntity entity)
        {
            CategorieService.AddBusinessEntity(entity);
        }

        public bool DeleteBusinessEntityCalled { get; set; }

        public void DeleteBusinessEntity(BeheerContextEntity entity)
        {
            CategorieService.DeleteBusinessEntity(entity);
        }

        public bool UpdateBusinessEntityCalled { get; set; }

        public void UpdateBusinessEntity(BeheerContextEntity entity)
        {
            CategorieService.UpdateBusinessEntity(entity);
        }

        public virtual BeheerContextEntity Selected
        {
            get { return CategorieService.Selected; }
            set
            {
                CategorieService.Selected = value;
                TrefwoordService.SelectedMaster = value;
            }
        }

        public void UpdateDetails(BeheerContextEntity master)
        {
            //TODO: wijzig de trefwoord service
            TrefwoordService.UpdateDetails(master);

        }

        public void AddMaster(BeheerContextEntity master)
        {
            TrefwoordService.AddMaster(master);
        }
        /// <summary>
        /// Haal de details op en bewaar de master in de detail.
        /// </summary>
        /// <returns></returns>
        public IList<BeheerContextEntity> GetDetails()
        {
            IList<BeheerContextEntity> details = new List<BeheerContextEntity>();
            var masters = CategorieService.GetEntities();

            foreach (var master in masters)
            {
                var masterDetails = master.Details;
                if (masterDetails.Count > 0)
                {
                    //Er zijn Details...Voeg het toe aan details-list.
                    foreach (BeheerContextEntity detail in masterDetails)
                    {
                        //bewaar de masters context ook.
                        detail.Parent = new ParentKeyEntity
                                            {
                                                DataKeyValue = master.DataKeyValue,
                                                Id = master.Id
                                            };
                        detail.MasterId = master.Id;
                        detail.Master = master.DataKeyValue;
                        details.Add(detail);
                    }
                }
                
            }


            return details;
        }

        /// <summary>
        /// De detail heeft altijd een parent/master.
        /// Als de detail.Id = -1 , dan refereert dit aan een categorie/master zonder details.
        /// </summary>
        /// <param name="detail"></param>
        public void UpdateDetailEntity(BeheerContextEntity detail)
        {
            var masters = CategorieService.GetEntities();
            var qryMaster = from aMaster in masters
                            where aMaster.Id.Equals(detail.Parent.Id) || aMaster.DataKeyValue.Equals(detail.Master)
                            select aMaster;
            var master = qryMaster.FirstOrDefault();

            detail.Parent = new ParentKeyEntity
            {
                DataKeyValue = detail.Master,//Deze kan veranderd zijn.
                Id = master.Id,
                DataKeyName = "categorienaam",
                Tablename = "categorie"
            };

            if (master.Details.Count==0)
            {
                CategorieService.UpdateBusinessEntity(detail);
            }
            else
            {
                master.DataKeyValue = detail.Master;//update de master en zijn detail.
                master.Details.Where( det => det.Id== detail.Id).
                    Update( det => det.DataKeyValue = detail.DataKeyValue);
                CategorieService.UpdateBusinessEntity(master);
            }
            
            
        }

        public void DeleteDetailEntity(BeheerContextEntity detail)
        {
            //TrefwoordService.DeleteBusinessEntity(detail);
            CategorieService.DeleteDetailBusinessEntity(detail);
        }

        public int Test { set; get; }
    }
}