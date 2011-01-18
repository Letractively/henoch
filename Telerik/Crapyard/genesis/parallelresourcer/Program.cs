using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelResourcer
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree<string> taken = CreateTasks();
            var t1 = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                   
                    Tree<string>.WalkParallel(taken,
                                            MyTask,true);
                }
            });

            Task.WaitAll(t1);
        }
        private static Tree<string> CreateTasks()
        {
            Tree<string> taken = new Tree<string>();
            taken.Left = new Tree<string>();
            taken.Right = new Tree<string>();
            taken.Data = "root";
            taken.Left.Data = "A";
            taken.Right.Data = "B";

            taken.Left.Left = new Tree<string>();
            taken.Left.Left.Data = "C-" + taken.Left.Data;
            taken.Left.Right = new Tree<string>();
            taken.Left.Right.Data = "D-" + taken.Left.Data;
            return taken;
        }
        private static void MyTask(string woord)
        {
            Console.WriteLine(woord);
        }
    }
}
