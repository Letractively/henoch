using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Audittrail.Interface.Services
{
    public interface IPureFabricationRepoService
    {
        bool AllowCrud { get; set; }
        AuditItem Selected { get; set; }
        AuditItem SelectedMaster { get; set; }
        
        //superkeys/unique identifiers
        long Id { get; set; }
        string DataKeyName { get; set; }

        //attributes
        IDictionary<string, AttributeValue> Attributes { get; set; }

        //Entity-Context
        string TableName { get; }
        IList<AuditItem> GetDetailsLastUpdated();
        AuditItem GetMaster();
        IList<AuditItem> GetMasters();

        //void UpdateDetail(AuditItem detail);
        void UpdateDetails(AuditItem master);
        void AddMaster(AuditItem master);

        IList<AuditItem> GetEntities();
        void AddBusinessEntity(IAuditItem adedAuditItem);
        void AddDetailBusinessEntity(AuditItem addedDetail);
        void DeleteBusinessEntity(IAuditItem deletedAuditItem);
        void DeleteDetailBusinessEntity(AuditItem deletedDetail);
        void UpdateBusinessEntity(IAuditItem updatedAuditItem);
        void UpdateDetailBusinessEntity(AuditItem updatedDetail);

    }
}