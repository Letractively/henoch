namespace DataResource.Patterns
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
        public DataConsumer(string fileName)
        {
            CreateResources(fileName);
        }
        public void CreateResources( ) //IFactory<TDataConsumer> factory) 
        {
            IFactory<TDataConsumer> factory = new Factory<TDataConsumer>( );
            MyDatabase = factory.CreateMyDatabase();
            MyStream = factory.CreateMyStream();
        }
        public void CreateResources(string fileName) //IFactory<TDataConsumer> factory) 
        {
            IFactory<TDataConsumer> factory = new Factory<TDataConsumer>();
            MyDatabase = factory.CreateMyDatabase();
            MyStream = factory.CreateMyStream(fileName);
           
        }
        public IMyDatabase MyDatabase { get; private set; }
        public IMyStream MyStream { get; private set; }
    }
}