using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Web.UI;
using System.Windows.Forms;
using ApplicationTypes.DesignPatterns;
using Observlet.Workers;
using TextBox=System.Web.UI.WebControls.TextBox;
using Timer=System.Threading.Timer;

namespace Observlet.WebForms
{
    public partial class Tracer : Page, ApplicationTypes.DesignPatterns.ISubject
    {
        private static BackgroundWorker m_BackgroundWorker = new BackgroundWorker();
        private SubjectImpl m_Subject = new SubjectImpl();

        private LoggingBase.CallbackNotifyer m_Notifyer;

        #region Implementation of ISubject

        public void Attach()
        {
            throw new NotImplementedException();
        }

        public void Detach()
        {
            throw new NotImplementedException();
        }

        public event LoggingBase.CallbackNotifyer NotifyLogger;
        public event LoggingBase.CallbackNotifyProgress NotifyProgress;

        #endregion
        #region TODO replace by a design pattern
        public event LoggingBase.NotificationEventHandler NotificationEvent;
        protected LoggingBase.CallbackNotifyer m_NotifyLogger;
        /// <summary>
        /// If the observer and its subject has the same subscription(event)
        /// and the subject subscribes the event a message can be handled
        /// by the observer: the observer has a log/logapplication/device.
        /// The observer may use tracing via streams or console output.
        /// </summary>
        /// <param name="message"></param>
        public void NotifyObserver(string message)
        {
            if (m_NotifyLogger != null) m_NotifyLogger(message); //+ "\n"
            var args = new LoggingBase.NotificationEventArgs(message);
            OnNotificationEvent(args);

        }
        private void OnNotificationEvent(LoggingBase.NotificationEventArgs e)
        {
            var handler = NotificationEvent;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// NotifyObserver has been done. Time to update the logapplication/device/resource.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ObserverNotified(object sender, EventArgs e)
        {
            //Message.Text += e.Message;
            #region gettype
            Type type = e.GetType();
            if (type.UnderlyingSystemType.Name.Equals("NotificationEventArgs"))
            {
                var notification = (LoggingBase.NotificationEventArgs)e;
                
                Trace.Write("ObserverNotified", notification.Message);

                Queue<string> messageQ = Session["ObserverNotified"] as Queue<string>;
                if (messageQ != null) messageQ.Enqueue(notification.Message);
                Session["ObserverNotified"] = messageQ;

                Message.Text = notification.Message;
            }

            Type senderType = sender.GetType();
            if (senderType.UnderlyingSystemType.Name.Equals("Timer"))
            {
                var timer = (System.Web.UI.Timer)sender;
                Message2.Text += DateTime.Now.ToShortTimeString() + "\tFrom " + timer.ToString() + "\n";
            }

            #endregion
        }
        #endregion

        private WebRequest _request;
        private Queue<int> m_TaskIds;
        private Dictionary<int, IAsyncResult> m_AsyncResults;
        private Dictionary<int, AsyncMethodCaller> m_AsyncCallers;
        private int MaxLoop;


        protected void OnTimer(object sender, EventArgs e)
        {
            Message2.Text += DateTime.Now.ToShortTimeString() + "\tFrom Timer1\n";
        }

        protected void OnTimer2(object sender, EventArgs e)
        {
            //Message2.Text += DateTime.Now.ToShortTimeString() + "\n";
            var messageQ = Session["ObserverNotified"] as Queue<string>;
            if (messageQ != null && messageQ.Count>0)
            {
                Message2.Text += messageQ.Dequeue();
                Session["ObserverNotified"] = messageQ;
            }
            else
                Message2.Text += DateTime.Now.ToShortTimeString() + "\n";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Write("Page_Load", "IsPostBack = " + IsPostBack);
            if(!IsPostBack)
            {
                Session["ObserverNotified"] = new Queue<string>();           
                ///Use the taskid (threadid) to store/restore async callers & results
                //Session["TaskIds"] = new Queue<int>();
                //Session["AsyncResults"] = new Dictionary<int, IAsyncResult>();
                //Session["AsyncCallers"] = new Dictionary<int, AsyncMethodCaller>();
                m_TaskIds = new Queue<int>();
                m_AsyncResults = new Dictionary<int, IAsyncResult>();
                m_AsyncCallers = new Dictionary<int, AsyncMethodCaller>();
            }
            if (IsAsync)
            {
                //BeginEventHandler bh = new BeginEventHandler(this.BeginAsyncOperation);
                //EndEventHandler eh = new EndEventHandler(this.EndAsyncOperation);

                //AddOnPreRenderCompleteAsync(bh, eh);

                //// Initialize the WebRequest.
                //string address = "http://localhost/";
                //_request = System.Net.WebRequest.Create(address);
            }
        }

        private void DoBackgroundWork()
        {
            //Timer1.Enabled = true;
            Thread.Sleep(1500);
            Trace.Write("DoBackgroundWork", "-------------------- Doing BackgroundWork ------------");
            using (BackgroundWorker lBackgroundWorker = new BackgroundWorker())
            {
                lBackgroundWorker.DoWork += backgroundWorker_Work;
                lBackgroundWorker.WorkerSupportsCancellation = true;
                lBackgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
                lBackgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

                lBackgroundWorker.RunWorkerAsync();
            }
            Timer1.Enabled = false;
        }

        void backgroundWorker_Work(object sender, DoWorkEventArgs e)
        {
            DoWork();
        }

        private void DoWork()
        {
            try
            {
                Trace.Write("DoWork", "-------------------- Doing Work ------------");
                m_Subject = new SubjectImpl();
                m_Subject.Message = new TextBox();

                Observer observer = new Observer(m_Subject);
                
                //Worker worker = new Worker();

                NotifyLogger += observer.UpdateLog;
                NotificationEvent += ObserverNotified;
                Message.TextChanged+=Message_TextChanged;

                //worker.Attach();
                //worker.Cancel = false;
                ////worker.Bereken(m_ProjectId, m_RootDir, m_Sof, m_Saf);
                DoDummyWork();

                Message.Text += "Gereed.\n\r";

            }
            catch (ThreadInterruptedException)
            {
                Message.Text = "interrupted!";
                Console.WriteLine("~~~~ thread2 interrupted...");
            }
            catch (Exception ex)
            {
                Message.Text = "interrupted!!" + ex.Message;
                if (NotifyLogger != null) NotifyLogger("\n" + ex.Message);
                if (NotifyLogger != null) NotifyLogger("\n" + ex.StackTrace);
                Console.Out.Write(ex);
                Console.WriteLine("~~~~ thread2 error...");
            }
            finally
            {
                Console.WriteLine("~~~~ thread2 initData2 ends ...~~~~");
                //button1.Enabled = true;
               
            }
        }
        public void DoDummyWork()
        {
            try
            {
                MaxLoop = Convert.ToInt32(BtnMaxLoop.Text);
                for (int i = 0; i < MaxLoop; i++)
                {

                    double result = (i);

                    string message= "Message\t" + DateTime.Now + "\t:" + string.Format(CultureInfo.InvariantCulture, "precision = {0}\n",
                            Convert.ToDouble(result).ToString("F1", CultureInfo.InvariantCulture));

                    NotifyObserver(message);
                    Random rnd1 = new Random();
                    Thread.Sleep(rnd1.Next(1000));
                }

            }
            catch (ThreadInterruptedException)
            {
                Message.Text = "interrupted..";
                Console.WriteLine("~~~~ thread2 interrupted...");
            }
            catch (Exception)
            {
                Message.Text = "interrupted...";
                Console.WriteLine("~~~~ thread2 error...");
            }
            finally
            {
                Console.WriteLine("~~~~ thread2 initData2 ends ...~~~~");

            }
        }
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ///manager is not an observer!!
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
               StartWork.Enabled = true;
               StartBackgroundWork.Enabled = true;

               Worker worker = new Worker();
               worker.Detach();
               worker.Cancel = false;
               Thread.Sleep(200);
               Timer1.Enabled = false;
            }
            finally
            {
                StartWork.Enabled = true;
                StartBackgroundWork.Enabled = true;

            }
            //Unsubscribe();
        }

        protected void Message_TextChanged(object sender, EventArgs e)
        {
         
        }

        protected void BtnTicker_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = !Timer1.Enabled;
        }



        protected void DoWork_Click(object sender, EventArgs e)
        {
            //Timer timer = new Timer(OnCurThreadtimer);
            //timer.Change(100, 500);                        
            
            //DoWork();
            //AddOnPreRenderCompleteAsync(new BeginEventHandler(BeginAsyncOperation), new EndEventHandler(EndAsyncOperation));

        }
        #region Async page
        /// <summary>
        /// Main: Async operation beginning using WaitOne (waithandler)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="cb"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        IAsyncResult BeginAsyncOperation(object sender,EventArgs e,AsyncCallback cb,object state)
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            Thread.Sleep(0);
            Console.WriteLine("Main thread {0} does some work.",
                Thread.CurrentThread.ManagedThreadId);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000,
                                                     out threadId, 
                                                     //null, null);
                new AsyncCallback(EndAsyncOperation),
                "The call executed on thread {0}, with return value \"{1}\".");

            Trace.Write("BeginAsyncOperation",
               string.Format("Waiting for the WaitHandle to become signaled...\nThreadId={0} \tResultState = {1}\t Caller= {2}) ", 
               threadId, result.AsyncState, caller));

            result.AsyncWaitHandle.WaitOne();

            // Perform additional processing here.
            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);            

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);

            Queue<int> threadIds = m_TaskIds;// Session["TaskIds"] as Queue<int>;
            if (threadIds != null) threadIds.Enqueue(threadId);
            IDictionary<int, IAsyncResult> asyncResult = m_AsyncResults;// Session["AsyncResults"] as Dictionary<int, IAsyncResult>;
            if (asyncResult != null) asyncResult.Add(new KeyValuePair<int, IAsyncResult>(threadId, result));
            IDictionary<int, AsyncMethodCaller> asyncCallers = m_AsyncCallers;// Session["AsyncCallers"] as Dictionary<int, AsyncMethodCaller>;
            if (asyncCallers != null) asyncCallers.Add(new KeyValuePair<int, AsyncMethodCaller>(threadId, caller));

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            return result;

        }
        //Async operation ending
        void EndAsyncOperation(IAsyncResult ar)
        {
            
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            //builder.Append(returnValue);
            //Message2.Text = builder.ToString();                
        }
        #endregion

        private void OnCurThreadtimer(object state)
        {
            string message = DateTime.Now.ToShortTimeString() + "\tFrom OnCurThreadtimer\n";
            Trace.Write("OnCurThreadtimer", message);
            Message2.Text += message;
        }

        protected void DoBackgroundWork_Click(object sender, EventArgs e)
        {
            Timer timer = new Timer(OnCurThreadtimer);
            timer.Change(100, 500);
            DoBackgroundWork();
            
        }

        protected void BtnTicker2_Click(object sender, EventArgs e)
        {
            Timer2.Enabled = !Timer2.Enabled;
        }



    }

    class SubjectImpl : Subject
    {
        #region Overrides of Subject

        public override ISite Site { get; set; }
        public override event EventHandler Disposed;
        public override void Dispose()
        {
            //Dispose();
        }

        public override System.Web.UI.WebControls.TextBox Message { get; set; }

        public override ProgressBar ProgressBarLogger { get; set; }

        #endregion
    }
}