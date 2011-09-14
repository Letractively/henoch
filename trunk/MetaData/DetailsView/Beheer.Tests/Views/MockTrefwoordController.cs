using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;


namespace MetaData.Beheer.Tests
{
    public class MockTrefwoordController : TrefwoordController
    {
        public MockTrefwoordController()
        {
            MockBusinessentity = new List<BeheerContextEntity>();
        }

        private IList<BeheerContextEntity> m_Entities;
        public List<BeheerContextEntity> MockBusinessentity { get; set; }
        public override IList<BeheerContextEntity> GetEntities()
        {
            m_Entities = MockBusinessentity;
            return MockBusinessentity;

        }
        public override BeheerContextEntity GetMaster()
        {
            return MockBusinessentity[0];
        }
        public override void AddDetail(BeheerContextEntity detail)
        {
            MockBusinessentity[0].Details.Add(detail);
        }
    }
}