using System.Collections.Generic;

namespace MetaData.BeheerThemas.Interface.Services
{
    public interface IBeheerThemasService
    {
        IList<Thema> GetThemaTable();
    }
}