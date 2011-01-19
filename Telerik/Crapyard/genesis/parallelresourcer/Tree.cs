﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParallelResourcer
{
    public class Tree<T> : IEnumerable
    {
        public static ConcurrentQueue<T> Queue;
        public Tree<T> Left , Right;
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
        public static void WalkParallel<T>(Tree<T> root, Action<T> action, bool waitAll=false)
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