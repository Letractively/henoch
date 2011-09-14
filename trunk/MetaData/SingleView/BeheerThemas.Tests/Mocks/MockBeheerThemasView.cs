using System.Collections.Generic;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.Views;

namespace MetaData.BeheerThemas.Tests.Mocks
{
    class MockBeheerThemasView : IBeheerThemasView
    {
        public IList<Thema> ThemaTable { get; set; }
    }
}