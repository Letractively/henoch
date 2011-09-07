using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UTS.AMS.TestSuite
{
    public class Tree<T> : IEnumerable
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
        public static ConcurrentQueue<T> Queue;
        public Tree<T> Left, Right;
        public T Data;

        public int Weight { get; set; }

        static Tree()
        {
            Queue = new ConcurrentQueue<T>();
        }
        public static IEnumerable<Tree<T>> Iterate(Tree<T> head)
        {
            for (Tree<T> i = head; i != null; i = i.Right)
            {
                yield return i;
            }
        }
        public static IEnumerable<T> Iterate<T>(
            Func<T> initialization, Func<T, bool> condition, Func<T, T> update)
        {
            for (T i = initialization(); condition(i); i = update(i))
            {
                yield return i;
            }
        }

        public IEnumerator GetEnumerator()
        {
            for (Tree<T> i = this; i != null; i = i.Right)
            {
                yield return i;
            }
        }
        public static void WalkParallel<T>(Tree<T> root, Action<T> action, bool waitAll = false)
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

        public static void WalkClassic<T>(Tree<T> root, Action<T> action)
        {
            if (root == null) return;
            //LRW wandeling!
            WalkClassic(root.Left, action);
            WalkClassic(root.Right, action);
            action(root.Data);
        }
        public static void WalkClassic<T>(Tree<T> root)
        {
            if (root == null) return;
            //LRW wandeling!
            WalkClassic(root.Left);
            WalkClassic(root.Right);
            _listOfHandlers(root.Data.ToString());
        }
        /// <summary>
        /// Create a subtree depending on the queue
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Tree<T> CreateSubTree(T parent)
        {
            if (parent == null)
                return null;
            T majorAction;
            T minorAction;
            int count = Queue.Count;
            Queue.TryDequeue(out minorAction);
            Queue.TryDequeue(out majorAction);
            
            var jobTree = new Tree<T>
            {
                Weight = count,
                Left = CreateSubTree(majorAction),
                Right = CreateSubTree(minorAction),
                Data = parent
            };

            return jobTree;
        }
        /// <summary>
        /// Ambiguous
        /// </summary>
        /// <param name="root"></param>
        /// <param name="treeHandler"></param>
        /// <param name="waitAll"></param>
        //public static void WalkParallel(Tree<T> root, TreeHandler treeHandler, bool waitAll = false)
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
}