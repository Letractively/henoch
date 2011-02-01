using System;
using System.Net;
using System.Threading;
using System.Web;

namespace Observlet.WebForms
{
    public partial class AsyncViewer : AsyncHandler
    {               
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



        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Label3.Text = AsyncState as string;

            if (AsyncOperator != null && AsyncOperator.IsCompleted)
            {
                /* The work is done by the workerthread within an async function, 
                 * not an anonymous methoud or action passed by the caller.
                 * Assume AsyncOperation.Start().
                 */
                Button1.Enabled = true;
                Timer1.Enabled = false;
                //Let the GC know the async operation could be collected for garbage.
                AsyncOperator = null;
            }
            else
            {
                if (AsyncOperator == null)
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
                _MyRequest = System.Net.WebRequest.Create(address);
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                AsyncOperator = null;
                Button1.Enabled = true;
                Label3.Text = AsyncState as string;
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

        /// <summary>
        /// Do some work like caching/handling viewstate/session.
        /// </summary>        
        public override void ExecuteCachePolicy()
        {
            //throw new ArgumentException("test exception in thread.");
            for (int i = 0; i < 250; i++)
            {
                Thread.Sleep(10);
                AsyncState = "Cache updated " + i.ToString();
                if (Halted)
                {
                    AsyncState = "Halted!";
                    break;
                }
            }
            if (!Halted) AsyncState = "Cache ready.";
            Completed = true;
            Button1.Enabled = true;
            AsyncOperator = null;
            if (_Observer != null) _Observer.Dispose();
            //pContext.Response.Redirect(pContext.Request.UrlReferrer.ToString());
        }
    }
}