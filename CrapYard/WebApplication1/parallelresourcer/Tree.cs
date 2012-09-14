using System;
using System.Linq;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelResourcer
{
    public class Tree<TKeyValue> : IEnumerable
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
        public static ConcurrentQueue<TKeyValue> Queue;
        public Tree<TKeyValue> Left, Right;
        public IList<Tree<TKeyValue>> NTree;
        public TKeyValue Data;

        /// <summary>
        /// Indicates whether the node is unique.
        /// </summary>
        public bool IsUnique{ get; private set;}
        /// <summary>
        /// List of all nodes keyvalues.
        /// </summary>
        public static ConcurrentDictionary<TKeyValue,TKeyValue> Nodes { get; private set; }

        private TKeyValue _Key;

        /// <summary>
        /// The uniqueness of the key will be also set.
        /// </summary>
        public TKeyValue Key 
        { 
            get
            {
                return _Key;
            }
            set 
            {
                _Key = Key;
               //if (Nodes == null) Nodes = new LinkedList<TKeyValue>();
                if (Nodes.TryAdd(_Key, _Key))
                    IsUnique = true;
                else
                    IsUnique = false;
            }
        }

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
            Queue = new ConcurrentQueue<TKeyValue>();
            Nodes = new ConcurrentDictionary<TKeyValue, TKeyValue>();

        }
        public Tree()
        {
            IsUnique = true;
        }
        public static IEnumerable<Tree<TKeyValue>> Iterate(Tree<TKeyValue> head)
        {
            for (Tree<TKeyValue> i = head; i != null; i = i.Right)
            {
                yield return i;
            }
        }
        public static IEnumerable<TKeyValue> Iterate<TKeyValue>(
            Func<TKeyValue> initialization, Func<TKeyValue, bool> condition, Func<TKeyValue, TKeyValue> update)
        {
            for (TKeyValue i = initialization(); condition(i); i = update(i))
            {
                yield return i;
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (Tree<TKeyValue> i = this; i != null; i = i.Right)
            {
                yield return i;
            }
        }
        public static void WalkParallel<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue> action, bool waitAll=false)
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

        public static void WalkParallelNTree<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue> action, bool waitAll = false)
        {
            if (root == null) return;

            if (root.NTree == null)
            {
                var t0= Task.Factory.StartNew(() => action(root.Data)
                        , TaskCreationOptions.AttachedToParent);
                if (waitAll) Task.WaitAll(t0);
                return;
            }

            int countNodes = root.NTree.Count;
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
        public static void WalkNaryTree<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue> action)
        {
            if (root == null) 
                return;

            if (root.NTree == null)
            {
                action(root.Data);
                return;
            }
            int countNodes = root.NTree.Count;
            action(root.Data);

            for (int i = 0; i < countNodes; i++)
            {
                //travelsal of children.
                WalkNaryTree(root.NTree[i], action);
            }

        }
        public static void WalkClassic<TKeyValue>(Tree<TKeyValue> root, Action<TKeyValue> action)
        {
            if (root == null) return;
            //LRW wandeling!
             WalkClassic(root.Left, action);
             WalkClassic(root.Right, action);
            action(root.Data);
        }
        public static void WalkClassic<TKeyValue>(Tree<TKeyValue> root)
        {
            if (root == null) return;
            //LRW wandeling!
            WalkClassic(root.Left);
            WalkClassic(root.Right);
            _listOfHandlers(root.Data.ToString());            
        }
        public static IList<TKeyValue> GetParents(TKeyValue node, IDictionary<TKeyValue, IList<TKeyValue>> dictionary)
        {
            var parents = from pair in dictionary
                          where pair.Value.Where(val => val.Equals(node)).FirstOrDefault() != null
                          select pair.Key;

            return parents.ToList<TKeyValue>();
        }
        public static IList<TKeyValue> GetChildren(TKeyValue node, IDictionary<TKeyValue, IList<TKeyValue>> dictionary)
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

            IList<TKeyValue> list;

            dictionary.TryGetValue(node, out list);
            if (list == null)
                list = new List<TKeyValue>();

            return list;
        }
        /// <summary>
        /// Creates  a N-ary tree. A node has a key-valued pair 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dictionary"></param>
        /// <param name="GetRelations"></param>
        /// <returns></returns>
        public static Tree<TKeyValue> CreateNTree(TKeyValue node, IDictionary<TKeyValue, IList<TKeyValue>> outerdictionary,
                                                    Func<TKeyValue, IDictionary<TKeyValue, IList<TKeyValue>>, IList<TKeyValue>> GetRelations)
        {
            var parents = GetRelations(node, outerdictionary);

            Tree<TKeyValue> nTree = new Tree<TKeyValue>();
            nTree.Key = node;

            int parentCount = parents.Count;

            if (parentCount > 0)
            {
                // create subtrees
                nTree.NTree = new List<Tree<TKeyValue>>();
                foreach (var parent in parents)
                {
                    nTree.NTree.Add(CreateNTree(parent, outerdictionary, GetRelations));
                }
            }
            return nTree;
        }


        /// <summary>
        /// Ambiguous
        /// </summary>
        /// <param name="root"></param>
        /// <param name="treeHandler"></param>
        /// <param name="waitAll"></param>
        //public static void WalkParallel(Tree<TKeyValue> root, TreeHandler treeHandler, bool waitAll = false)
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