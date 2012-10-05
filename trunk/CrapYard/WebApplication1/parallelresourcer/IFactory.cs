

namespace Dictionary.System
{
                                                        // ReSharper disable UnusedTypeParameter
    public interface IFactory<TKeyValue, TResource>
                                                        // ReSharper restore UnusedTypeParameter
        where TResource : IResource 
    {
        ITree<TKeyValue> CreateTree();
    }
}