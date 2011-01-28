using System;
using System.Net;
using System.Threading;
using System.Web;

namespace Observlet.WebForms
{
    public partial class AsyncViewer : System.Web.UI.Page, ISubscriber
    {        
        private AsyncOperationPattern operationPattern;
        private WebRequest m_MyRequest;
        private Observer<AsyncOperationPattern, AsyncViewer> _Observer;
        private bool _Halted;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["halted"]!=null)
            {
                Label3.Text = "Halted";
                Session["halted"] = null;
                Button2.Enabled = true;
            }

        }
        public bool IsReusable
        {
            get { return false; }
        }

        public AsyncOperationPattern AsyncOperation
        {
            get
            {
                operationPattern = Session["AsynchOperationPattern"] as AsyncOperationPattern;
                return operationPattern;
            }
            set
            {
                Session["AsynchOperationPattern"] = value;
                operationPattern = value;
            }
        }

        #region Implementation of IHttpAsyncHandler

        public IAsyncResult BeginProcessRequest(object sender, EventArgs eventArgs, AsyncCallback cb, object extraData)
        {
            Session["AsyncIsCompleted"] = null;
            Label1.Text = "BeginGetAsyncData: thread #" + Thread.CurrentThread.ManagedThreadId;
            Trace.Write("BeginGetAsyncData", Label2.Text); 
            //Response.Write("<p>BeginProcessRequest starting ...</p>");
            
            AsyncOperation = new AsyncOperationPattern(CallBackResult, this.Context, extraData);
            _Observer = new Observer<AsyncOperationPattern, AsyncViewer>(AsyncOperation, this);
            AsyncOperation.StartAsync(DoMyWork);           

            Session["label3"] = Label3;
            

            Label2.Text = "BeginProcessRequest queued ...";
            //Response.Write("<p>BeginProcessRequest queued ...</p>");
            return m_MyRequest.BeginGetResponse(cb, Session["label3"]); 
        }
        public void DoMyWork()
        {
            for (int i = 0; i < 250; i++)
            {
                Thread.Sleep(10);
                Session["label3"] = i.ToString();
            }
            Session["label3"] = "Worker ready.";
            Button1.Enabled = true;
            NotifyHalt(new NotifyObserverEventargs("stop"));
            AsyncOperation = null;
            if (_Observer != null) _Observer.Dispose();
            //redirect            
        }
        public void EndProcessRequest(IAsyncResult result)
        {                        
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Label2.Text = "EndGetAsyncData: thread #" + threadId;              
            Trace.Write("EndGetAsyncData", Label3.Text);                
            //System.Net.WebResponse myResponse = myRequest.EndGetResponse(ar);               
            result.AsyncWaitHandle.WaitOne();
        }

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            throw new InvalidOperationException();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Label3.Text = Session["label3"] as string;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (AsyncOperation!=null && AsyncOperation.IsCompleted)
            {                
                Button1.Enabled = true;
                Timer1.Enabled = false;
                AsyncOperation = null;
            }
            else
                if (AsyncOperation==null)
                {
                    Button1.Enabled = true;
                    Timer1.Enabled = false;
                    if (_Observer != null) _Observer.Dispose();
                }
        }

        private void CallBackResult(IAsyncResult result)
        {
            int threadId;
            //Queue<int> threadIds = m_TaskIds;// Session["TaskIds"] as Queue<int>;

            //If ac is a delegate: AsynchOperationPattern ac = (AsynchOperationPattern)((AsyncResult)result).AsyncDelegate;
            var res = result;

            if (AsyncOperation.IsCompleted)
            {
                Button1.Enabled = true;                
                NotifyHalt(new NotifyObserverEventargs("stop"));
                if (_Observer != null) _Observer.Dispose();
            }
            Session["label3"] = AsyncOperation.AsyncState;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Button1.Enabled = false;

            if (IsPostBack && IsAsync)
            {
                Label1.Text = "BeginProcessRequest starting ...";
                Timer1.Enabled = true;
                AddOnPreRenderCompleteAsync(
                    new BeginEventHandler(BeginProcessRequest),
                    new EndEventHandler(EndProcessRequest));

                // Initialize the WebRequest.                  
                string address = "http://localhost/";
                ;// Request.Url.ToString();                    
                m_MyRequest = System.Net.WebRequest.Create(address);
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                _Halted = true;
                AsyncOperation = null;
                Button1.Enabled = true;
                Label3.Text = Session["label3"] as string;
                Session["halted"] = "yes";
                if (Request.UrlReferrer != null) Response.Redirect(Request.UrlReferrer.ToString());
                //if (operationPattern != null) operationPattern.Stop();
            }
            catch (Exception ex)
            {
                ;
            }
            Timer1.Enabled = false;
            Button1.Enabled = true;
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