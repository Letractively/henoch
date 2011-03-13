using System.Collections.ObjectModel;

namespace DataResource.Patterns
{
    /// <summary>
    /// Represents the Type Brand(DataConsumer) to be created from  Product A(Database), Product B(Stream) and directories (C)
    /// </summary>
    public interface IDataConsumer
    {
        #region product A
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
        #endregion

        #region product B
        bool Open(string fileNaam);
        string ReadLine();
        void Dispose();
        bool Close();
        string FileName { get; set; }
        
        #endregion


    }
}