namespace ApplicationTypes.DesignPatterns
{

    // Singleton Pattern Judith Bishop Nov 2007
    // Generic version
    public class Singleton<T> where T : class, new()
    {
        Singleton() { }
        
        public class SingletonCreator
        {
            // ReSharper disable EmptyConstructor
            static SingletonCreator() { }
            // ReSharper restore EmptyConstructor

            // Private object instantiated with private constructor
            internal static readonly T Instance = new T();
        }
        public static T UniqueInstance
        {
            get { return SingletonCreator.Instance; }
        }
    }
}
