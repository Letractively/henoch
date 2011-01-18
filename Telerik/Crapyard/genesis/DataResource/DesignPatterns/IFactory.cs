using DataResource.DesignPatterns;
using DataResource;

namespace MyDataConsumer
{
                                                        // ReSharper disable UnusedTypeParameter
    public interface IFactory<TDataConsumer>
                                                        // ReSharper restore UnusedTypeParameter
        where TDataConsumer : IDataConsumer 
    {
        IMyDatabase CreateMyDatabase();
        IMyStream CreateMyStream();
        IMyFileSystem CreateMyFileSystem();
        IMyStream CreateMyStream(string fileName);
        IDirectory CreateMyDirectory();
        IGraphInfo CreateGraphInfo();
    }
}