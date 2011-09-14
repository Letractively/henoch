using System;
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Interface.Services;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;

namespace MetaData.Beheer
{
    public class BeheerController : IBeheerController
    {
        public BeheerController()
        {
        }
        public bool AllowCrud { get; set; }
        [InjectionConstructor]
        public BeheerController([ServiceDependency] IBeheerService beheerService)
            //    [ServiceDependency] IMasterDetailService masterDetailService)
        {
            // <pex>
            if (beheerService == null)
                throw new ArgumentNullException("beheerService");
            // </pex>
            BeheerService = beheerService;
        }

        public virtual IBeheerService BeheerService { get; private set; }

        #region IBeheerController Members

        public virtual IList<BeheerContextEntity> GetEntities()
        {
            return BeheerService.GetEntities();
        }

        public virtual bool AddBusinessEntityCalled { get; set; }

        public virtual void AddBusinessEntity(BeheerContextEntity entity)
        {
            BeheerService.AddBusinessEntity(entity);
        }

        public virtual bool DeleteBusinessEntityCalled { get; set; }

        public void DeleteBusinessEntity(BeheerContextEntity entity)
        {
            BeheerService.DeleteBusinessEntity(entity);
        }

        public virtual bool UpdateBusinessEntityCalled { get; set; }

        public virtual void UpdateBusinessEntity(BeheerContextEntity entity)
        {
            BeheerService.UpdateBusinessEntity(entity);
        }

        public BeheerContextEntity Selected { get; set; }

        #endregion

        public virtual IList<Thema> GetThemas()
        {
            return BeheerService.GetEntities() as IList<Thema>;
        }
    }
}