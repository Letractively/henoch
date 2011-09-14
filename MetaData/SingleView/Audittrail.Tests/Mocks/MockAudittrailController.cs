using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Audittrail;

namespace MetaData.Audittrail.Tests.Mocks
{
    public class MockAudittrailController : IAudittrailController
    {

        #region Implementation of IAudittrailController

        public void AddBusinessEntity(AuditItem audit)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IAudittrailController

        public IList<AuditItem> GetEntities()
        {
            throw new NotImplementedException();
        }

        #endregion

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

    }
}
