using System.Collections.ObjectModel;

namespace DataResource.Patterns
{
    public interface IMyDatabase 
    {
        bool SaveBronBestand(int gegevensetId, string naam);
        Collection<string> ReadFile(string filename);
    }
}