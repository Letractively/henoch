using System;
using System.Linq;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelResourcer
{
    public class Tree<TKey,TValue> : IEnumerable
    {

        // 1) Define a delegate type.
        public delegate void TreeHandler(string msgForCaller);
        // 2) Define a member variable of this delegate.
        private static TreeHandler _listOfHandlers;
        // 3) Add registration function for the caller.
        public void RegisterWithTree(TreeHandler methodToCall)
        {
            //multicasting support
            _listOfHandlers += methodToCall;
        }
        public void UnRegisterWithTree(TreeHandler methodToCall)
        {
            _listOfHandlers -= methodToCall;
        }
        public static ConcurrentQueue<TValue> Queue;
        public Tree<TKey, TValue> Left, Right;
        public Tree<TKey, TValue>[] NTree;
        public TValue Data;

        private string _UID;

        public string UID
        {
            get
            {
                if (string.IsNullOrEmpty(_UID))
                    _UID = Guid.NewGuid().ToString();

                return _UID;
            }
        }   
        
        public int Weight { get; set; }

        static Tree()
        {
            Queue = new ConcurrentQueue<TValue>();
        }
        public static IEnumerable<Tree<TKey, TValue>> Iterate(Tree<TKey,TValue> head)
        {
            for (Tree<TKey,TValue> i = head; i != null; i = i.Right)
            {
                yield return i;
            }
        }
        public static IEnumerable<TValue> Iterate<TValue>(
            Func<TValue> initialization, Func<TValue, bool> condition, Func<TValue, TValue> update)
        {
            for (TValue i = initialization(); condition(i); i = update(i))
            {
                yield return i;
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (Tree<TKey, TValue> i = this; i != null; i = i.Right)
            {
                yield return i;
            }
        }
        public static void WalkParallel<TValue>(Tree<TKey, TValue> root, Action<TValue> action, bool waitAll=false)
        {
            if (root == null) return;
            //LRW wandeling in parallel!
            var t2 = Task.Factory.StartNew(() => WalkParallel(root.Left, action)
                , TaskCreationOptions.AttachedToParent);
            var t3 = Task.Factory.StartNew(() => WalkParallel(root.Right, action)
                , TaskCreationOptions.AttachedToParent);
            var t1 = Task.Factory.StartNew(() => action(root.Data)
                , TaskCreationOptions.AttachedToParent);
            if (waitAll) Task.WaitAll(t1, t2, t3);
        }

        public static void WalkParallelNTree<TKey, TValue>(Tree<TKey, TValue> root, Action<TValue> action, bool waitAll = false)
        {
            if (root == null) return;

            if (root.NTree == null)
            {
                var t0= Task.Factory.StartNew(() => action(root.Data)
                        , TaskCreationOptions.AttachedToParent);
                if (waitAll) Task.WaitAll(t0);
                return;
            }

            int countNodes = root.NTree.Length;
            Task[] tasks = new Task[countNodes +1 ];
            tasks[countNodes] = Task.Factory.StartNew(() => action(root.Data)
                , TaskCreationOptions.AttachedToParent);

            Parallel.For(0, countNodes, 
                (i) => 
                {
                    tasks[i] = Task.Factory.StartNew(() => WalkParallelNTree(root.NTree[i], action,waitAll));
                }
                );
           
            if (waitAll) Task.WaitAll(tasks);
        }
        public static void WalkNaryTree<TKey, TValue>(Tree<TKey, TValue> root, Action<TValue> action)
        {
            if (root == null) 
                return;

            if (root.NTree == null)
            {
                action(root.Data);
                return;
            }
            int countNodes = root.NTree.Length;
            action(root.Data);

            for (int i = 0; i < countNodes; i++)
            {
                //travelsal of children.
                WalkNaryTree(root.NTree[i], action);
            }

        }
        public static void WalkClassic<TValue>(Tree<TKey, TValue> root, Action<TValue> action)
        {
            if (root == null) return;
            //LRW wandeling!
             WalkClassic(root.Left, action);
             WalkClassic(root.Right, action);
            action(root.Data);
        }
        public static void WalkClassic<TValue>(Tree<TKey, TValue> root)
        {
            if (root == null) return;
            //LRW wandeling!
            WalkClassic(root.Left);
            WalkClassic(root.Right);
            _listOfHandlers(root.Data.ToString());            
        }
        public static IList<TValue> GetParents(TValue node, IDictionary<TValue, IList<TValue>> linkedList)
        {
            var parents = from pair in linkedList
                          where pair.Value.Where(val => val.Equals(node)).FirstOrDefault() != null
                          select pair.Key;

            return parents.ToList<TValue>();
        }
        public static IList<TValue> GetChildren(TValue node, IDictionary<TValue, IList<TValue>> linkedList)
        {
            #region return null for root values : null, empty
            string defaultVar = default(string);
            if (node == null)
                return null;
            else
            {
                defaultVar = node.ToString();
                if (string.IsNullOrEmpty( defaultVar))
                    return null;
            }
            #endregion

            IList<TValue> list;

            linkedList.TryGetValue(node, out list);
            if (list == null)
                list = new List<TValue>();

            return list;
        }
        public static Tree<TKey, TValue> CreateNTree(TValue node, IDictionary<TValue, IList<TValue>> linkedList)
        {
            var parents = GetParents(node, linkedList);

            return null;
        }


        /// <summary>
        /// Ambiguous
        /// </summary>
        /// <param name="root"></param>
        /// <param name="treeHandler"></param>
        /// <param name="waitAll"></param>
        //public static void WalkParallel(Tree<TKey, TValue> root, TreeHandler treeHandler, bool waitAll = false)
        //{
        //    if (root == null) return;

        //    //LRW wandeling in parallel!
        //    var t2 = Task.Factory.StartNew(() => WalkParallel(root.Left, treeHandler)
        //        , TaskCreationOptions.AttachedToParent);
        //    var t3 = Task.Factory.StartNew(() => WalkParallel(root.Right, treeHandler)
        //        , TaskCreationOptions.AttachedToParent);
        //    var t1 = Task.Factory.StartNew(() => _listOfHandlers(string.Format("Handling node {0}", root.Data))
        //        , TaskCreationOptions.AttachedToParent);
        //    if (waitAll) Task.WaitAll(t1, t2, t3);
        //}
    }
    /// <summary>
    /// Taskduration uses DateTime.
    /// Taskduration2 uses Stopwatch.
    /// </summary>
    public struct TaskInfo
    {
        public long Taskduration
        {
            get
            {
                return TaskEnded - TaskStarted;
            }
        }
        public long Taskduration2
        {
            get
            {
                return TaskEnded2 - TaskStarted2;
            }
        }
        public string TaskDescription;

        public long TaskEnded;
        public long TaskStarted;

        public long TaskStarted2;
        public long TaskEnded2;
    }
}