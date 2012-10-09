namespace Dictionary.System
{
    /// <summary>
    /// Represents the Type Brand(DataConsumer) to be created from  Product A(Database) and Product B(Stream)
    /// </summary>
    public class Resource<TKeyValue, TResource>
        where TResource : IResource, new( ) 
    {
        public Resource()
        {
            CreateResources();
        }
        public void CreateResources() //IFactory<TResource> factory) 
        {
            IFactory<TKeyValue, TResource> factory = new Factory<TKeyValue, TResource>();

        }
    }
}