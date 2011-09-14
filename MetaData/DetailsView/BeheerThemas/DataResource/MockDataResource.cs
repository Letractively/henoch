using System.Collections.Generic;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.Interface.DataResource;
using Rhino.Mocks;

namespace MetaData.BeheerThemas.DataResource
{
    public class MockDataResource
    {
        private IList<Thema> list;
        
        /// <summary>
        /// Deze is om BO te vullen
        /// </summary>
        /// <returns></returns>
        public IList<Thema> GetThemaTableStub()
        {
            if (list == null)
            {
                MockRepository mocks = new MockRepository();
                list = new List<Thema>();

                ///Make 10 tables/Business Objects.
                for (int i = 0; i < 4; i++)
                {
                    IThema table = mocks.Stub<IThema>();
                    table.ThemaNaam = "thema-" + i;
                    list.Add(new Thema {ThemaNaam = table.ThemaNaam});
                }
            }
            return list;
        }
    }
}