using System;
using System.Threading;
using System.Web;

namespace Observlet.WebForms
{
    public class AsyncOperationPattern : SubjectBase, IAsyncResult 
    {
        private bool _completed;
        private object _state;
        private AsyncCallback _callback;
        private HttpContext _httpContext;
        private Thread myThread;
        private ManualResetEvent _waitHandle = new ManualResetEvent(false);

        public AsyncOperationPattern(AsyncCallback callback, HttpContext httpContext, object state)
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

        public void Start()
        {
            //ThreadPool.QueueUserWorkItem(StartAsyncOperation,null);
            //ThreadStart myThreadDelegate = StartAsyncOperation;
            myThread = new Thread(StartAsyncOperation);
            myThread.Start();
            IsBusy = true;
        }
        public void Start(Action action)            
        {
            //ThreadPool.QueueUserWorkItem(StartAsyncOperation,null);
            //ThreadStart myThreadDelegate = StartAsyncOperation;
            if (action != null) myThread = new Thread(action.Invoke);
            myThread.Start();
            IsBusy = true;            
        }

        public bool IsBusy { private set; get; }

        /// <summary>
        /// Only for testing. This generates a simple task: sleep 10 ms.
        /// </summary>
        public void StartAsyncOperation()
        {
            try
            {
                HttpResponse Response = _httpContext.Response;

                for (int i = 0; i < 250; i++)
                {
                    Thread.Sleep(10);
                    _state = i.ToString();
                    _callback(this);                    
                    NotifyObserverLog(new NotifyObserverEventargs(i.ToString()));
                    if (_Stop) break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                //_waitHandle.Set();
                if (_Stop)
                    _state = "Halted";
                else
                    _state = "Asynch operation completed";
                _completed = true;
                _callback(this);
            }
        }

    }
}