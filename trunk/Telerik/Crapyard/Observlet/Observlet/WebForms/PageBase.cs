using System;
using System.Web;

namespace Observlet.WebForms
{
    public class PageBase : System.Web.UI.Page, ISubscriber
    {
        protected IAsyncResult operationPattern;
        protected Observer<AsyncOperationPattern, AsyncViewer> _Observer;

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

        protected IAsyncResult AsyncOperation
        {
            get
            {
                operationPattern = Session["AsynchOperationPattern"] as IAsyncResult;
                return operationPattern;
            }
            set
            {
                Session["AsynchOperationPattern"] = value;
                operationPattern = value;
            }
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

            if (AsyncOperation.IsCompleted)
            {
                NotifyHalt(new NotifyObserverEventargs("stop"));
                if (_Observer != null) _Observer.Dispose();
            }
            Session["label3"] = AsyncOperation.AsyncState;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void ExecuteCachePolicy()
        {
            //TODO: promote to interface or override in derived classes which implement different policies.
            throw new NotImplementedException("You must override");
        }
        #region Implementation of ISubscriber

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