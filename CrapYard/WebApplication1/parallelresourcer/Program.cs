using System;
using System.Threading.Tasks;

namespace ParallelResourcer
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree<string> taken = CreateTasks();
            Tree<string>.TreeHandler treeHandler = OnTreeEvent;
            taken.RegisterWithTree(treeHandler);

            var t1 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Tree<string>.WalkClassic(taken, MyTask);
                }
            });

            Task.WaitAll(t1);

            taken.UnRegisterWithTree(treeHandler);            
            Console.ReadKey();
        }
        private static Tree<string> CreateTasks()
        {
            var taken = new Tree<string>
                                     {
                                         Left = new Tree<string>(),
                                         Right = new Tree<string>(),
                                         Data = "root"
                                     };


            taken.Left.Data = "A";
            taken.Left.Left = new Tree<string> { Data = string.Format("{0}-C", taken.Left.Data) };
            taken.Left.Right = new Tree<string> { Data = string.Format("{0}-D", taken.Left.Data) };

            taken.Right.Data = "B";
            return taken;
        }
        private static void MyTask(string woord)
        {
            Console.WriteLine(woord);
        }
        // This is the target for incoming events.
        public static void OnTreeEvent(string msg)
        {
            Console.WriteLine("\n***** Message From Tree node ***** {0}", msg);
            Console.WriteLine("=> {0}",msg);
            Console.WriteLine("*********************************** {0}\r\n", msg);
        }
    }
}
