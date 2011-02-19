using System;
using MatStudio.ExceptionHandler;

namespace MatStudio.DesignPatterns
{

 // The Component
    public class Component<T> : IComponent<T>
    {
        public string Name { get; set; }
        public T Uid { get; set; }

        public Component(T uid, string name)
        {
            Name = name;
            Uid = uid;
        }
        /// <summary>
        /// Composite pattern does not allow this operation.
        /// </summary>
        /// <param name="c"></param>
        public void Add(IComponent<T> c)
        {
            Console.WriteLine(String.Format("Cannot add to {0}.", c.Uid));
            throw new CheckedException(ErrorType.ProcessFailure, "Composite pattern does not allow this operation.");
        }
        /// <summary>
        ///  Composite pattern does not allow this operation.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public IComponent<T> Remove(T s)
        {
            IComponent<T> component;
            try
            {
                component = s as IComponent<T>;
                if (component != null) Console.WriteLine(String.Format("Cannot remove {0} directly.", component.Uid));
            }
            catch (Exception ex)
	        {
                Console.WriteLine(String.Format("Cannot remove {0} directly: {1}.", s, ex.Message));
	        }
            throw new CheckedException(ErrorType.ProcessFailure, "Composite pattern does not allow this operation.");
        }
        /// <summary>
        /// Displays the component in a format indicating its level in the composite structure (i.e. its container).
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public string Display(int depth)
        {
            return new String('-', depth) + Name + "\n";
        }

        public IComponent<T> Find(T uid)
        {
            if (uid.Equals(Uid))
                return this;
            return null;
        }
    }
}
