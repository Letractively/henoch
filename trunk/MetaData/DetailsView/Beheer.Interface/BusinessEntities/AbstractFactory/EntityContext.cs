using System;
using System.Collections.Generic;
using MetaData.Beheer.Interface.Services;

namespace MetaData.Beheer.Interface.BusinessEntities.AbstractFactory
{
    public class EntityContext<TBeheerService> : IEntityContext
        where TBeheerService : IBeheerService, new()        
    {
        private TBeheerService m_MyBeheerService;

        public EntityContext()
        {
            m_MyBeheerService=new TBeheerService();
        }


        public string TableName
        {
            get { return m_MyBeheerService.TableName; }
        }

        public IList<BeheerContextEntity> GetDetails()
        {
            return m_MyBeheerService.GetDetailsLastUpdated();
        }

        public IList<BeheerContextEntity> GetBusinessEntities()
        {
            return m_MyBeheerService.GetEntities();
        }

        public void AddBusinessEntity(IBeheerContextEntity beheerContextEntity)
        {
            m_MyBeheerService.AddBusinessEntity(beheerContextEntity);
        }

        public void DeleteBusinessEntity(IBeheerContextEntity beheerContextEntity)
        {
            m_MyBeheerService.DeleteBusinessEntity(beheerContextEntity);
        }

        public void UpdateBusinessEntity(IBeheerContextEntity beheerContextEntity)
        {
            m_MyBeheerService.UpdateBusinessEntity(beheerContextEntity);
        }
    }
}