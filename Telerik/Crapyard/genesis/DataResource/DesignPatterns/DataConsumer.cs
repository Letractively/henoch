namespace MyDataConsumer
{
    /// <summary>
    /// Represents the Type Brand(DataConsumer) to be created from  Product A(Database) and Product B(Stream)
    /// </summary>
    public class DataConsumer<TDataConsumer>
        where TDataConsumer : IDataConsumer, new( ) 
    {
        public DataConsumer()
        {
            CreateResources();
        }
        public DataConsumer(string fileName, IDirectory directory)
        {
            CreateResources(fileName, directory);
        }
        public void CreateResources( ) //IFactory<TDataConsumer> factory) 
        {
            IFactory<TDataConsumer> factory = new Factory<TDataConsumer>( );
            MyDatabase = factory.CreateMyDatabase();
            MyStream = factory.CreateMyStream();
            MyDirectories = factory.CreateMyDirectory();
        }
        public void CreateResources(string fileName, IDirectory directories) //IFactory<TDataConsumer> factory) 
        {
            IFactory<TDataConsumer> factory = new Factory<TDataConsumer>();
            MyDatabase = factory.CreateMyDatabase();
            MyStream = factory.CreateMyStream(fileName);
            MyDirectories = directories;
            
        }
        public IMyDatabase MyDatabase { get; private set; }
        public IMyStream MyStream { get; private set; }
        public IDirectory MyDirectories { get; private set; }
    }
}