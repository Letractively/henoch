using System;
using MetaData.Beheer.Interface.BusinessEntities;
using MetaData.Beheer.Interface.BusinessEntities.AbstractFactory;

namespace MetaData.BeheerThemas.BusinessEntities
{
    public class Thema : IThema
    {
        //public long Id { get; set; }

        public long Id { get; set; }
        public string ThemaNaam { get; set; }
        public ValueType Created { get; set; }
        
    }
}
