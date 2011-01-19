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
        private HttpContext _context;

        public AsynchOperationPattern(AsyncCallback callback, HttpContext context, object state)
        {
            _callback = callback;
            _context = context;
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

        #endregion

        public void StartAsync()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncOperation), null);
        }
        public void StartAsyncOperation(object workItemState)
        {            
            HttpResponse Response = _context.Response;

            Label label = _context.Session["label3"] as Label;
            if (label != null)
            {
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(100);
                    _context.Session["label3"] = i.ToString();
                }
                
            }
            else
            {
                Response.Write("<p>Asynch operation completed.</p>");    
            }
            _context.Session["label3"] = "Asynch operation completed";
            _completed = true;
            _callback(this);
        }
    }
}