using System;
using System.Net;
using System.Threading;
using System.Web;

namespace Observlet.WebForms
{
    public partial class AsyncViewer : PageBase
    {        
        private WebRequest m_MyRequest;

        protected void Page_Load(object sender, EventArgs e)
        {
            Label3.Text = String.Empty;
            if (Halted)
            {
                Label1.Text = String.Empty;
                Label2.Text = String.Empty;
                Label3.Text = "Halted";
                Halted = false;
                Completed = false;
                Button2.Enabled = true;
            }
            if (Completed)
            {
                Completed = false;
                Halted = false;
                Button1.Enabled = true;
                Button2.Enabled = true;
                Label1.Text = String.Empty;
                Label2.Text = String.Empty;
            }
        }


        /// <summary>
        /// Do some work like caching.
        /// </summary>        
        public override void ExecuteCachePolicy()
        {
            for (int i = 0; i < 250; i++)
            {
                Thread.Sleep(10);
                Session["label3"] = "Cache updated " + i.ToString();
                if (Halted)
                {
                    Session["label3"] = "Halted!";
                    break;
                }
            }
            if (!Halted) Session["label3"] = "Cache ready.";
            Completed = true;
            Button1.Enabled = true;
            AsyncOperation = null;
            if (_Observer != null) _Observer.Dispose();
            //pContext.Response.Redirect(pContext.Request.UrlReferrer.ToString());
        }
        #region Implementation of IHttpAsyncHandler

        public IAsyncResult BeginProcessRequest(object sender, EventArgs eventArgs, AsyncCallback cb, object extraData)
        {
            Session["AsyncIsCompleted"] = null;
            Label1.Text = "BeginGetAsyncData: thread #" + Thread.CurrentThread.ManagedThreadId;
            Trace.Write("BeginGetAsyncData", Label2.Text); 
            //Response.Write("<p>BeginProcessRequest starting ...</p>");
            
            var async = new AsyncOperationPattern(CallBackResult, this.Context, extraData);
            AsyncOperation = async;
            _Observer = new Observer<AsyncOperationPattern, AsyncViewer>((AsyncOperationPattern) AsyncOperation, this);
            async.Start(ExecuteCachePolicy);

            Label2.Text = "BeginProcessRequest queued ...";
            //Response.Write("<p>BeginProcessRequest queued ...</p>");
            return m_MyRequest.BeginGetResponse(cb, Session["label3"]); 
        }


        public void EndProcessRequest(IAsyncResult result)
        {                        
            int threadId = Thread.CurrentThread.ManagedThreadId;
            Label2.Text = "Busy...";// +threadId;              
                       
            //System.Net.WebResponse myResponse = myRequest.EndGetResponse(ar);               
            //result.AsyncWaitHandle.WaitOne();
        }

        #endregion


        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Label3.Text = Session["label3"] as string;

            if (AsyncOperation != null && AsyncOperation.IsCompleted)
            {
                /* The work is done by the workerthread within an async function, 
                 * not an anonymous methoud or action passed by the caller.
                 * Assume AsyncOperation.Start().
                 */
                Button1.Enabled = true;
                Timer1.Enabled = false;
                //Let the GC know the async operation could be collected for garbage.
                AsyncOperation = null;
            }
            else
            {
                if (AsyncOperation == null)
                {
                    /* The work is an action started in a new w3w-process within the async operation:
                     * Assume AsyncOperation.Start(anAction).
                     */
                    Button1.Enabled = true;
                    Button2.Enabled = false;
                    Timer1.Enabled = false;
                }
                else
                {
                    Button2.Enabled = true;
                }
            }
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            Button1.Enabled = false;

            if (IsPostBack && IsAsync)
            {
                Label1.Text = "BeginProcessRequest starting ...";
                //timer1 can be removed.
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
                AsyncOperation = null;
                Button1.Enabled = true;
                Label3.Text = Session["label3"] as string;
                Halted = true;
                //if (Request.UrlReferrer != null) Response.Redirect(Request.UrlReferrer.ToString());
                //if (operationPattern != null) operationPattern.Stop();
            }
            catch (Exception ex)
            {
                ;
            }
            Button1.Enabled = true;
        }
    }
}