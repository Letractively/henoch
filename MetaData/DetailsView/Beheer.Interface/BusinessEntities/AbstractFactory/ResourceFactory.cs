using MetaData.Beheer.Interface.Services;

namespace MetaData.Beheer.Interface.BusinessEntities.AbstractFactory
{
    public class ResourceFactory<TBeheerService>
        where TBeheerService : IBeheerService, new( )
    {
        public IEntityContext Context { get; private set; }

        public IBeheerContextEntity BeheerEntity { get; private set; }

        public void CreateResource()
        {
            IFactory<TBeheerService> factory = new Factory<TBeheerService>();
            BeheerEntity = factory.CreateSuperEntity();
            Context = factory.CreateEntityContext();
        }
    }
}