using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Dictionary.System.Caching
{
    /// <summary>
    /// Encapsulates all cachereferences for handling several issues like multithreading/locking.
    /// AMS must use the enterpriselibrary for caching.
    /// 
    /// </summary>
    /// <typeparam name="T">represents the return type of the caching function</typeparam>
    public sealed class MyCache<T> : ICacheManager
    {
        private static readonly MyCache<T> Instance = new MyCache<T>();
        private readonly ReaderWriterLockSlim mCacheLock = new ReaderWriterLockSlim();

        private readonly CacheManager mCacheManager = CacheFactory.GetCacheManager() as CacheManager;

        /// <summary>
        /// represents a list of functions which fills the cache.
        /// </summary>
        private readonly IDictionary<string, Func<IQueryable<T>>> mCachingFunctions;
        /// <summary>
        /// Represents the jobqueue of cache functions.
        /// </summary>
        private readonly Queue<Action> mProducerConsumerQueue = new Queue<Action>();
        // Private Constructor
        private MyCache()
        {
            //by design
            if (mCachingFunctions == null)
                mCachingFunctions = new Dictionary<string, Func<IQueryable<T>>>();
            if(mProducerConsumerQueue==null)
                mProducerConsumerQueue = new Queue<Action>();
        }

        // Private object instantiated with private constructor
        // Public static property to get the object
        public static MyCache<T> CacheManager
        {
            get { return Instance; }
        }

        #region "Implementation of ICacheManager"

        public void Add(string pKey, object pValue)
        {
            try
            {
                mCacheLock.EnterWriteLock();
                mCacheManager.Add(pKey, pValue);                
            }
            finally
            {
                mCacheLock.ExitWriteLock();
            }
        }

        public void Add(string pKey, object pValue, CacheItemPriority pScavengingPriority,
                        ICacheItemRefreshAction pRefreshAction, params ICacheItemExpiration[] pExpirations)
        {
            try
            {
                mCacheLock.EnterWriteLock();
                mCacheManager.Add(pKey, pValue, pScavengingPriority, pRefreshAction, pExpirations);
            }
            finally
            {
            mCacheLock.ExitWriteLock();                
            }
        }

        public bool Contains(string pKey)
        {
            bool _Result;
            try
            {
                mCacheLock.EnterReadLock();
                _Result = mCacheManager.Contains(pKey);
            }
            finally
            {
                mCacheLock.ExitReadLock();
            }
            return _Result;
        }

        public void Flush()
        {
            try
            {
                mCacheLock.EnterWriteLock();
                mCacheManager.Flush();               
            }
            finally
            {
                 mCacheLock.ExitWriteLock();
            }
        }

        public object GetData(string pKey)
        {
            object _Result;
            try
            {
                mCacheLock.EnterReadLock();
                _Result = mCacheManager.GetData(pKey);                
            }
            finally
            {
                mCacheLock.ExitReadLock();
            }
            return _Result;
        }

        public void Remove(string pKey)
        {
            try
            {
                mCacheLock.EnterWriteLock();
                mCacheManager.Remove(pKey);               
            }
            finally
            {
                mCacheLock.ExitWriteLock();
            }
        }

        public int Count
        {
            get
            {
                int _Result;
                try
                {
                    mCacheLock.EnterReadLock();
                    _Result = mCacheManager.Count;
                }
                finally
                {
                    mCacheLock.ExitReadLock();
                }

                return _Result;
            }
        }

        public object this[string pKey]
        {
            get
            {
                object _Result = null;
                try
                {
                    mCacheLock.EnterReadLock();
                    _Result = mCacheManager[pKey];
                   
                }
                finally
                {
                    mCacheLock.ExitReadLock();
                }

                return _Result;
            }
        }

        /// <summary>
        /// Adds the value after evaluating with and storing the pFillFunc to the cache. This function is related to the key.
        /// </summary>
        /// <param name="pKey"></param>
        /// <param name="pFillFunc"></param>
        /// <param name="pScavengingPriority"></param>
        public void Add(string pKey, Func<IQueryable<T>> pFillFunc, CacheItemPriority pScavengingPriority)
        {
            //EventLog.WriteEntry("Adding Cache-item", String.Format("Key: {0} time: {1}", pKey, DateTime.Now.Ticks),
            //                    EventLogEntryType.Information);

            try
            {
                mCacheLock.EnterWriteLock();
                var _Found = mCachingFunctions.Where(p => p.Key.Equals(pKey)).SingleOrDefault();

                if (_Found.Key == null)
                {
                    //store the caching function.
                    mCachingFunctions.Add(pKey, pFillFunc);
                }
                else
                {
                    //re-add
                    mCachingFunctions.Remove(pKey);
                    mCachingFunctions.Add(pKey, pFillFunc);
                }
                //execute caching fun
                mCacheManager.Add(pKey, pFillFunc(), CacheItemPriority.High, null, null);
            }
            finally
            {
                mCacheLock.ExitWriteLock();
            }   
        }

        /// <summary>
        /// Removes the the current cach based on key and re-adds the values by 
        /// using the function related to cachekey.
        /// </summary>
        /// <param name="pCacheKey"></param>
        /// <exception cref="ArgumentException"></exception>
        //public void Reload(CacheKey pCacheKey)
        //{
        //    Type _TypeParameterType = typeof (T);
        //    Func<IQueryable<T>> _FillFunc;

        //    try
        //    {
        //        mCacheLock.EnterWriteLock();
        //        bool _Found = mCachingFunctions.TryGetValue(pCacheKey.ToString(), out _FillFunc);

        //        if (_Found)
        //        {
        //            mCacheManager.Remove(pCacheKey.ToString());
        //            mCacheManager.Add(pCacheKey.ToString(), _FillFunc(), CacheItemPriority.High, null, null);
        //        }
        //        else
        //        {
        //            throw new ArgumentException(
        //                string.Format(
        //                    "Related function for reloading cache object does not exist for key : {0} and type {1}",
        //                    pCacheKey, _TypeParameterType));
        //        }   
        //    }
        //    finally 
        //    {
        //        mCacheLock.ExitWriteLock();
        //    }      
        //}

        //public IQueryable<T> GetData(CacheKey pKey)
        //{
        //    //By design use generics
        //    IQueryable<T> _Result = null;

        //    try
        //    {
        //        mCacheLock.EnterReadLock();
        //        object _Data = mCacheManager.GetData(pKey.ToString());

        //        if (_Data != null)
        //        {
        //            _Result = mCacheManager.GetData(pKey.ToString()) as IQueryable<T>;
        //        }
        //    }
        //    finally
        //    {
        //        mCacheLock.ExitReadLock(); 
        //    }
                                                 
        //    return _Result;
        //}

        private void ExecuteAllWriteTasks(Action pReadTask)
        {
            while (true)                        // Keep consuming until
            {                                   // told otherwise.
                Action _ActionItem;
                lock (mCacheLock)
                {
                    _ActionItem = mProducerConsumerQueue.Dequeue();
                }
                if (_ActionItem == null)
                {
                    pReadTask();// read cache.
                    Monitor.PulseAll(mCacheLock);
                    return;         // This signals our exit.
                }

                _ActionItem();                           // Execute item.
            }
        }

        #endregion
    }
}
