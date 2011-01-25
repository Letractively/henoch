using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace TelerikExample
{
    public partial class AsyncViewer : System.Web.UI.Page
    {
        private AsynchOperationPattern operationPattern;
        private WebRequest m_MyRequest;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Timer1.Enabled = true;
            Label1.Text = "BeginProcessRequest starting ...";

            AddOnPreRenderCompleteAsync(
                new BeginEventHandler(BeginProcessRequest),
                new EndEventHandler(EndProcessRequest));

            // Initialize the WebRequest.                  
            string address = "http://localhost/";
            ;// Request.Url.ToString();                    
            m_MyRequest = System.Net.WebRequest.Create(address);  
        }
        public bool IsReusable
        {
            get { return false; }
        }
        #region Implementation of IHttpAsyncHandler

        public IAsyncResult BeginProcessRequest(object sender, EventArgs eventArgs, AsyncCallback cb, object extraData)
        {
            Label1.Text = "BeginGetAsyncData: thread #" + Thread.CurrentThread.GetHashCode();
            Trace.Write("BeginGetAsyncData", Label2.Text); 
            //Response.Write("<p>BeginProcessRequest starting ...</p>");
            operationPattern = new AsynchOperationPattern(CallBackResult, this.Context, extraData);
           
            operationPattern.StartAsync();
            
            Session["label3"] = Label3;
            //operationPattern.StartAsync();
            Label2.Text = "BeginProcessRequest queued ...";
            //Response.Write("<p>BeginProcessRequest queued ...</p>");
            return m_MyRequest.BeginGetResponse(cb, Session["label3"]); 
        }

        private void DoAsync(AsyncCallback cb, object extraData)
        {

        }

        public void EndProcessRequest(IAsyncResult result)
        {
            
            int threadId = Thread.CurrentThread.GetHashCode();              
            //int threadId = Thread.CurrentThread.ManagedThreadId;              
            Label2.Text = "EndGetAsyncData: thread #" + threadId;              
            Trace.Write("EndGetAsyncData", Label3.Text);                
            //System.Net.WebResponse myResponse = myRequest.EndGetResponse(ar);               

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
                if (operationPattern != null)
                {
                    Session["label3"] = operationPattern.AsyncState;
                    Label3.Text = Session["label3"] as string;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (operationPattern!=null && operationPattern.IsCompleted)
                Timer1.Enabled = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            Timer1.Enabled = true;
            AsyncCallback cb = new AsyncCallback(CallBackResult);
            object extraData = Session["label3"];
            DoAsync(cb, extraData);
           
            
        }
        private void CallBackResult(IAsyncResult result)
        {
            int threadId;
            //Queue<int> threadIds = m_TaskIds;// Session["TaskIds"] as Queue<int>;

            //If ac is a delegate: AsynchOperationPattern ac = (AsynchOperationPattern)((AsyncResult)result).AsyncDelegate;
            var res = result;
            Session["label3"] = operationPattern.AsyncState;
            Label3.Text = operationPattern.AsyncState as string;
            //result.AsyncWaitHandle.Close();     
            if (operationPattern.IsCompleted)
            {
                Timer1.Enabled = false;
                if (operationPattern.MyThread.IsAlive)
                {
                    operationPattern.MyThread.Abort();
                }
            }
        }
    }
}