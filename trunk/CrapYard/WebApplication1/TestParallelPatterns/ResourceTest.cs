using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParallelResourcer;
using System.Threading.Tasks;

namespace TestParallelPatterns
{
    [TestClass]
    public class ResourceTest
    {
        private TestContext testContextInstance;
        private Action<string> MyAction;
        private int loop = 10;
        private Stopwatch stopwatch;
        private long start;
        private Tree<string,string> taken;

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


        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        [TestInitialize]
        public void TestInitialize()
        {
           
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            taken = CreateTasks();

            start = Environment.TickCount;
            stopwatch = new Stopwatch();
            stopwatch.Start();

            MyAction = MyTask2;
            testContextInstance.BeginTimer("stopwatch");
            Tree<string,TaskInfo>.Queue = new ConcurrentQueue<TaskInfo>();
        }
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void TestCleanup()
        {           
            testContextInstance.EndTimer("stopwatch");
            long ended = Environment.TickCount;
            stopwatch.Stop();
            
            long elapsed =  ended - start;
            Console.WriteLine("Treewalk nodes: " + Tree<string, TaskInfo>.Queue.Count);
            Console.WriteLine("\n----- Environment.TickCount------");
            Console.WriteLine("Treewalk duration: " + elapsed);

            TaskInfo task;
            long totalTaskDuration = 0;

            var query = from taak in Tree<string, TaskInfo>.Queue
                        where ended <= taak.TaskEnded
                        select taak;
            var taskBusyAfterTreeWalk = from taak in query
                                        where taak.TaskEnded > ended
                                        select taak;
            Assert.AreEqual(0, taskBusyAfterTreeWalk.Count(), 
                "A task was still busy while treewalk was ending.");
            int aantalVerdachteTaken = query.Count();

            int len = Tree<string, TaskInfo>.Queue.Count;
            var listQ = Tree<string, TaskInfo>.Queue.ToArray();
            Parallel.For(0, len, (i) =>
            {
                Console.WriteLine(listQ[i].TaskDescription + " ----");
                totalTaskDuration += listQ[i].Taskduration;
            });
            while (Tree<string, TaskInfo>.Queue.TryDequeue(out task))
            {
                Console.WriteLine(task.TaskDescription);
                totalTaskDuration += task.Taskduration;
            }
            Console.WriteLine("Treewalk ended at: " + ended);
            Console.WriteLine("Count task ended at same\ntime as treewalk: " + aantalVerdachteTaken);
            Console.WriteLine("Total task weight : " + totalTaskDuration);
            Console.WriteLine("-----------------------------\n");
            DisplayTimerProperties();
            Console.WriteLine("Stopwatch highresolution(sw): " + Stopwatch.IsHighResolution);
            Console.WriteLine("Stopwatch frequency: " + Stopwatch.Frequency);
            Console.WriteLine("Treewalk duration(sw): " + stopwatch.ElapsedTicks);
            Console.WriteLine("Treewalk duration(sw): " + stopwatch.ElapsedMilliseconds);
            
        }
        
        #endregion


        private void MyTask(string woord)
        {
            Console.WriteLine(woord);            
        }

        private void MyTask2(string woord)
        {
            var taskInfo = new TaskInfo
                               {
                                   TaskStarted = Environment.TickCount,
                                   TaskStarted2 = Stopwatch.GetTimestamp()
                               };
            Thread.Sleep(10);// Console.WriteLine(woord)

            taskInfo.TaskEnded = Environment.TickCount;
            taskInfo.TaskEnded2 = Stopwatch.GetTimestamp();
            var elapsed = taskInfo.TaskEnded - taskInfo.TaskStarted;

            //Tree<string,string>.Queue.Enqueue(woord + elapsed);

            taskInfo.TaskDescription = woord + " ;elapsed " + elapsed;
            Tree<string, TaskInfo>.Queue.Enqueue(taskInfo);
        }
        [TestMethod]
        public void TestWalkParallel()
        { 
            var t1 = System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < loop; i++)
                    {
                        Tree<string,string>.WalkParallel(taken, MyAction);
                    }
                }
            );
            
            System.Threading.Tasks.Task.WaitAll(t1);

        }
        [TestMethod]
        public void TestWalkParallelNTree()
        {
            var NTree = CreateTasksForNTree();
                        
            //Tree<string,string>.WalkNaryTree(NTree, Console.WriteLine);   
            var t1 = Task.Factory.StartNew(() =>
            {
                Tree<string,string>.WalkParallelNTree(NTree, MyTask2, true);
            }
            );
           
            Task.WaitAll(t1);

            Assert.AreEqual(9, Tree<string, TaskInfo>.Queue.Count);
        }
        [TestMethod]
        public void TestWalkNTree()
        {
            var NTree = CreateTasksForNTree();

            //Tree<string,string>.WalkNaryTree(NTree, Console.WriteLine);   
            var t1 = Task.Factory.StartNew(() =>
            {
                Tree<string,string>.WalkNaryTree(NTree, MyTask2);
            }
            );

            //Assert.AreEqual(8, Tree<string, TaskInfo>.Queue.Count);
            Task.WaitAll(t1);
        }
        [TestMethod]
        public void TestWalkParallelWaitAll()
        {
            for (int i = 0; i < loop; i++)
            {
                Tree<string,string>.WalkParallel(taken, MyAction, true);
            }
          
        }
        [TestMethod]
        public void TestWalkClassic()
        {
            for (int i = 0; i < loop; i++)
            {
                Tree<string,string>.WalkClassic(taken, MyAction);
            }
        }
        [TestMethod]
        public void TestWalkClassicUsingDelegates()
        {
            Tree<string,string>.TreeHandler treeHandler = MyTask2;
            taken.RegisterWithTree(treeHandler);

            for (int i = 0; i < loop; i++)
            {
                Tree<string,string>.WalkClassic(taken);
            }
            taken.UnRegisterWithTree(treeHandler);  
        }

        private static Tree<string,string> CreateTasks()
        {
            var taken = new Tree<string,string>
                                     {
                                         Left = new Tree<string,string>(),
                                         Right = new Tree<string,string>(),
                                         Data = "root"
                                     };
            taken.Left.Data = "A";
            taken.Left.Left = new Tree<string,string> { Data = string.Format("{0}-C", taken.Left.Data) };
            taken.Left.Right = new Tree<string,string> { Data = string.Format("{0}-D", taken.Left.Data) };
 
            taken.Right.Data = "B";
            return taken;
        }
        /// <summary>
        /// Create tree (non-binairy)
        /// </summary>
        /// <returns></returns>
        private static Tree<string,string> CreateTasksForNTree()
        {
            var taken = new Tree<string,string>
            {
                Data = "root",
                NTree = new Tree<string,string>[]
                {
                    new Tree<string,string>{ Data = "S11"},
                    new Tree<string,string>
                    { 
                        Data = "S211",
                        NTree = new Tree<string,string>[]
                        {
                          new Tree<string,string>{Data = "S22"}
                        }
                    },
                    new Tree<string,string>
                    { 
                        Data = "S221",
                        NTree = new Tree<string,string>[]
                        {
                            new Tree<string,string>{ Data = "S21"},
                            new Tree<string,string>{ Data = "m1"},
                            new Tree<string,string>{ Data = "Sp"},
                        }
                    },
                    new Tree<string,string>{ Data = "S31"}
                }
            };

            return taken;
        }
    }

}
