using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.DataResource;

namespace MetaData.BeheerThemas.Tests.Mocks
{
    public class MockBeheerThemasController : BeheerThemasController
    {
        private IList<Thema> m_ThemaTable;
        public List<Thema> MockThemas { get; set; }

        public MockBeheerThemasController()
        {
            MockThemas = new List<Thema>();
        }

        public MockBeheerThemasController(MockBeheerThemasService service)
        {
            MockThemas = service.GetThemaTable() as List<Thema>;
        }

        #region Overrides BeheerThemasController

        public override IList<Thema> GetThemaTable()
        {
            m_ThemaTable = MockThemas;
            return MockThemas;
        }



        public override bool AddThemaCalled { get; set; }
        public override bool DeleteThemaCalled { get; set; }
        public override bool UpdateThemaCalled { get; set; }
        public override Thema UpdatedThema { get; set; }
        public override Thema DeletedThema { get; set; }



        public override void AddThema(Thema thema)
        {
            AddThemaCalled = true;
            if (m_ThemaTable == null)
            {
                m_ThemaTable = new List<Thema>();
            }
            m_ThemaTable.Add(thema);
            MockThemas = m_ThemaTable as List<Thema>;
        }

        public override void DeleteThema(Thema thema)
        {
            DeleteThemaCalled = true;
            DeletedThema = thema;
        }

        public override void UpdateThema(Thema thema)
        {
            UpdateThemaCalled = true;
            UpdatedThema = thema;
        }

        //public override ReadOnlyCollection<Thema> GetThemas()
        //{
        //    return MockThemas.AsReadOnly();
        //}

        //public override void CreateNewThemas()
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}