using System;
using System.Collections.Generic;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.Services;

namespace MetaData.BeheerThemas.DataResource
{
    public class MockBeheerThemasService : BeheerThemasService
    {
        private IList<Thema> m_ThemaTable = new List<Thema>();
        private long m_Id;

        public MockBeheerThemasService()
        {
            m_ThemaTable = new MockDataResource().GetThemaTableStub();
        }

        #region Implementation of IBeheerThemasService

        public override IList<Thema> GetThemaTable()
        {
            //ResourceFactory<MockThemasService> resourceFactory = new ResourceFactory<MockThemasService>();
            //resourceFactory.CreateResource();

            //m_ThemaTable = resourceFactory.Context.GetBusinessEntities() as IList<Thema>;
            return m_ThemaTable;
        }

        public override void AddThema(Thema thema)
        {
            if (thema == null)
                throw new ArgumentNullException("thema");
            thema.Id = m_Id;
            m_ThemaTable.Add(thema);
            m_Id++;
        }

        public override void DeleteThema(Thema thema)
        {
            Thema found = FindThema(m_ThemaTable, thema);
            if (found != null)
            {
                bool succeeded = m_ThemaTable.Remove(found);
            }
        }

        public override void UpdateThema(Thema thema)
        {
            Thema found = FindThema(m_ThemaTable, thema);
            if (found != null)
            {
                found.ThemaNaam = thema.ThemaNaam;
            }
        }

        //internal static Thema FindThema(IList<Thema> themas, Thema thema)
        //{
        //    IEnumerable<Thema> result = from aThema in themas
        //                                where aThema.Id.Equals(thema.Id)
        //                                select aThema;

        //    return result.FirstOrDefault();
        //}

        #endregion
    }
}