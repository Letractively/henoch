using System.Collections.Generic;
using MetaData.BeheerThemas.BusinessEntities;

namespace MetaData.BeheerThemas.Services
{
    public interface IBeheerThemasService
    {
        IList<Thema> GetThemaTable();
        void AddThema(Thema thema);
        void DeleteThema(Thema thema);
        void UpdateThema(Thema thema);
    }
}