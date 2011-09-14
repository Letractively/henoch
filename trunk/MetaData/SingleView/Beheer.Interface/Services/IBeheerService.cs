using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Beheer.Interface.Services
{
    public interface IBeheerService
    {
        bool AllowCrud { get; set; }
        BeheerContextEntity Selected { get; set; }
        BeheerContextEntity SelectedMaster { get; set; }
        
        //superkeys/unique identifiers
        long Id { get; set; }
        string DataKeyName { get; set; }

        //attributes
        IDictionary<string, AttributeValue> Attributes { get; set; }

        //Entity-Context
        string TableName { get; }
        IList<BeheerContextEntity> GetDetailsLastUpdated();
        BeheerContextEntity GetMaster();
        IList<BeheerContextEntity> GetMasters();

        //void UpdateDetail(BeheerContextEntity detail);
        void UpdateDetails(BeheerContextEntity master);
        void AddMaster(BeheerContextEntity master);

        IList<BeheerContextEntity> GetEntities();
        void AddBusinessEntity(IBeheerContextEntity adedBeheerContextEntity);
        void AddDetailBusinessEntity(BeheerContextEntity addedDetail);
        void DeleteBusinessEntity(IBeheerContextEntity deletedBeheerContextEntity);
        void DeleteDetailBusinessEntity(BeheerContextEntity deletedDetail);
        void UpdateBusinessEntity(IBeheerContextEntity updatedBeheerContextEntity);
        void UpdateDetailBusinessEntity(BeheerContextEntity updatedDetail);

    }
}