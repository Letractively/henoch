using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Audittrail.Interface.Services
{
    public interface IAuditTrailService
    {
        IList<AuditItem> GetAuditTrailDomeintabelDefinitie(string tabelnaam);
        IList<AuditItem> GetAuditTrailDomeintabelDefinitie(string tabelnaam, string kolomnaam);

        IList<AuditItem> GetAuditTrailDomeintabelWaarden(string tabelnaam);
        IList<AuditItem> GetAuditTrailDomeintabelWaarden(string tabelnaam, string sleutelwaarde);

        int DeleteAll();
    }
}