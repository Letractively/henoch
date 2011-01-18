using System;
using System.Collections.ObjectModel;
using DataResource.DesignPatterns;

namespace MyDataConsumer
{
    public class MyDatabase<TDataConsumer> : IMyDatabase
        where TDataConsumer : IDataConsumer, new() 
    {
        private readonly TDataConsumer MyDataConsumer;
        public MyDatabase( ) 
        {
            MyDataConsumer = new TDataConsumer();
            ///Code coverage tool cannot verify
        }

        #region Implementation of IMyDatabase

        public CollectionBronPaden CreateHierarchicalDatabase(int projectId, string root, bool sof, bool saf)
        {
            return MyDataConsumer.CreateHierarchicalDatabase(projectId,root,sof,saf);
        }

        public CollectionBronPaden GetPathBronBestanden(int projectId, string root, bool sof, bool saf)
        {
            return MyDataConsumer.GetPathBronBestanden(projectId, root, sof, saf);
        }

        public Collection<string> GetBronBestanden(int projectId)
        {
            return MyDataConsumer.GetBronBestanden(projectId);
        }

        public bool SaveBronBestand(int gegevensetId, string naam)
        {
            return MyDataConsumer.SaveBronBestand(gegevensetId,naam);
        }

        public Collection<string> ReadFile(string filename)
        {
            return MyDataConsumer.ReadFile(filename);
        }

        public Collection<string> CreateHeader(int projectId, string title, string columnNames)
        {
            return MyDataConsumer.CreateHeader(projectId, title,columnNames);
        }

        #endregion
    }
}