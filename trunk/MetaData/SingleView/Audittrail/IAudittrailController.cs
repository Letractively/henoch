using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;

using MetaData.Audittrail.Interface.Services;


namespace MetaData.Audittrail
{
    public interface IAudittrailController : IAuditTrailCore
    {
        void AddBusinessEntity(AuditItem audit);
        IList<AuditItem> GetEntities();
    }
}
