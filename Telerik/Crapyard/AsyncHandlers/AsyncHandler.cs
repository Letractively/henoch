using System;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace AsyncHandlers
{
    /// <summary>
    /// Supports asynchronous handling of requests in an own process with notifications
    /// based on the observerpattern.
    /// </summary>
    public abstract class AsyncHandler : Page, IHttpAsyncHandler, ISubscriber
    {
        #region Delegates

        public delegate void ProcessRequestDelegate(HttpContext ctx);

        #endregion

        private static int BUFFER_SIZE = 1024;
        private readonly string FOR_HTTPHANDLERS_ONLY = "Only for httphandlers.";
        private IAsyncResult _AsyncOperator;
        protected WebRequest _MyRequest;

        /// <summary>
        /// Observs the
        /// </summary>
        protected Observer<AsyncRequestPattern, AsyncHandler> _Observer;

        /// <summary>
        /// Caches the asyncstate into session. TODO: use cache.
        /// </summary>
        protected Object AsyncState
        {
            get { return Session["AsyncState"]; }
            set { Session["AsyncState"] = value; }
        }

        protected bool Halted
        {
            get
            {
                var res = (bool) (Session["halted"] ?? false);
                return res;
            }
            set { Session["halted"] = value; }
        }

        protected bool Completed
        {
            get
            {
                var res = (bool) (Session["Completed"] ?? false);
                return res;
            }
            set { Session["Completed"] = value; }
        }

        #region IHttpAsyncHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
        }

        #endregion

        protected virtual void CallBackResult(IAsyncResult result)
        {
            int threadId;
            //Queue<int> threadIds = m_TaskIds;// Session["TaskIds"] as Queue<int>;

            //If ac is a delegate: AsynchOperationPattern ac = (AsynchOperationPattern)((AsyncResult)result).AsyncDelegate;
            IAsyncResult res = result;

            if (_AsyncOperator.IsCompleted)
            {
                NotifyHalt(new NotifyObserverEventargs("stop"));
                if (_Observer != null) _Observer.Dispose();
            }
            AsyncState = _AsyncOperator.AsyncState;
        }

        /// <summary>
        /// Derived classes must implement their own caching policy concerning different levels of
        /// staleness.
        /// </summary>  
        public abstract void ExecuteCachePolicy();

        #region Implementation of IHttpAsyncHandler

        /// <summary>
        /// Only for httphandlers.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cb"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            throw new InvalidOperationException(FOR_HTTPHANDLERS_ONLY);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Trace.Write("EndProcessRequest", "Threadname = " + Thread.CurrentThread.Name);
            //System.Net.WebResponse myResponse = myRequest.EndGetResponse(ar);               
            //result.AsyncWaitHandle.WaitOne();
        }

        /// <summary>
        /// See: http://msdn.microsoft.com/en-us/library/system.web.ihttpasynchandler.aspx , 
        /// http://msdn.microsoft.com/en-us/magazine/cc164128.aspx#S4 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        /// <param name="cb"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        public IAsyncResult BeginProcessRequest(object sender, EventArgs eventArgs, AsyncCallback cb, object extraData)
        {
            Session["AsyncIsCompleted"] = null;
            Thread.CurrentThread.Name = new Guid().ToString();
            Trace.Write("BeginGetAsyncData", "Threadname = " + Thread.CurrentThread.Name);

            var async = new AsyncRequestPattern(CallBackResult, Context, extraData);
            AsyncOperator = async;
            _Observer = new Observer<AsyncRequestPattern, AsyncHandler>((AsyncRequestPattern) _AsyncOperator, this);
            async.Start(ExecuteCachePolicy);

            // Fire-and-forget
            return _MyRequest.BeginGetResponse(cb, extraData);
        }

        #endregion

        #region Implementation of ISubscriber

        public IAsyncResult AsyncOperator
        {
            get
            {
                _AsyncOperator = Session["AsynchOperationPattern"] as IAsyncResult;
                return _AsyncOperator;
            }
            set
            {
                Session["AsynchOperationPattern"] = value;
                _AsyncOperator = value;
            }
        }

        public event EventHandler<NotifyObserverEventargs> NotifyHaltHandler;

        public void NotifyHalt(NotifyObserverEventargs args)
        {
            NotifyHaltHandler.Invoke(this, args);
        }

        public void Log(string message)
        {
            ;
        }

        #endregion
    }
}