using System;
using System.Collections.Generic;
using Beheer.BusinessObjects.Dictionary;
using MetaData.Beheer.Views;

namespace MetaData.Beheer.Tests
{
    public class MockTrefwoordenView : IBusinessEntityView
    {
        private BeheerContextEntity m_Master;
        public IList<BeheerContextEntity> BusinessEntities { get; set; }
        public IList<BeheerContextEntity> DetailsEntities
        {
            set { throw new NotImplementedException(); }
        }

        public bool IsMasterView
        {
            set { throw new NotImplementedException(); }
        }

        public bool IsInsertingInline
        {
            set { throw new NotImplementedException(); }
        }

        public BeheerContextEntity Master
        {
            set { m_Master=value; }
        }

        public bool IsSortable
        {
            set { throw new NotImplementedException(); }
        }

        public bool AllowCrud
        {
            set { throw new NotImplementedException(); }
        }

        public BeheerContextEntity Selected
        {
            set { throw new NotImplementedException(); }
        }

        public bool ShowFooter
        {
            set { throw new NotImplementedException(); }
        }

        public void ShowErrorMessage(string errorMessage)
        {
            throw new NotImplementedException();
        }

        public bool IsVisibleInsert { get; set; }

        public IList<BeheerContextEntity> Details
        {
            set { m_Master.Details = value; }
            get
            {
                return m_Master.Details;
            }
        }
    }
}