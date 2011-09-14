using MetaData.Beheer.Interface.Services;

namespace MetaData.Beheer.Interface.BusinessEntities.AbstractFactory
{
    public interface IFactory<TBeheerService>
        where TBeheerService : IBeheerService
    {
        IBeheerContextEntity CreateSuperEntity();
        IEntityContext CreateEntityContext();
    }
}