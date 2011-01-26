using System;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace TelerikExample
{
    public class AsynchOperationPattern : SubjectBase, IAsyncResult 
    {
        private bool _completed;
        private object _state;
        private AsyncCallback _callback;
        private HttpContext _httpContext;
        private Thread myThread;
        private ManualResetEvent _waitHandle = new ManualResetEvent(false);

        public AsynchOperationPattern(AsyncCallback callback, HttpContext httpContext, object state)
        {
            _callback = callback;
            _httpContext = httpContext;
            _state = state;
            _completed = false;
            //_waitHandle = new WaitHandle(new ManualResetEvent(false));
        }

        #region Implementation of IAsyncResult

        public bool IsCompleted
        {
            get { return _completed; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return _waitHandle; }
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
            //ThreadPool.QueueUserWorkItem(StartAsyncOperation,null);
            //ThreadStart myThreadDelegate = StartAsyncOperation;
            myThread = new Thread(StartAsyncOperation);
            myThread.Start();            
        }

        public void StartAsyncOperation()
        {
            try
            {
                HttpResponse Response = _httpContext.Response;

                for (int i = 0; i < 500; i++)
                {
                    Thread.Sleep(10);
                    _state = i.ToString();
                    _callback(this);
                    NotifyObserverLog(new NotifyObserverEventargs(i.ToString()));
                    if (_Stop) break;
                }
                _waitHandle.Set();
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