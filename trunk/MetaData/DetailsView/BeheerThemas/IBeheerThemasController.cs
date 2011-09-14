using System.Collections.Generic;
using System.Collections.ObjectModel;
using MetaData.BeheerThemas.BusinessEntities;

namespace MetaData.BeheerThemas
{
    public interface IBeheerThemasController
    {
        
        IList<Thema> GetThemaTable();

        bool AddThemaCalled { get; set; }
        void AddThema(Thema thema);

        bool DeleteThemaCalled { get; set; }
        void DeleteThema(Thema thema);

        bool UpdateThemaCalled { get; set; }
        void UpdateThema(Thema thema);
    }
}
