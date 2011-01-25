﻿using System;
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
            if (!IsPostBack && IsAsync)
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
            else
            {

            }
        }
        public bool IsReusable
        {
            get { return false; }
        }
        #region Implementation of IHttpAsyncHandler

        public IAsyncResult BeginProcessRequest(object sender, EventArgs eventArgs, AsyncCallback cb, object extraData)
        {
            Session["AsyncIsCompleted"] = null;
            Label1.Text = "BeginGetAsyncData: thread #" + Thread.CurrentThread.ManagedThreadId;
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

        public void EndProcessRequest(IAsyncResult result)
        {                        
            int threadId = Thread.CurrentThread.ManagedThreadId;
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
                Label3.Text = Session["label3"] as string;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (Session["AsyncIsCompleted"]!=null)
                Timer1.Enabled = false;
        }

        private void CallBackResult(IAsyncResult result)
        {
            int threadId;
            //Queue<int> threadIds = m_TaskIds;// Session["TaskIds"] as Queue<int>;

            //If ac is a delegate: AsynchOperationPattern ac = (AsynchOperationPattern)((AsyncResult)result).AsyncDelegate;
            var res = result;
            
            //result.AsyncWaitHandle.Close();     
            if (operationPattern.IsCompleted)
            {
                Timer1.Enabled = false;

                Session["AsyncIsCompleted"] = "AsyncIsCompleted";
               
            }
            Session["label3"] = operationPattern.AsyncState;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}