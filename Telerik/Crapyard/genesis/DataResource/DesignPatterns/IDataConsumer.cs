using System.Collections.ObjectModel;
using DataResource.DesignPatterns;
using System.Collections;
using DataResource;

namespace MyDataConsumer
{
    /// <summary>
    /// Represents the Type Brand(DataConsumer) to be created from  Product A(Database), Product B(Stream) and directories (C)
    /// </summary>
    public interface IDataConsumer
    {
        #region product A
        CollectionBronPaden CreateHierarchicalDatabase(int projectId, string root, bool sof, bool saf);
        CollectionBronPaden GetPathBronBestanden(int projectId, string root, bool sof, bool saf);
        Collection<string> GetBronBestanden(int projectId);
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

        Collection<string> CreateHeader(int projectId, string title, string columnNames);
        #endregion

        #region product B
        bool Open(string fileNaam);
        string ReadLine();
        void Dispose();
        bool Close();
        string FileName { get; set; }
        
        #endregion

        #region product C

        string ResultDir { get; set; }
        string OutputDir { get; set; }
        string DeploymentDirectory { get; set; }
        #region prototype2
        string RootDirectory { get; set; }
        string ImplicDirectory { get; set; }
        string FaalkansDirectory { get; set; }
        string TnoDirectory { get; set; }
        string ToetsPeilenDirectory { get; set; }
        #endregion

        #endregion C

        #region Graphinfo
        ICollection OverschrijdingsKansen { get; }
        IDataPointBase ToetsPeil { get; }
        IDataPointBase PrestatiePeil { get; }
        #endregion
    }
}