using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignPatterns
{
    /// <summary>
    /// Interfaces to an enitity Component as part of the composite pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IComponent <T> 
    {
        /// <summary>
        /// Adds the component to its list."/>
        /// </summary>
        /// <param name="c"></param>
        void Add(IComponent<T> component);
        /// <summary>
        /// Removes the component from its list.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        IComponent<T> Remove(T component);
        /// <summary>
        /// Finds the component using the uid in its list.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        IComponent<T> Find(T component);
        /// <summary>
        /// Gives feedback about the position of the component in the hierarchy.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        string Display(int depth);
        /// <summary>
        /// The name of the component.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Uniquely identifies the Component.
        /// </summary>
        T Uid { get; set; }
    }
}
