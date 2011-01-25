using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            Timer1.Enabled = false;
            Label1.Text = "BeginProcessRequest starting ...";

            AddOnPreRenderCompleteAsync(
                new BeginEventHandler(BeginProcessRequest),
                new EndEventHandler(EndProcessRequest));

            // Initialize the WebRequest.                  
            string address = Request.Url.ToString();                    
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
            operationPattern = new AsynchOperationPattern(cb, this.Context, extraData);
            Session["label3"] = Label3;
            operationPattern.StartAsync();
            Label2.Text = "BeginProcessRequest queued ...";
            //Response.Write("<p>BeginProcessRequest queued ...</p>");
            return m_MyRequest.BeginGetResponse(cb, extraData);  
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
            Label3.Text = Session["label3"] as String;

            if (operationPattern!=null && operationPattern.IsCompleted)
                Timer1.Enabled = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            


            
        }
    }
}