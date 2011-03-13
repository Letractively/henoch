using System;
using MyDataConsumer;

namespace DataResource.Patterns
{
    public class Factory<TDataConsumer> : IFactory<TDataConsumer>
        where TDataConsumer : IDataConsumer, new( ) 
    {
        public IMyDatabase CreateMyDatabase( ) 
        {
            return new MyDatabase<TDataConsumer>( );
        }
        public IMyStream CreateMyStream( ) {
            return new MyStream<TDataConsumer>( );
        }

        public IMyFileSystem CreateMyFileSystem()
        {
            return new MyFileSystem<TDataConsumer>();
        }

        public IMyStream CreateMyStream(string fileName)
        {
            return new MyStream<TDataConsumer>(fileName);
        }

    }
}