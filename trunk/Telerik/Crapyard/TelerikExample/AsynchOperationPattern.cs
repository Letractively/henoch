using System;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace TelerikExample
{
    public class AsynchOperationPattern : IAsyncResult
    {
        private bool _completed;
        private object _state;
        private AsyncCallback _callback;
        private HttpContext _httpContext;
        private Thread myThread;

        public AsynchOperationPattern(AsyncCallback callback, HttpContext httpContext, object state)
        {
            _callback = callback;
            _httpContext = httpContext;
            _state = state;
            _completed = false;
        }

        #region Implementation of IAsyncResult

        public bool IsCompleted
        {
            get { return _completed; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return null; }
        }

        public object AsyncState
        {
            get { return _state; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public Thread MyThread
        {
            get { return myThread; }
        }

        #endregion

        public void StartAsync()
        {
            ThreadStart myThreadDelegate = StartAsyncOperation;
            myThread = new Thread(myThreadDelegate);
            MyThread.Start();

            //ThreadPool.QueueUserWorkItem(StartAsyncOperation, null);
        }

        public void StartAsyncOperation()
        {
            try
            {
                HttpResponse Response = _httpContext.Response;

                Label label = _httpContext.Session["label3"] as Label;
                if (label != null)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Thread.Sleep(500);
                        _state = i.ToString();
                    }
                }
                else
                {
                    Response.Write("<p>Asynch operation completed.</p>");
                }
                //_httpContext.Session["label3"] 
                _state = "Asynch operation completed";
                _completed = true;
                _callback(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}