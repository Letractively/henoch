using System;
using System.ComponentModel;
using System.Threading;
using System.Web;
using DataResource;

namespace AsyncHandlers
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncRequestPattern : SubjectBase, IAsyncResult 
    {
        private bool _completed;
        private object _state;
        private AsyncCallback _callback;
        private HttpContext _httpContext;
        private Thread myThread;
        private ManualResetEvent _callCompleteEvent = null;
        static BackgroundWorker _bw = new BackgroundWorker();

        private AsyncRequestPattern()
        {
            //Default constructor is disabled to force parameterized instanciation.
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback">Put here the callback that will be called.</param>
        /// <param name="httpContext">Put here the Context of the current page.</param>
        /// <param name="state">Put here the instance to be monitored.</param>
        public AsyncRequestPattern(AsyncCallback callback, HttpContext httpContext, object state)
        {
            _callback = callback;
            _httpContext = httpContext;
            _state = state;
            _completed = false;

        }
        private void CompleteRequest()
        {
            if (_Stop)
                _state = "Halted";
            else
                _state = "Asynch operation completed";

            _completed = true;
            lock (this)
            {
                if (_callCompleteEvent != null)
                    _callCompleteEvent.Set();
            }
            // if a callback was registered, invoke it now
            if (_callback != null)
                _callback(this);
        }

        #region Implementation of IAsyncResult

        public bool IsCompleted
        {
            get { return _completed; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                lock (this)
                {
                    if (_callCompleteEvent == null)
                        _callCompleteEvent = new ManualResetEvent(false);

                    return _callCompleteEvent;
                }
            }
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

        public void Start(string file)
        {
            //ThreadPool.QueueUserWorkItem(StartAsyncOperation,null);
            //ThreadStart myThreadDelegate = StartAsyncOperation;
            myThread = new Thread(StartAsyncOperation);
            myThread.Start(file);
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
        /// Only for testing. This generates a simple task: get rowcount intended to be excecuted in a 32-bit pool.
        /// </summary>
        private void StartAsyncOperation(object datasource)
        {
            try
            {
                //var result = new MyAccess().DoOleDbAction(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + datasource.ToString() + ";");
                HttpResponse Response = _httpContext.Response;

                for (int i = 0; i < 250; i++)
                {
                    Thread.Sleep(100);
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
                CompleteRequest();
            }
        }

    }
}