using System;
using System.Collections.Generic;
using MetaData.Beheer.Interface.Services;

namespace MetaData.Beheer.Interface.BusinessEntities.AbstractFactory
{
    /// <summary>
    /// Generic Concrete entity. 
    /// </summary>
    /// <typeparam name="TBeheerService"></typeparam>
    public class BeheerContext<TBeheerService> : IBeheerContextEntity
        where TBeheerService:IBeheerService,new()
    {
        private TBeheerService m_MyBeheerService;

        public BeheerContext()
        {
            m_MyBeheerService=new TBeheerService();
        }

        public string Tablename { get; set; }

        public int Id { get; set; }

        public string DataKeyName { get; set; }
        public IList<BeheerContextEntity> Details { get; set; }

        public ParentKeyEntity Parent { get; set; }
    }

}