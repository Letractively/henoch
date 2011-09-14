using System.Collections.Generic;
using MetaData.BeheerThemas.BusinessEntities;
using MetaData.BeheerThemas.Interface.DataResource;

namespace MetaData.BeheerThemas.Views
{
    public interface IBeheerThemasView
    {
        IList<Thema> ThemaTable { set;}
    }
}




