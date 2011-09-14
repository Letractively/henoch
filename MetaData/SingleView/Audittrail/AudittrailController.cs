using System;
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Audittrail.Interface.Services;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.ObjectBuilder;

namespace MetaData.Audittrail
{
    public class AudittrailController : IAudittrailController
    {
        public AudittrailController()
        {
        }
        public virtual IPureFabricationRepoService AudittrailService { get; private set; }

        [InjectionConstructor]
        public AudittrailController([ServiceDependency] IPureFabricationRepoService audittrailService)
        {
            // <pex>
            if (audittrailService == null)
                throw new ArgumentNullException("audittrailService");
            // </pex>
            AudittrailService = audittrailService;
        }
        #region Implementation of IAuditTrailCore

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public IList<AuditItem> GetAuditTrailDomeintabelDefinitie(string tabelnaam)
        {
            throw new NotImplementedException();
        }

        public IList<AuditItem> GetAuditTrailDomeintabelDefinitie(string tabelnaam, string kolomnaam)
        {
            throw new NotImplementedException();
        }

        public IList<AuditItem> GetAuditTrailDomeintabelWaarden(string tabelnaam)
        {
            throw new NotImplementedException();
        }

        public IList<AuditItem> GetAuditTrailDomeintabelWaarden(string tabelnaam, string sleutelwaarde)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IAudittrailController

        public void AddBusinessEntity(AuditItem audit)
        {
            AudittrailService.AddBusinessEntity(audit);
        }

        public IList<AuditItem> GetEntities()
        {
            return AudittrailService.GetEntities();
        }

        #endregion
    }
}
