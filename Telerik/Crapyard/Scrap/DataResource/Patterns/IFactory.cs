namespace DataResource.Patterns
{
    public interface IFactory<TDataConsumer>
        // ReSharper restore UnusedTypeParameter
        where TDataConsumer : IDataConsumer 
    {
        IMyDatabase CreateMyDatabase();
        IMyStream CreateMyStream();
        IMyFileSystem CreateMyFileSystem();
        IMyStream CreateMyStream(string fileName);
    }
}