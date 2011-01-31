﻿using System;
using System.Net;
using System.Threading;
using System.Web;

namespace Observlet.WebForms
{
    /// <summary>
    /// Supports asynchronuous handling of request in an own process with notifications
    /// based on the observerpattern.
    /// </summary>
    public abstract class AsyncPageBase : System.Web.UI.Page, IHttpAsyncHandler, ISubscriber
    {
        protected WebRequest _MyRequest;
        /// <summary>
        /// Observs the
        /// </summary>
        protected Observer<AsyncOperationPattern, AsyncPageBase> _Observer;

        private IAsyncResult _AsyncOperator;

        /// <summary>
        /// Caches the asyncstate into session. TODO: use cache.
        /// </summary>
        protected Object AsyncState
        {
            get
            {
                return Session["AsyncState"];
            }
            set
            {
                Session["AsyncState"] = value;
            }
        }
        protected bool Halted
        {
            get
            {
                bool res = (bool)(Session["halted"] ?? false);
                return res;
            }
            set
            {

                Session["halted"] = value;
            }
        }

        protected bool Completed
        {
            get
            {
                bool res = (bool)(Session["Completed"] ?? false);
                return res;
            }
            set
            {
                Session["Completed"] = value;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            throw new InvalidOperationException();
        }
        protected virtual void CallBackResult(IAsyncResult result)
        {
            int threadId;
            //Queue<int> threadIds = m_TaskIds;// Session["TaskIds"] as Queue<int>;

            //If ac is a delegate: AsynchOperationPattern ac = (AsynchOperationPattern)((AsyncResult)result).AsyncDelegate;
            var res = result;

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
            //Response.Write("<p>BeginProcessRequest starting ...</p>");

            var async = new AsyncOperationPattern(CallBackResult, this.Context, extraData);
            AsyncOperator = async;
            _Observer = new Observer<AsyncOperationPattern, AsyncPageBase>((AsyncOperationPattern)_AsyncOperator, this);
            async.Start(ExecuteCachePolicy);

            //Response.Write("<p>BeginProcessRequest queued ...</p>");
            return _MyRequest.BeginGetResponse(cb, AsyncState);
        }

        /// <summary>
        /// Only for httphandlers.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cb"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            throw new NotImplementedException();
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Trace.Write("EndProcessRequest", "Threadname = " + Thread.CurrentThread.Name);
            //System.Net.WebResponse myResponse = myRequest.EndGetResponse(ar);               
            //result.AsyncWaitHandle.WaitOne();
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