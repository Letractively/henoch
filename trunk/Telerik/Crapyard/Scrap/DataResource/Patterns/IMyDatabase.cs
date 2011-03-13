using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataResource.Patterns
{
    public interface IMyDatabase 
    {
        bool SaveBronBestand(int gegevensetId, string naam);
        ICollection<string> ReadFile(string filename);
    }
}