using System;
namespace Dictionary.BusinessObjects
{
    public interface IBusinessObject<TKey, TValue>
    {
        TKey Key { get; set; }
        string Name { get; set; }
        TValue Value { get; set; }
    }
}
