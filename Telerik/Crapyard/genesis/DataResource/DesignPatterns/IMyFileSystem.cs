using System.Collections.ObjectModel;

namespace DataResource.DesignPatterns
{
    public interface IMyFileSystem
    {
        /// <summary>
        /// Creates the hierarchical db for this locatie.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        CollectionBronPaden CreateHierarchicalDatabase(int projectId, string root, bool sof, bool saf);
        /// <summary>
        /// Gets the path of the bronbestanden.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="root"></param>
        /// <param name="sof"></param>
        /// <param name="saf"></param>
        /// <returns></returns>
        CollectionBronPaden GetPathBronBestanden(int projectId, string root, bool sof, bool saf);

        bool SaveBronBestand(int gegevensetId, string naam);
        Collection<string> ReadFile(string filename);

        /// <summary>
        /// Saves the matrix to a datFile. countSpacesPostFix indicates the number of spaces 
        /// between the values per line.
        /// </summary>
        /// <param name="datFile"></param>
        /// <param name="matrix"></param>
        /// <param name="rows"></param>
        /// <param name="colums"></param>
        /// <param name="format"></param>
        /// <param name="countSpacesPostFix"></param>
        /// <returns></returns>
        int SaveToDatFile(string datFile, double[][] matrix,
                          int rows, int colums, string format, int countSpacesPostFix);
    }
}
