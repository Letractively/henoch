using System.Collections.Generic;

namespace MetaData.Beheer
{
    public interface IMasterController<TMasterObject>
    {
        void UpdateDetails(TMasterObject entity);
        void AddMaster(TMasterObject master);
        IList<TMasterObject> GetDetails();
        void UpdateDetailEntity(TMasterObject detail);
        void DeleteDetailEntity(TMasterObject detail);
    }
}