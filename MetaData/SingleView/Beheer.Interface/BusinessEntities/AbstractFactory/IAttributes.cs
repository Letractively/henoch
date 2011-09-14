using System.Collections.Generic;

namespace MetaData.Beheer.Interface.BusinessEntities.AbstractFactory
{
    /// <summary>
    /// Representeert de attributen van een entiteit.
    /// </summary>
    public interface IAttributes
    {
        //TODO: .NET 4.0 ondersteunt tuples, variance (generics). Property set moet een set van generieke tuples worden.
        IDictionary<string, AttributeValue> Attributes { get; set; }
    }
}