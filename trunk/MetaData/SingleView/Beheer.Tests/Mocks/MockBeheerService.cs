using System;
using System.Collections.Generic;
using System.Threading;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Interface.Services;

namespace MetaData.Beheer.Tests.Mocks
{
    public class MockBeheerService : BeheerService
    {     
        IList<Thema> m_Themas = new List<Thema>();

        public MockBeheerService()
        {
            m_BusinessEntities = new MockDataResource().GetBusinessEntitiesTableStub<Thema>("thema") as IList<BeheerContextEntity>;
            m_Themas = new MockDataResource().GetBusinessEntitiesTableStub<Thema>("thema");
        }

        #region Implementation of IThemasService

        public override IList<BeheerContextEntity> GetEntities()
        {
            return m_Themas as IList<BeheerContextEntity>;
            //note: return base.GetThemaTable(), base.m_ThemaTable;
        }

        public override void AddBusinessEntity(IBeheerContextEntity beheerContextEntity)
        {
            if (beheerContextEntity == null)
                throw new ArgumentNullException("beheerContextEntity");
            beheerContextEntity.Id = m_Id;
            base.m_BusinessEntities.Add(beheerContextEntity as BeheerContextEntity);
            m_Id++;
        }

        public override void DeleteBusinessEntity(IBeheerContextEntity beheerContextEntity)
        {
            BeheerContextEntity found = FindBusinessEntity(m_BusinessEntities, beheerContextEntity as BeheerContextEntity);
            if (found != null)
            {
                bool succeeded = m_BusinessEntities.Remove(found);
            }
        }

        public override void UpdateBusinessEntity(IBeheerContextEntity beheerContextEntity)
        {
            BeheerContextEntity found = FindBusinessEntity(m_BusinessEntities, beheerContextEntity as BeheerContextEntity);
            if (found != null)
            {
                found.DataKeyValue = beheerContextEntity.DataKeyValue;
            }
        }

        #endregion

    }
}