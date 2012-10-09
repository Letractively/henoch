using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Dictionary
{
    /// <summary>
    /// Singleton Pattern Judith Bishop Nov 2007
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton <T> where T : class, new( )
    {
        Singleton() { }
        class SingletonCreator
        {
            static SingletonCreator() { }
            // Private object instantiated with private constructor
            internal static readonly T instance = new T();
        }
        public static T UniqueInstance
        {
            get { return SingletonCreator.instance; }
        }
    }
}
