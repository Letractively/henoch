using System.Collections.ObjectModel;
using DataResource.DesignPatterns;

namespace MyDataConsumer
{
    public interface IMyDatabase 
    {
        CollectionBronPaden CreateHierarchicalDatabase(int projectId, string root, bool sof, bool saf);
        CollectionBronPaden GetPathBronBestanden(int projectId, string root, bool sof, bool saf);
        Collection<string> GetBronBestanden(int projectId);
        bool SaveBronBestand(int gegevensetId, string naam);
        Collection<string> ReadFile(string filename);
        Collection<string> CreateHeader(int projectId, string title, string columnNames);
    }
}