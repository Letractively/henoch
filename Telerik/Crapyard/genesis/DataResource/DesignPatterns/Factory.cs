using System;
using DataResource.DesignPatterns;

namespace MyDataConsumer
{
    // Concrete Factories (both in the same one)
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

        public IDirectory CreateMyDirectory()
        {
            return new MyDirectory<TDataConsumer>();
        }



        public DataResource.IGraphInfo CreateGraphInfo()
        {
            return new DataResource.GraphInfo<TDataConsumer>();
        }
    }

    public class GraphInfoFactory
    {
        public DataResource.IGraphInfo CreateGraphInfo()
        {
            return new DataResource.GraphInfoSimple();
        }
    }
}
