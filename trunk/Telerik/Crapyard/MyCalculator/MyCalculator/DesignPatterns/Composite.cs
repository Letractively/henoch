﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DesignPatterns;
using System.Collections;
using System.Diagnostics;

namespace DesignPatterns
{
    #region Comparer
    public class ComponentComparer<T> : IEqualityComparer<T>
    {
        #region IEqualityComparer<Component<T>> Members

        //public bool Equals(Component<T> x, Component<T> y)
        //{
        //    // Check whether the compared objects reference the same data.
        //    if (Object.ReferenceEquals(x, y)) return true;

        //    // Check whether any of the compared objects is null.
        //    if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
        //        return false;

        //    // Check whether the products' properties are equal.
        //    return x.Uid.GetHashCode() == y.Uid.GetHashCode();

        //}

        //public int GetHashCode(Component<T> component)
        //{
        //    // Check whether the object is null.
        //    if (Object.ReferenceEquals(component, null)) return 0;

        //    // Get the hash code for the waardeid field if it is not null.
        //    int hashComponent = component.Uid == null ? 0 : component.Uid.GetHashCode();

        //    return hashComponent;
        //}

        #endregion

        #region IEqualityComparer<T> Members

        public bool Equals(T x, T y)
        {
            IComponent<T> componentX = new Component<T>(x, "x");
            IComponent<T> componentY = new Component<T>(y, "y");

            // Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            // Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
           
            // Check whether the products' properties are equal.
            return componentX.Uid.GetHashCode() == componentY.Uid.GetHashCode();
            ;
        }

        public int GetHashCode(T Uid)
        {
            IComponent<T> componentX = new Component<T>(Uid, "Uid");
            // Check whether the object is null.
            if (Object.ReferenceEquals(componentX, null)) return 0;

            // Get the hash code for the waardeid field if it is not null.
            int hashComponent = componentX.Uid == null ? 0 : componentX.Uid.GetHashCode();

            return hashComponent;
        }

        #endregion
    }
    #endregion
    /// <summary>
    /// Represents the composite pattern, i.e. a container.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Composite<T> : IComponent<T>
    {
        Dictionary<T , IComponent<T>> m_List;
        IComponent<T> m_Container = null;

        /// <summary>
        /// Contains a dictionary of components (container).
        /// Uid is the unique identifier.
        /// Name is the name of the container.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        public Composite(T uid, string name)
        {
            m_List = new Dictionary<T, IComponent<T>>();

            //IComponent<T> component = uid as Component<T>; (component==null)
            IComponent<T> component = new Component<T>(uid, name);
            Debug.Assert(component != null);
            m_List.Add(component.Uid, component);
            m_Container = this;
        }

        ~Composite()
        {
            m_List.Clear();
        }


        #region IComponent<object> Members

        public void Add(IComponent<T> component)
        {
            m_List.Add(component.Uid, component);
        }

        /// <summary>
        /// Removes the component from its container.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public IComponent<T> Remove(T uidComponent)
        {
            m_Container = this;
            IComponent<T> component = m_Container.Find(uidComponent);

            if (m_Container != null)
            {
                (m_Container as Composite<T>).m_List.Remove(component.Uid);
                return m_Container;
            }
            else
                return this;

        }
        /// <summary>
        /// Finds the component with ID uid.
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IComponent<T> Find(T uid)
        {
            m_Container = this;
            if (Uid.Equals(uid)) return this;
            IComponent<T> found = null;

            m_List.TryGetValue(uid, out found);

            return found;
        }
        /// <summary>
        /// Displays items in a format indicating their level in the composite structure.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public string Display(int depth)
        {
            StringBuilder strBuilder = new StringBuilder(new String('-', depth));
            strBuilder.Append("Set " + Name + " length :" + m_List.Count() + "\n");
            foreach (var  component in m_List)
            {
                strBuilder.Append(component.Value.Display(depth + 2));
            }
            return strBuilder.ToString();
        }

        public string Name { get; set; }

        public T Uid { get; set; }

        #endregion
        /// <summary>
        /// Associates to its component in its list.
        /// </summary>
        public Dictionary<T, IComponent<T>> Components
        {
            get
            {
               return m_List;
            }
            set
            {
                m_List = value;
            }
        }


    }
}
