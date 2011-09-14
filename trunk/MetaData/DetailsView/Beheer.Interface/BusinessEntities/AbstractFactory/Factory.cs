using System;
using MetaData.Beheer.Interface.Services;

namespace MetaData.Beheer.Interface.BusinessEntities.AbstractFactory
{
    /// <summary>
    /// Generic Concrete Factory voor de (business)eniteit en zijn context.
    /// </summary>
    /// <typeparam name="TBeheerService"></typeparam>
    public class Factory<TBeheerService> : IFactory<TBeheerService>
        where TBeheerService: IBeheerService, new () 
    {
        public IBeheerContextEntity CreateSuperEntity()
        {
            return new BeheerContext<TBeheerService>();
        }

        public IEntityContext CreateEntityContext()
        {
            return new EntityContext<TBeheerService>();
        }
    }
}