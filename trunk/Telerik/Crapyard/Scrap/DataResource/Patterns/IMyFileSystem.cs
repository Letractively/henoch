using System.Collections.ObjectModel;

namespace DataResource.Patterns
{
    public interface IMyFileSystem
    {

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