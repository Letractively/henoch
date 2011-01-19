using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.SessionState;

///http://msdn.microsoft.com/en-us/library/2e08f6yc.aspx
namespace ApplicationTypes.DesignPatterns
{

    // The delegate must have the same signature as the method
    // it will call asynchronously.
    public delegate string AsyncMethodCaller(int callDuration, out int threadId);
    public delegate string[] AsyncCreateQueue(out int threadId, int maxValue, int sleep);

    public class AsyncDemo : LoggingBase
    {
        private Thread m_ThreadId;

        public AsyncDemo(HttpSessionState session, TraceContext trace)
        {
            m_Session = session;
            m_Trace = trace;
        }

        public AsyncDemo()
        {
        }

        // The method to be executed asynchronously.
        public string TestMethod(int callDuration, out int threadId)
        {

            //Console.WriteLine("Test method begins.");
            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;

            return String.Format("My call time was {0}.", callDuration.ToString());

        }
        // The method to be executed asynchronously.
        /// <summary>
        /// MaxValue = count loops
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="maxValue"></param>
        /// <param name="sleep"></param>
        /// <returns></returns>
        public string[] CreateQueue(out int threadId, int maxValue, int sleep)
        {
            TraceContext trace = m_Trace;

            threadId = Thread.CurrentThread.ManagedThreadId;
            m_ThreadId = Thread.CurrentThread;
             if (trace != null) trace.Write("CreateQueue",
                string.Format("ThreadId={0}", threadId));
            string[] list = CreateDummyQueue(maxValue, sleep);
            return list;

        }
        private string[] CreateDummyQueue(int maxValue, int sleep)
        {

            List<string> messages = new List<string>();
            try
            {
                HttpSessionState session = m_Session;
                TraceContext trace = m_Trace;

                for (int i = 0; i < maxValue; i++)
                {

                    double result = (DateTime.Now.Ticks);

                    string message = DateTime.Now.ToShortTimeString() + ":" +
                        string.Format(CultureInfo.InvariantCulture, "ThreadName = {0}\tManagedThreadId = {1}\tScheduled on = {2}\n",
                             m_ThreadId.Name, m_ThreadId.ManagedThreadId,
                            Convert.ToDouble(result).ToString("F1", CultureInfo.InvariantCulture));

                    NotifyObserver(session, message);
                    messages.Add(message);
                    if (trace != null) trace.Write("CreateDummyQueue",
                            string.Format("Producing queue ={0}", message));

                    var rnd1 = new Random();
                    ///Thread.Sleep(rnd1.Next(sleep));
                    //Thread.Sleep(0);
                }
                Thread.Sleep(1000);
                NotifyObserver(session, "Producer finished!\n");//NotifyObserver("Producer finised!");
            }
            catch (ThreadInterruptedException)
            {
                NotifyObserver("interrupted..");
                Console.WriteLine("~~~~ thread2 interrupted...");
            }
            catch (Exception ex)
            {
                //NotifyObserver(ex.Message);
                //NotifyObserver(null, ex.Message);
                Console.WriteLine("~~~~ thread2 error...");
            }
            finally
            {
                Console.WriteLine("~~~~ thread2 initData2 ends ...~~~~");
                

            }
            return messages.ToArray();
        }


    }
    /// <summary>
    /// 
    /// </summary>
    public class AsyncMain
    {
        /// <summary>
        /// Waiting for an Asynchronous Call with EndInvoke.
        /// </summary>
        public static void Main()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo(HttpContext.Current.Session, HttpContext.Current.Trace);

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);

            Thread.Sleep(0);
            Console.WriteLine("Main thread {0} does some work.",
                Thread.CurrentThread.ManagedThreadId);

            // Call EndInvoke to wait for the asynchronous call to complete,
            // and to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }

        static void DoWaitHandleVariant()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo(HttpContext.Current.Session, HttpContext.Current.Trace);

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);

            Thread.Sleep(0);
            Console.WriteLine("Main thread {0} does some work.",
                Thread.CurrentThread.ManagedThreadId);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            // Perform additional processing here.
            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }

        static void DoPollingVariant()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo(HttpContext.Current.Session, HttpContext.Current.Trace);

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);

            // Poll while simulating work.
            while (result.IsCompleted == false)
            {
                Thread.Sleep(250);
                Console.Write(".");
            }

            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            Console.WriteLine("\nThe call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);
        }

        static void DoCallbackVariant()
        {
            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo(HttpContext.Current.Session, HttpContext.Current.Trace);

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            // The threadId parameter of TestMethod is an out parameter, so
            // its input value is never used by TestMethod. Therefore, a dummy
            // variable can be passed to the BeginInvoke call. If the threadId
            // parameter were a ref parameter, it would have to be a class-
            // level field so that it could be passed to both BeginInvoke and 
            // EndInvoke.
            int dummy = 0;

            // Initiate the asynchronous call, passing three seconds (3000 ms)
            // for the callDuration parameter of TestMethod; a dummy variable 
            // for the out parameter (threadId); the callback delegate; and
            // state information that can be retrieved by the callback method.
            // In this case, the state information is a string that can be used
            // to format a console message.
            IAsyncResult result = caller.BeginInvoke(3000,
                out dummy,
                new AsyncCallback(CallbackMethod),
                "The call executed on thread {0}, with return value \"{1}\".");

            Console.WriteLine("The main thread {0} continues to execute...",
                Thread.CurrentThread.ManagedThreadId);

            // The callback is made on a ThreadPool thread. ThreadPool threads
            // are background threads, which do not keep the application running
            // if the main thread ends. Comment out the next line to demonstrate
            // this.
            Thread.Sleep(4000);

            Console.WriteLine("The main thread ends.");
        }

        // The callback method must have the same signature as the
        // AsyncCallback delegate.

        static void CallbackMethod(IAsyncResult ar)
        {
            // Retrieve the delegate.
            AsyncResult result = (AsyncResult)ar;
            AsyncMethodCaller caller = (AsyncMethodCaller)result.AsyncDelegate;

            // Retrieve the format string that was passed as state 
            // information.
            string formatString = (string)ar.AsyncState;

            // Define a variable to receive the value of the out parameter.
            // If the parameter were ref rather than out then it would have to
            // be a class-level field so it could also be passed to BeginInvoke.
            int threadId = 0;

            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, ar);

            // Use the format string to format the output message.
            Console.WriteLine(formatString, threadId, returnValue);
        }

    }


}