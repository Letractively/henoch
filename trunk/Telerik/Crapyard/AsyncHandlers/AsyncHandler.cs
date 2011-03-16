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
    public class AsyncHandler : HttpApplication, IHttpAsyncHandler, ISubscriber
    {
        #region Delegates

        public delegate void ProcessRequestDelegate(HttpContext ctx);

        #endregion

        private static int BUFFER_SIZE = 1024;
        private readonly string FOR_HTTPHANDLERS_ONLY = "Only for httphandlers.";
        private IAsyncResult _AsyncOperator;
        //protected WebRequest _MyRequest;
        public WebRequest WebRequest { get; set; }
        private HttpContext _context;
        /// <summary>
        /// Observs the
        /// </summary>
        protected Observer<AsyncRequestPattern, AsyncHandler> _Observer;

        public AsyncHandler(HttpContext context)
        {
            _context = context;
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

            var asyncPattern = result as AsyncRequestPattern;
            if (asyncPattern != null) Log(asyncPattern.AsyncState.ToString());
            if (_AsyncOperator.IsCompleted)
            {
                NotifyHalt(new NotifyObserverEventargs("stop"));
                if (_Observer != null) _Observer.Dispose();
            }

        }

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
            //Trace.Write("EndProcessRequest", "Threadname = " + Thread.CurrentThread.Name);
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
            _context.Session["AsyncIsCompleted"] = null;
            Thread.CurrentThread.Name = new Guid().ToString();
            _context.Trace.Write("BeginGetAsyncData", "Threadname = " + Thread.CurrentThread.Name);

            var async = new AsyncRequestPattern(CallBackResult, _context, extraData);
            AsyncOperator = async;
            _Observer = new Observer<AsyncRequestPattern, AsyncHandler>((AsyncRequestPattern) _AsyncOperator, this);

            //string mdfFile = MapPath(@"\bin\") + "Nwind.mdb";
            //async.StartAsyncOperation(mdfFile);
            async.Start(String.Empty);

            // Fire-and-forget
            return WebRequest.BeginGetResponse(cb, extraData);
        }

        #endregion

        public void ExecuteAsyncFun()   
        {
            var async = AsyncOperator as AsyncRequestPattern;
            //string mdfFile = MapPath(@"\bin\") + "Nwind.mdb";
            if (async != null) async.Start(String.Empty);
        }
        #region Implementation of ISubscriber

        public IAsyncResult AsyncOperator
        {
            get
            {
                _AsyncOperator = _context.Session["AsynchOperationPattern"] as IAsyncResult;
                return _AsyncOperator;
            }
            set
            {
                _context.Session["AsynchOperationPattern"] = value;
                _AsyncOperator = value;
            }
        }

        public event EventHandler<NotifyObserverEventargs> NotifyHaltHandler;
        public event EventHandler<NotifyObserverEventargs> NotifyLogger;

        public void NotifyHalt(NotifyObserverEventargs args)
        {
            NotifyHaltHandler.Invoke(this, args);
        }

        public void Log(string message)
        {
            NotifyLogger.Invoke(this,new NotifyObserverEventargs(message));
        }

        #endregion
    }
}