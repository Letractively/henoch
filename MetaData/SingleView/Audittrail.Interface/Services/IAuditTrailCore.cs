using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Audittrail.Interface.Services
{
    public interface IAuditTrailCore
    {
        void Initialize();

        IList<AuditItem> GetAuditTrailDomeintabelDefinitie(string tabelnaam);
        IList<AuditItem> GetAuditTrailDomeintabelDefinitie(string tabelnaam, string kolomnaam);

        IList<AuditItem> GetAuditTrailDomeintabelWaarden(string tabelnaam);
        IList<AuditItem> GetAuditTrailDomeintabelWaarden(string tabelnaam, string sleutelwaarde);
    }
}