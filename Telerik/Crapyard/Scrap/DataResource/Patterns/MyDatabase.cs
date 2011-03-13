using System;
using System.Collections.ObjectModel;

namespace DataResource.Patterns
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


        public bool SaveBronBestand(int gegevensetId, string naam)
        {
            return MyDataConsumer.SaveBronBestand(gegevensetId,naam);
        }

        public Collection<string> ReadFile(string filename)
        {
            return MyDataConsumer.ReadFile(filename);
        }

        #endregion
    }
}