using System;
using System.Collections.Generic;
using System.Text;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer;

using MetaData.Beheer.Interface.Services;

namespace MetaData.Beheer.Tests.Mocks
{

    public class MockBeheerControllerBase : BeheerController
    {
        protected IList<BeheerContextEntity> m_Entities;
        public virtual List<BeheerContextEntity> MockBusinessentity{ get; set; }

        public override IList<BeheerContextEntity> GetEntities()
        {
            return m_Entities;
        }
    }

    public class MockBeheerController : MockBeheerControllerBase
    {

        public MockBeheerController()
        {
            MockBusinessentity = new List<BeheerContextEntity>();
        }

        public override List<BeheerContextEntity> MockBusinessentity { get; set; }
        public override IList<BeheerContextEntity> GetEntities()
        {
            m_Entities = MockBusinessentity;
            return MockBusinessentity;
        }

        public IList<BeheerContextEntity> GetThemaTable<TBeheerService>() where TBeheerService : IBeheerService, new()
        {
            throw new NotImplementedException();
        }


        #region IBeheerController Members

        public IList<BeheerContextEntity> GetEntities<TBeheerService>(string serviceName) where TBeheerService :
            IBeheerService, new()
        {
            throw new NotImplementedException();
        }

        public IList<BeheerContextEntity> GetEntities2()
        {
            throw new NotImplementedException();
        }

        public bool AddBusinessEntityCalled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void AddBusinessEntity(BeheerContextEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBusinessEntityCalled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void DeleteBusinessEntity(BeheerContextEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateBusinessEntityCalled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateBusinessEntity(BeheerContextEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region obsolete
        public bool AddThemaCalled
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void AddThema(Thema thema)
        {
            throw new NotImplementedException();
        }

        public bool DeleteThemaCalled
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void DeleteThema(Thema thema)
        {
            throw new NotImplementedException();
        }

        public bool UpdateThemaCalled
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void UpdateThema(Thema thema)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
