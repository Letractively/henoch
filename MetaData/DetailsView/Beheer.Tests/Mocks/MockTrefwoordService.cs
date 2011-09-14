using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Interface.Services;

namespace MetaData.Beheer.Tests.Mocks
{
    public class MockTrefwoordService : BeheerService
    {
        public MockTrefwoordService()
        {
            m_BusinessEntities = new MockDataResource().GetBusinessEntitiesTableStub<BeheerContextEntity>("trefwoord") as IList<BeheerContextEntity>;
        }
        public override IList<BeheerContextEntity> GetEntities()
        {
            return m_BusinessEntities;
        }
    }
}