using System;
using System.Collections.Generic;
using System.Linq;
using Beheer.BusinessObjects.Dictionary;

using MetaData.Beheer.Interface.Services;

namespace MetaData.Beheer.Tests.Mocks
{
    public class MockThemasService : BeheerService
    {
        private IList<Thema> m_Themas;
        public MockThemasService()
        {
            m_BusinessEntities = new Tests.Mocks.MockDataResource().GetBusinessEntitiesTableStub("thema");
            //m_Themas = new MockDataResource().GetBusinessEntitiesTableStub<Thema>("thema");
        }

        #region Implementation of IThemasService
        public override IList<BeheerContextEntity> GetEntities()
        {
            return m_BusinessEntities;
            //note: return base.GetThemaTable(), base.m_ThemaTable;
        }


        public override void AddBusinessEntity(IBeheerContextEntity thema)
        {
            if (thema == null)
                throw new ArgumentNullException("thema");
            thema.Id = m_Id;
            base.m_BusinessEntities.Add(thema as BeheerContextEntity);
            m_Id++;
        }

        public override void DeleteBusinessEntity(IBeheerContextEntity thema)
        {
            BeheerContextEntity found = FindBusinessEntity(m_BusinessEntities, thema as BeheerContextEntity);
            if (found != null)
            {
                bool succeeded = m_BusinessEntities.Remove(found);
            }
        }

        public override void UpdateBusinessEntity(IBeheerContextEntity thema)
        {
            BeheerContextEntity found = FindBusinessEntity(m_BusinessEntities, thema as BeheerContextEntity);
            if (found != null)
            {
                found.DataKeyValue = thema.DataKeyValue;
            }
        }

        #endregion

    }
}