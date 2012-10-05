using System;

namespace Dictionary.System
{
    // Concrete Factories (both in the same one)
    public class Factory<TKeyValue, TResource> : IFactory<TKeyValue, TResource>
    where TResource : IResource, new()
    {
        public ITree<TKeyValue> CreateTree()
        {
            return new Tree<TKeyValue>();
        }
    }
}
