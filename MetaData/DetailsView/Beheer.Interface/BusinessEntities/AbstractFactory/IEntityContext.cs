using System.Collections.Generic;

namespace MetaData.Beheer.Interface.BusinessEntities.AbstractFactory
{
    public interface IEntityContext
    {
        string TableName { get; }
        IList<BeheerContextEntity> GetDetails();
        IList<BeheerContextEntity> GetBusinessEntities();
        void AddBusinessEntity(IBeheerContextEntity beheerContextEntity);
        void DeleteBusinessEntity(IBeheerContextEntity beheerContextEntity);
        void UpdateBusinessEntity(IBeheerContextEntity beheerContextEntity);
    }
}