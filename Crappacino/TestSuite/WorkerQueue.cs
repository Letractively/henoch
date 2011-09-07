using System;
using System.Collections.Generic;
using System.Threading;

namespace UTS.AMS.TestSuite
{
    public class WorkerQueue
    {
        readonly object _locker = new object();
        Thread[] _workers;
        Queue<Action> _itemQ = new Queue<Action>();
        private int _workerCount;
        private static bool _isDequeing;

        public WorkerQueue(int workerCount)
        {
            _workers = new Thread[workerCount];

            // Create and start a separate thread for each worker
            for (int i = 0; i < workerCount; i++)
            {
                _workers[i] = new Thread(DoWork);
                _workers[i].Name = "WorkerThread" + i.ToString();
            }
            _workerCount = workerCount;
        }
        /// <summary>
        /// Start workers and wait.
        /// </summary>
        /// <param name="waitForWorkers"></param>
        public void StartWorkers(bool waitForWorkers)
        {
            for (int i = 0; i < _workerCount; i++)
            {
                if (i > 0)//kickstart with thread 1;
                    _isDequeing = true;

                _workers[i].Start();
            }
            // Enqueue one null item per worker to make each exit.
            EnqueueItem(null);

            // Wait for workers to finish
            if (waitForWorkers)
                foreach (Thread worker in _workers)
                {
                    string msg = string.Format("Busy for ", worker.Name);
                    //EventLog.WriteEntry("StressTestWorker", msg, EventLogEntryType.Information);
                    Console.WriteLine(msg);
                    worker.Join();
                }
        }

        public void EnqueueItem(Action item)
        {
            lock (_locker)
            {
                _itemQ.Enqueue(item);           // We must pulse because we're
                Monitor.PulseAll(_locker);         // changing a blocking condition.
            }
        }

        void DoWork()
        {
            int jobCount = 10;
            for (int i = 0; i < jobCount; i++)
            {

                //do jobs
                Action theJob;
                lock (_locker)
                {
                    //if (Thread.CurrentThread.Name.Equals("WorkerThread0"))
                    //{
                    //    _isDequeing = false;
                    //}
                    //while (_isDequeing)
                    //{
                    //    Monitor.Wait(_locker, TimeSpan.FromMilliseconds(10));
                    //}

                    theJob = _itemQ.Dequeue();

                    _isDequeing = false;
                    Monitor.Pulse(_locker);         // changing a blocking condition (unblock).
                    _isDequeing = true;
                    Monitor.Pulse(_locker);         // changing a blocking condition (block).
                }
                if (theJob == null)
                    // This signals our exit, because there are no jobs left.
                    return;
                theJob();
            }
        }
    }
}