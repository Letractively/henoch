using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationTypes.ErrorHandler;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.DataResource;
using MetaData.BeheerThemas.Interface.DataResource;
using System.Threading;

namespace MetaData.BeheerThemas.Services
{
    public class BeheerThemasService : IBeheerThemasService
    {

        public BeheerThemasService()
        {

        }

        private long m_Id;
        private IList<Thema> m_ThemaTable=new List<Thema>();
        #region Implementation of IBeheerThemasService

        public virtual IList<Thema> GetThemaTable()
        {
            //m_ThemaTable = new MockDataResource().GetThemaTableStub();
            return m_ThemaTable;
        }

        public virtual void AddThema(Thema thema)
        {
            // <pex>
            if (thema == (Thema)null)
                throw new ArgumentNullException("thema");
            // </pex>
            Thread.Sleep(1000);
            if(thema.ThemaNaam.Equals("1"))
                throw new BusinessLayerException("duplicate");

            thema.Id = m_Id;
            m_ThemaTable.Add(thema);
            m_Id++;
        }

        public virtual void DeleteThema(Thema thema)
        {
            Thema found = FindThema(m_ThemaTable, thema);
            if (found!=null)
            {
                bool succeeded = m_ThemaTable.Remove(found);
            }

        }

        public virtual void UpdateThema(Thema thema)
        {
            // <pex>
            if (thema == (Thema)null)
                throw new ArgumentNullException("thema");
            // </pex>
            Thema found = FindThema(m_ThemaTable, thema);
            if (found != null)
            {
                found.ThemaNaam = thema.ThemaNaam;
            }
        }

        internal static Thema FindThema(IList<Thema> themas, Thema thema)
        {
            // <pex>
            if (thema == (Thema)null)
                throw new ArgumentNullException("thema");
            // </pex>
            var result = from aThema in themas 
                         where aThema.Id.Equals(thema.Id)
                         select aThema;
                         
            return result.FirstOrDefault();
        }
        #endregion
    }
}