using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Repository;

namespace UTS.AMS.TestSuite
{
    /// <summary>
    ///This is a test class for MyCacheTest and is intended
    ///to contain all MyCacheTest Unit Tests.
    /// Current limit: max 10000 cache-operations can concurrently work with each item consisting
    /// of 100000 pairs(int,string)
    ///</summary>
    [TestClass()]
    public class MyCacheTest
    {
        private int loop = 10;
        private Stopwatch stopwatch;
        private long start;

        private TestContext testContextInstance;
        private int _cacheItems = 10;

        /// <summary>
        /// key to perform cache hits
        /// </summary>
        private static string _Key1 = Guid.NewGuid().ToString();

        /// <summary>
        /// key to perform cache hits
        /// </summary>
        private static string _Key2 = Guid.NewGuid().ToString();

        /// <summary>
        /// Indicates the size of a cache item. A size is the length of a array/list/collection.
        /// </summary>
        private const int CACHE_ITEM_SIZE = 1000;

        /// <summary>
        /// indicates the size of jobs which are executed concurrently.
        /// </summary>
        private const int JOB_COUNT_CONCURRENT = 1000;
        private const int JOB_COUNT_SEQUENTIAL = 50;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            MyCache<IList<string>>.CacheManager.Flush();
            Console.WriteLine("Key1 = " + _Key1);
            Console.WriteLine("Key2 = " + _Key2);
            //cache should be empty.
            Assert.AreEqual(0, MyCache<IList<string>>.CacheManager.Count);

        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            MyCache<IList<string>>.CacheManager.Flush();
            Assert.AreEqual(0, MyCache<IList<string>>.CacheManager.Count);
        }
        //

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Console.WriteLine(testContextInstance.TestName);

            GC.Collect();
            GC.WaitForPendingFinalizers();


            start = Environment.TickCount;
            stopwatch = new Stopwatch();
            stopwatch.Start();

            testContextInstance.BeginTimer("stopwatch");
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            testContextInstance.EndTimer("stopwatch");
            long ended = Environment.TickCount;
            stopwatch.Stop();
            long elapased = ended - start;
            DisplayTimerProperties();
            Console.WriteLine("Duration = " + elapased);
        }
        public void DisplayTimerProperties()
        {
            // Display the timer frequency and resolution.
            //if (Stopwatch.IsHighResolution)
            //{
            //    Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
            //}
            //else
            //{
            //    Console.WriteLine("Operations timed using the DateTime class.");
            //}

            //long frequency = Stopwatch.Frequency;
            //Console.WriteLine("Timer frequency in ticks per second = {0}",
            //    frequency);
            long nanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
            Console.WriteLine("Timer is accurate within {0} nanoseconds",
                nanosecPerTick);
        }
        #endregion


        [TestMethod()]
        public void AddTest()
        {
            //Thread.Sleep(1000);
            IList<string> myCollection = new List<string>() { "abc", "xyz" };
            IList<string> myCollection2 = new List<string>() { "abc", "xyz" };

            string pKey = Guid.NewGuid().ToString();

            var myCache = MyCache<IList<string>>.CacheManager; // TODO: Initialize to an appropriate value            
            Action action = () =>
            {
                AddToCache(myCollection, myCache, pKey);
            };

            action();
            
            //Dummies
            var myCacheDummy = MyCache<IQueryable<int>>.CacheManager;
            var dummy = myCacheDummy.GetData(1);
            if (dummy != null)
            {
                var count = dummy.Count();
            }

            string keyForBigCollection = Guid.NewGuid().ToString();// _Key2;
            int collectionSize = 1000;

            CreateItemCollectionAndAddToCache(keyForBigCollection, collectionSize);
            Assert.AreEqual(myCollection, myCache.GetData(pKey));
            Assert.AreNotEqual(myCollection2, myCache.GetData(pKey));
        }

        private static void AddToCache<T>(T myCollection, MyCache<IList<string>> myCache, string pKey)
        {
            object pValue = myCollection;
            CacheItemPriority pScavengingPriority = new CacheItemPriority();
            ICacheItemRefreshAction pRefreshAction = null;
            ICacheItemExpiration[] pExpirations = null;
            myCache.Add(pKey, pValue, pScavengingPriority, pRefreshAction, pExpirations);
        }

        //[TestMethod]
        //public void ReloadTest()
        //{
        //    var myCache = MyCache<SurveyGradingBO>.CacheManager; // TODO: Initialize to an appropriate value            
        //    var myCache2 = MyCache<string>.CacheManager;

        //    Func<IQueryable<SurveyGradingBO>> myFunc = MyFunc;
        //    Func < IQueryable <string>> myFunc2 = MyFunc2;

        //    myCache.Add(CacheKey.Accounts.ToString(), myFunc, CacheItemPriority.High);
        //    myCache.Reload(CacheKey.Accounts);

        //    myCache2.Add(CacheKey.VatCodes.ToString(), MyFunc2, CacheItemPriority.High);
        //    myCache2.Reload(CacheKey.VatCodes);

        //    Assert.AreEqual(myFunc().First().Description, 
        //        myCache.GetData(CacheKey.Accounts).First().Description);
        //    Assert.AreEqual(myFunc2().First(), myCache2.GetData(CacheKey.VatCodes).First());
        //}

        private IQueryable<string> MyFunc2()
        {
            IList<string> myCollection = new List<string>();
            myCollection.Add("test");
            return myCollection.AsQueryable();
        }


        //private IQueryable<SurveyGradingBO> MyFunc()
        //{
        //    IList<SurveyGradingBO> myCollection = new List<SurveyGradingBO>();
        //    myCollection.Add(new SurveyGradingBO(){Description = "test"});
        //    return myCollection.AsQueryable(); 
        //}

        [TestMethod()]
        public void RemoveTest()
        {
            string pKey = Guid.NewGuid().ToString();
            MyCache<IList<string>> myCache = null;

            Action action = () =>
            {
                myCache = GetMyCache();
            };
            action();
            //Console.WriteLine("removing item " + pKey);    

            myCache.Remove(pKey);
            Assert.AreEqual(null, myCache.GetData(pKey));
        }

        private MyCache<IList<string>> GetMyCache()
        {
            IList<string> myCollection = new List<string>() { "abc", "xyz" };
            var myCache = MyCache<IList<string>>.CacheManager; // TODO: Initialize to an appropriate value            
            object pValue = myCollection;
            CacheItemPriority pScavengingPriority = new CacheItemPriority();
            ICacheItemRefreshAction pRefreshAction = null;
            ICacheItemExpiration[] pExpirations = null;
            return myCache;
        }
        [TestMethod]
        public void TestCacheActions()
        {
            Tree<Action>.Queue = new ConcurrentQueue<Action>();
            CreateCacheJobQueue(JOB_COUNT_CONCURRENT);

            Action start;
            Tree<Action>.Queue.TryDequeue(out start);
            var taken = Tree<Action>.CreateSubTree(start);

            var t1 = Task.Factory.StartNew(() =>
            {
                Tree<Action>.WalkParallel(taken, DoAction);
            });

            Task.WaitAll(t1);
        }

        [TestMethod]
        public void TestCacheActionsParallel()
        {
            //create queue of jobs.
            Tree<Action>.Queue = new ConcurrentQueue<Action>();
            CreateCacheJobQueue(JOB_COUNT_CONCURRENT);

            var actions = Tree<Action>.Queue.ToArray();
            int upper = actions.Length;

            var t1 = Task.Factory.StartNew(() =>             
                //execute actions concurrently.
                Parallel.For(0, upper, i =>
                                       {
                                           actions[i]();
                                       }));
            Task.WaitAll(t1);

        }

        [TestMethod]
        public void TestCacheActionsSerial()
        {
            //create queue of jobs.
            Tree<Action>.Queue = new ConcurrentQueue<Action>();
            CreateCacheJobQueue(JOB_COUNT_SEQUENTIAL);

            var actions = Tree<Action>.Queue.ToArray();
            int upper = actions.Length;

            //execute actions concurrently.
            for (int i = 0; i < upper; i++)
            {
                 actions[i]();
                Thread.Sleep(10);
            }                           
        }

        /// <summary>
        /// Creates concurrently 2 jobqueues.
        /// </summary>
        /// <param name="jobCount"></param>
        private void CreateCacheJobQueue(int jobCount)
        {
            var t1 = Task.Factory.StartNew(() => CreateJobQueue(jobCount));
            Task.WaitAll(t1);
        }

        /// <summary>
        /// Test if the tree of jobs contains the jobqueue.
        /// </summary>
        [TestMethod]
        public void TestCreateDummyActions()
        {
            Tree<Action>.Queue = new ConcurrentQueue<Action>();
            CreateDummyJobQueue(100);

            Action start;
            Tree<Action>.Queue.TryDequeue(out start);
            var taken = Tree<Action>.CreateSubTree(start);

            var t1 = Task.Factory.StartNew(() =>
            {
                Tree<Action>.WalkParallel(taken, DoAction);
            });

            Task.WaitAll(t1);
        }

        private void CreateJobQueue(int jobCount)
        {
            Random rand = new Random();
            for (int i = 0; i < jobCount; i++)
            {
                string j = i.ToString();
                Tree<Action>.Queue.Enqueue(() =>
                {                    
                    int blockingTime = rand.Next(1, 5);
                    Thread.Sleep(blockingTime);
                    Console.WriteLine(j + "-Add-" + blockingTime);
                    AddTest();
                    PerformCacheHits();
                });
                Tree<Action>.Queue.Enqueue(() =>
                {
                    int blockingTime = rand.Next(1, 5);
                    Thread.Sleep(blockingTime);
                    Console.WriteLine(j + "-Remove-" + blockingTime);
                    RemoveTest();
                });
            }
        }
        /// <summary>
        /// Performs 2 cachehits: 1 simple + 1 big item
        /// </summary>
        private void PerformCacheHits()
        {
            IList<string> myCollection = new List<string>() { "abc", "xyz" };
            var myCache = MyCache<IList<string>>.CacheManager;
            AddToCache(myCollection, myCache, _Key1);

            string keyForBigCollection = _Key2;
            int collectionSize = CACHE_ITEM_SIZE;

            CreateItemCollectionAndAddToCache(keyForBigCollection, collectionSize);
        }

        private void CreateItemCollectionAndAddToCache(string keyForBigCollection, int collectionSize)
        {
            IDictionary<int, string> myBigCollection = new Dictionary<int, string>();
            for (int i = 0; i < collectionSize; i++)
            {
                myBigCollection.Add(i, Guid.NewGuid().ToString());
            }
            Assert.AreEqual(collectionSize, myBigCollection.Count);
            var myCache2 = MyCache<IList<string>>.CacheManager;
            AddToCache(myBigCollection, myCache2, keyForBigCollection);
        }

        private static void CreateDummyJobQueue(int jobCount)
        {
            Random rand = new Random();

            for (int i = 0; i < jobCount; i++)
            {
                string j = i.ToString();

                Tree<Action>.Queue.Enqueue(() =>
                {
                    int blockingTime = rand.Next(100, 1000);
                    Thread.Sleep(blockingTime);
                    Console.WriteLine(j + "-" + blockingTime);
                    Assert.IsTrue(true);
                });
            }
        }

        private void DoAction(Action theAction)
        {

            //Console.Write("Starting time:" + DateTime.Now.Millisecond + "\t");
            theAction();
        }

        /// <summary>
        /// Create a subtree depending on the queue
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Tree<Action> CreateSubTree(Action parent)
        {
            if (parent == null)
                return null;
            Action majorAction;
            Action minorAction;
            Tree<Action>.Queue.TryDequeue(out minorAction);
            Tree<Action>.Queue.TryDequeue(out majorAction);

            var jobTree = new Tree<Action>
            {
                Left = CreateSubTree(majorAction),
                Right = CreateSubTree(minorAction),
                Data = parent
            };

            return jobTree;
        }
        [Ignore()]
        public void TestWorkers()
        {
            WorkerQueue workers = new WorkerQueue(10);
            IDictionary<int, IList<string>> cacheItems = new Dictionary<int, IList<string>>();
            var myCache = MyCache<IList<string>>.CacheManager;

            //add cache-actions to workqueue.
            for (int i = 0; i < _cacheItems; i++)
            {
                cacheItems.Add(i, new List<string>() { "abc", "xyz" });
                Action action = () =>
                {
                    string pKey = i.ToString();
                    AddToCache(cacheItems[i], myCache, pKey);
                };

                workers.EnqueueItem(action);
            }

            workers.StartWorkers(true);

            for (int i = 0; i < _cacheItems; i++)
            {
                Assert.AreEqual(cacheItems[i], myCache.GetData(i.ToString()));
            }
        }

        [TestMethod()]
        public void FlushTest()
        {
            IList<string> myCollection1 = new List<string>() { "abc", "xyz" };
            IList<string> myCollection2 = new List<string>() { "abcde", "vwxyz" };

            var myCache = MyCache<IList<string>>.CacheManager;

            string pKey1 = "theKey1";
            string pKey2 = "theKey2";

            AddToCache(myCollection1, myCache, pKey1);
            AddToCache(myCollection2, myCache, pKey2);

            myCache.Flush();

            //Assert.AreEqual(null, myCache); //will this work?
            Assert.AreEqual(0, myCache.Count);
            //Assert.AreEqual(null, myCache.GetData(pKey1));
            //Assert.AreEqual(null, myCache.GetData(pKey2));
        }

        [TestMethod()]
        public void RemoveAllTest()
        {
            IList<string> myCollection1 = new List<string>() { "a", "bcd" };
            IList<string> myCollection2 = new List<string>() { "ab", "cde" };
            IList<string> myCollection3 = new List<string>() { "abc", "def" };

            var myCache = MyCache<IList<string>>.CacheManager;

            string pKey1 = "theKey1";
            string pKey2 = "theKey2";
            string pKey3 = "theKey3";

            AddToCache(myCollection1, myCache, pKey1);
            AddToCache(myCollection2, myCache, pKey2);
            AddToCache(myCollection3, myCache, pKey3);

            myCache.Remove(pKey1);
            myCache.Remove(pKey2);
            myCache.Remove(pKey3);

            Assert.AreEqual(null, myCache.GetData(pKey1));
            Assert.AreEqual(null, myCache.GetData(pKey2));
            Assert.AreEqual(null, myCache.GetData(pKey3));
        }

        [TestMethod()]
        public void ExpirationTest()
        {
            IList<string> myCollection1 = new List<string>() { "a", "bcd" };

            var myCache = MyCache<IList<string>>.CacheManager;

            string pKey = Guid.NewGuid().ToString();

            AddToCache(myCollection1, myCache, pKey);
            AbsoluteTime _AbsoluteTime = new AbsoluteTime(TimeSpan.FromMilliseconds(100));
            ICacheItemExpiration[] pExpirations = new ICacheItemExpiration[] { _AbsoluteTime };
            myCache.Add(pKey, myCollection1, CacheItemPriority.Normal, null, pExpirations);
            Assert.AreEqual("a", (myCache.GetData(pKey) as List<string>)[0]);
            Assert.AreEqual(myCollection1, myCache.GetData(pKey));

            Thread.Sleep(120);

            Assert.AreEqual(null, myCache.GetData(pKey));
            Assert.AreNotEqual(myCollection1, myCache.GetData(pKey));
        }
    }
}
