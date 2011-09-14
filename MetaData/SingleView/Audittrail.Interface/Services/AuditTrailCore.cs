using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Audittrail.Interface.Services
{
    public class AuditTrailCore : IAuditTrailCore
    {
        private const string m_ServiceNotInitialized = "Service is nog niet geïnitialiseerd!";

        private IAuditTrailService m_Service;

        #region IAuditTrailCore Members

        public void Initialize()
        {
            //TODO
        }

        public IList<AuditItem> GetAuditTrailDomeintabelDefinitie(string tabelnaam)
        {
            return GetAuditTrailDomeintabelDefinitie(tabelnaam, null);
        }

        public IList<AuditItem> GetAuditTrailDomeintabelDefinitie(string tabelnaam, string kolomnaam)
        {
            if (m_Service == null)
                throw new InvalidOperationException(m_ServiceNotInitialized);

            return m_Service.GetAuditTrailDomeintabelDefinitie(tabelnaam, kolomnaam);
        }

        public IList<AuditItem> GetAuditTrailDomeintabelWaarden(string tabelnaam)
        {
            return GetAuditTrailDomeintabelWaarden(tabelnaam, null);
        }

        public IList<AuditItem> GetAuditTrailDomeintabelWaarden(string tabelnaam, string sleutelwaarde)
        {
            if (m_Service == null)
                throw new InvalidOperationException(m_ServiceNotInitialized);

            return m_Service.GetAuditTrailDomeintabelWaarden(tabelnaam, sleutelwaarde);
        }

        #endregion
    }
}