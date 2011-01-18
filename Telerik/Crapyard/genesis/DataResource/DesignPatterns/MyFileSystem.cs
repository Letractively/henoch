using System;
using System.Collections.ObjectModel;
using DataResource.DesignPatterns;

namespace MyDataConsumer
{
    public class MyFileSystem<TDataConsumer> : IMyFileSystem
        where TDataConsumer : IDataConsumer,new() 
    {
        private readonly TDataConsumer MyDataConsumer;

        public MyFileSystem()
        {
            MyDataConsumer = new TDataConsumer();
            ///Code coverage tool cannot verify
        }
        public CollectionBronPaden CreateHierarchicalDatabase(int projectId, string root, bool sof, bool saf)
        {
            return MyDataConsumer.CreateHierarchicalDatabase(projectId, root, sof, saf);
        }

        public CollectionBronPaden GetPathBronBestanden(int projectId, string root, bool sof, bool saf)
        {
            return MyDataConsumer.GetPathBronBestanden(projectId, root, sof, saf);
        }

        public bool SaveBronBestand(int gegevensetId, string naam)
        {
             return MyDataConsumer.SaveBronBestand(gegevensetId, naam);
        }

        public Collection<string> ReadFile(string filename)
        {
            return MyDataConsumer.ReadFile(filename);
        }

        public int SaveToDatFile(string datFile, double[][] matrix, int rows, int colums, string format, int countSpacesPostFix)
        {
            return MyDataConsumer.SaveToDatFile(datFile,matrix,rows,colums,format,countSpacesPostFix);
        }
    }
}