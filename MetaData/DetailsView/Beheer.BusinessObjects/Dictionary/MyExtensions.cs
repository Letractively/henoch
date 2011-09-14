using System;
using System.Collections.Generic;

namespace Beheer.BusinessObjects.Dictionary
{
    public static class MyExtensions
    {
        public static void Update<T>(this IEnumerable<T> source, params Action<T>[] updates)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (updates == null)
                throw new ArgumentNullException("updates");

            foreach (T item in source)
            {
                foreach (Action<T> update in updates)
                {
                    update(item);
                }
            }
        }
    }
}