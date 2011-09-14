using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;

namespace MetaData.Beheer
{
    public interface IBeheerController
    {
        IList<BeheerContextEntity> GetEntities();
        bool AddBusinessEntityCalled { get; set; }
        void AddBusinessEntity(BeheerContextEntity entity);

        bool DeleteBusinessEntityCalled { get; set; }
        void DeleteBusinessEntity(BeheerContextEntity entity);

        bool UpdateBusinessEntityCalled { get; set; }
        void UpdateBusinessEntity(BeheerContextEntity entity);

        BeheerContextEntity Selected { get; set; }
        bool AllowCrud { get; set; }
    }
}
