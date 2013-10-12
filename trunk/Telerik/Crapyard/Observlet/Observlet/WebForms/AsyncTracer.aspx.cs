﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using ApplicationTypes.DesignPatterns;
using System.Diagnostics;
using System.Text;

namespace Observlet.WebForms
{
    public partial class AsyncTracer : System.Web.UI.Page
    {
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
            m_Locker = Session["m_Locker"];
            lock (m_Locker)
            {
                //Message.Text += e.Message;

                #region gettype
                int countNotifications = 0;
                int countMaxNotifications = 0;
                HttpSessionState session = null;
                MySession mySession = null;
                Queue<string> jobQueue = new Queue<string>();

                Type type = e.GetType();
                if (type.UnderlyingSystemType.Name.Equals("NotificationEventArgs"))
                {
                    var notification = (LoggingBase.NotificationEventArgs)e;
                    ///result.Text += notification.Message;                   

                    Trace.Write("ObserverNotified", notification.Message);

                    session = notification.Session;

                    mySession = GetMySession(notification.Session.SessionID);
                    bool taskQIsEmpty = mySession.TaskQIsEmtpty;// DeadLock => session["TaskQ-Empty"] is bool ? (bool)session["TaskQ-Empty"] : false;

                    /////////////////////////////////////////
                    /// WAIT UNTIL THERE ARE NO TASKS.
                    while (!taskQIsEmpty)
                    {
                        Monitor.Wait(m_Locker);
                        mySession = GetMySession(mySession.SessionId);
                        taskQIsEmpty = mySession.TaskQIsEmtpty;
                        Monitor.PulseAll(m_Locker);
                    }
                    /////////////////////////////////////////
                    
                    
                    //session["ObserverNotified"] = m_MessageQ; ERROR

                    if (session != null)
                    {
                        m_MessageQ = session["ObserverNotified"] as Queue<string>;
                        jobQueue = session["TotalJobQueue"] as Queue<string>;
                        countNotifications = session["countNotifications"] is int ? (int)session["countNotifications"] : 0;
                    }
                    Monitor.PulseAll(m_Locker);

                    if (m_MessageQ != null && jobQueue != null)
                    {
                        m_MessageQ.Enqueue(notification.Message);
                        jobQueue.Enqueue(notification.Message);
                        ///Thread.Sleep(10);
                        if (session != null)
                        {
                            session["ObserverNotified"] = m_MessageQ;
                            session["TotalJobQueue"] = jobQueue;
                        }
                    }
                    #region store/restore counters for this session
                    countNotifications++;
                    if (countNotifications == m_MaxValue + 1)
                    {
                        countMaxNotifications = m_MaxValue + 1;
                        if (session != null) session["countMaxNotifications"] = countMaxNotifications;
                    }
                    if (countNotifications == m_MaxValue + 1)
                    {
                        if (session != null) session["countNotifications"] = 0;
                    }
                    else
                        if (session != null) session["countNotifications"] = countNotifications;

                    #endregion

                    if ((countNotifications + 1) % 30 == 0)
                    {
                        UpdateTaskQReadyStatus(mySession, false);

                        Monitor.PulseAll(m_Locker);
                    }
                }

                Type senderType = sender.GetType();
                if (senderType.UnderlyingSystemType.Name.Equals("Timer"))
                {
                    var timer = (System.Web.UI.Timer)sender;
                    //result.Text += DateTime.Now.ToShortTimeString() + "\tFrom " + timer.ToString() + "\n";
                    if (m_MessageQ != null && m_MessageQ.Count > 0) result.Text += m_MessageQ.Dequeue();
                }

                #endregion

                

            }
        }
        /// <summary>
        /// Task Queue must be updated if queue is produced.
        /// </summary>
        /// <param name="mySession"></param>
        /// <param name="isEmpty"></param>
        private void UpdateTaskQReadyStatus(MySession mySession, bool isEmpty)
        {
            Application.Lock();
            ConcurrentDictionary<string, MySession> applicationSessions = Application["MTA-Sessions"] as ConcurrentDictionary<string, MySession>;
            if (applicationSessions != null)
                applicationSessions[mySession.SessionId].TaskQIsEmtpty = isEmpty;//there are 30 tasks to do OR not...
            Application["MTA-Sessions"] = applicationSessions;
            Application.UnLock();
        }

        private MySession GetMySession(string sessionId)
        {
            Application.Lock();
            ConcurrentDictionary<string, MySession> applicationSessions = Application["MTA-Sessions"] as ConcurrentDictionary<string, MySession>;
            MySession mySession = null;
            if (applicationSessions != null)
                applicationSessions.TryGetValue(sessionId, out mySession);
            Application.UnLock();
            return mySession;
        }
        private void SignalConsumerForWork(MySession mySession)
        {
            mySession.ProducerThread = Thread.CurrentThread;
            mySession.TaskQIsEmtpty = false;
            mySession.HasWaitingProducer = false;
            UpdateMySession(mySession);

            Monitor.PulseAll(mySession);
        }

        /// <summary>
        /// Task Queue must be updated if queue is produced.
        /// </summary>
        /// <param name="mySession"></param>
        private void UpdateMySession(MySession mySession)
        {
            Application.Lock();
            ConcurrentDictionary<string, MySession> applicationSessions = Application["MTA-Sessions"] as ConcurrentDictionary<string, MySession>;
            if (applicationSessions != null)
            {
                applicationSessions[mySession.SessionId] = mySession;
            }
            Application["MTA-Sessions"] = applicationSessions;
            Application.UnLock();
        }


        #endregion

        protected void OnTimer1(object sender, EventArgs e)
        {
            m_Locker = Session["m_Locker"];
            lock (m_Locker)
            {

                /////////////////////////////////////////
                /// WAIT UNTIL THERE ARE TASKS.
                MySession mySession = GetMySession(HttpContext.Current.Session.SessionID);
                bool taskQIsEmpty = mySession.TaskQIsEmtpty;// DeadLock => Session["TaskQ-Empty"] is bool ? (bool)Session["TaskQ-Empty"] : false;

                while (false)
                {
                    Monitor.Wait(m_Locker, 1000);
                    mySession = GetMySession(HttpContext.Current.Session.SessionID);
                    taskQIsEmpty = mySession.TaskQIsEmtpty;// DeadLock => Session["TaskQ-Empty"] is bool ? (bool)Session["TaskQ-Empty"] : false;
                    Monitor.PulseAll(m_Locker);
                }
                /////////////////////////////////////////
                
                //result.Text += DateTime.Now.ToShortTimeString() + "\tFrom " + timer.ToString() + "\n";
                m_MessageQ = Session["ObserverNotified"] as Queue<string>;
                Stack<string> messageStack = Session["ObserverNotified-Stacked"] as Stack<string>;


                int countNotifications = Session["countNotifications"] is int ? (int)Session["countNotifications"] : 0;
                int countMaxNotifications = Session["countMaxNotifications"] is int ? (int)Session["countMaxNotifications"] : 0;

                ///Create stack messages to show last messages
                var messagesToTextBox = new StringBuilder();
                while (m_MessageQ != null && m_MessageQ.Count > 0 && messageStack != null)
                {
                    string message = m_MessageQ.Dequeue();
                    messageStack.Push(message);

                    message = countNotifications + "\t" + m_MessageQ.Count + "\t" + message;
                    if (message != null) if (message.Contains("finished")) Timer1.Enabled = false;
                    messagesToTextBox.Append(message);
                }
                if (RbStack.Checked)
                {
                    messagesToTextBox = new StringBuilder();
                    while (messageStack != null && messageStack.Count > 0)
                    {
                        string message = messageStack.Pop();
                        message = countNotifications + "\t" + messageStack.Count + "\t" + message;
                        if (message != null) if (message.Contains("finished")) Timer1.Enabled = false;
                        messagesToTextBox.Append(message);
                    }
                    ///Show stack order
                    if (messagesToTextBox.Length > 0) result.Text = messagesToTextBox.ToString();
                }
                else///Show queue order
                    if (messageStack != null && messageStack.Count > 0) result.Text = messagesToTextBox.ToString();

                UpdateTaskQReadyStatus(mySession, true);
                Session["ObserverNotified"] = m_MessageQ;
                Session["ObserverNotified-Stacked"] = messageStack;

                Interlocks.Text = countNotifications + "\t" + countMaxNotifications + "\n";
                Monitor.PulseAll(m_Locker);

            }
        }

        //private static int countNotifications;
        //private static int countMaxNotifications;
        private object m_Locker = new object();
        private System.Net.WebRequest m_MyRequest;
        private Queue<int> m_TaskIds;
        private Dictionary<int, IAsyncResult> m_AsyncResults;
        private Dictionary<int, AsyncCallback> m_AsyncCallbacks;
        private SubjectImpl m_Subject = new SubjectImpl();
        private Queue<string> m_MessageQ;
        private AsyncDemo m_Producer;
        private Observer m_Observer;
        private int m_MaxValue;
        private const int Sleep = 100;

        void Page_Load(object sender, EventArgs e)
        {
            m_TaskIds = new Queue<int>();
            m_AsyncResults = new Dictionary<int, IAsyncResult>();
            m_AsyncCallbacks = new Dictionary<int, AsyncCallback>();


            if (!IsPostBack)
            {
                m_MessageQ = new Queue<string>();
                Session["ObserverNotified"] = new Queue<string>();//Cross-process?
                Session["ObserverNotified-Stacked"] = new Stack<string>();//Cross-process?
                Session["TotalJobQueue"] = new Queue<string>();
                Session["countMaxNotification"] = 0;
                Session["countMaxNotifications"] = 0;
                Session["TaskQ-Empty"] = true; //init

            }

            if (IsAsync)
            {
                m_Subject = new SubjectImpl();
                m_Subject.Message = new TextBox();

                m_Observer = new Observer(m_Subject);

                // Create an instance of the test class.
                m_Producer = new AsyncDemo(HttpContext.Current.Session, HttpContext.Current.Trace);
                m_Producer.NotifyLogger += m_Observer.UpdateLog;
                m_Producer.AsyncNotificationEvent += ObserverNotified;
                m_Producer.Attach();

                Label1.Text = "Page_Load: thread #" + System.Threading.Thread.CurrentThread.GetHashCode();

                BeginEventHandler bh = new BeginEventHandler(this.BeginGetAsyncData);
                EndEventHandler eh = new EndEventHandler(this.EndGetAsyncData);

                AddOnPreRenderCompleteAsync(bh, eh);

                // Initialize the WebRequest.
                string address = "http://localhost/";

                m_MyRequest = System.Net.WebRequest.Create(address);
            }
        }


        private void DoAsyncOperation()
        {
            m_Locker = Session["m_Locker"];
            lock (m_Locker)
            {
                HttpSessionState session = HttpContext.Current.Session;

                Trace.Write("DoAsyncOperation", "-------------------- Doing AsyncOperation ------------");

                // The asynchronous method puts the thread id here.
                int threadId;

                // Create the delegate.
                AsyncCreateQueue caller = new AsyncCreateQueue(m_Producer.CreateQueue);
                AsyncCallback callBack = new AsyncCallback(CallBackResult);
                Thread.Sleep(0);
                Console.WriteLine("Main thread {0} does some work.",
                                  Thread.CurrentThread.ManagedThreadId);

                // Initiate the asychronous call.
                IAsyncResult result = caller.BeginInvoke(out threadId, m_MaxValue, Sleep, callBack, null);

                //result.AsyncWaitHandle.WaitOne();
                Trace.Write("BeginAsyncOperation",
                            string.Format(
                                "Waiting for the WaitHandle to become signaled...\nThreadId={0} \tResultState = {1}\t Caller= {2}",
                                threadId, result.AsyncState, caller));
                Monitor.PulseAll(m_Locker);//in doubt pulsall...
            }
        }

        private void CallBackResult(IAsyncResult result)
        {
            int threadId;
            //Queue<int> threadIds = m_TaskIds;// Session["TaskIds"] as Queue<int>;

            AsyncCreateQueue ac = (AsyncCreateQueue)((AsyncResult)result).AsyncDelegate;
            string[] returnValue = ac.EndInvoke(out threadId, result);

            //result.AsyncWaitHandle.Close();            
        }
        IAsyncResult BeginGetAsyncData(Object src, EventArgs args, AsyncCallback cb, Object state)
        {
            //Timer1.Enabled = true;
            Label2.Text = "BeginGetAsyncData: thread #" + Thread.CurrentThread.GetHashCode();
            Trace.Write("BeginGetAsyncData", Label2.Text);

            return m_MyRequest.BeginGetResponse(cb, state);
        }
        void EndGetAsyncData(IAsyncResult ar)
        {
            int threadId = Thread.CurrentThread.GetHashCode();
            //int threadId = Thread.CurrentThread.ManagedThreadId;
            Label3.Text = "EndGetAsyncData: thread #" + threadId;
            Trace.Write("EndGetAsyncData", Label3.Text);

            //System.Net.WebResponse myResponse = myRequest.EndGetResponse(ar);
            string message;
            //Trace.Write("EndGetAsyncData", "Thread.Sleep(5000)...");
            //Thread.Sleep(5000);
            ///TryGetAsyncResult(threadId, out message);

            ///if(!string.IsNullOrEmpty(message)) result.Text += message + "...Done.\n";
            //myResponse.Close();

        }
        /// <summary>
        /// Toggle 1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnToggle_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = true;
            m_MaxValue = Convert.ToInt32(MaxLoop.Text);
            DoAsyncOperation();
            //Timer1.Enabled = false;
        }
        /// <summary>
        /// Toggle 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnToggle0_Click(object sender, EventArgs e)
        {
            //Timer1.Enabled = true;
            m_MaxValue = Convert.ToInt32(MaxLoop.Text);
            DoAsyncOperation();

        }
        /// <summary>
        /// Clear list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            result.Text = "";
            Interlocks.Text = "";
        }
        /// <summary>
        ///  Pause togle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnTimer_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = !Timer1.Enabled;

        }

        protected void RbStack_CheckedChanged(object sender, EventArgs e)
        {
            RbQueue.Checked = false;
            RbStack.Checked = true;
        }

        protected void RbQueue_CheckedChanged(object sender, EventArgs e)
        {
            RbQueue.Checked = true;
            RbStack.Checked = false;
        }

        protected void BtnShowAll_Click(object sender, EventArgs e)
        {
            var queue = Session["TotalJobQueue"] as Queue<string>;
            if (queue != null && queue.Count > 0)
            {
                var stringBuilder = new StringBuilder();

                if (RbStack.Checked)
                {
                    var stack = queue.Reverse<string>();
                    int i = stack.Count() - 1;
                    foreach (var elt in stack)
                    {
                        stringBuilder.Append(i-- + "\t" + elt);
                    }
                }
                else //Queued
                {
                    int i = 0;
                    foreach (var elt in queue)
                    {
                        stringBuilder.Append(i++ + "\t" + elt);
                    }
                }

                result.Text = stringBuilder.ToString();
            }
        }
        #region "Scrap"
        private void DoTestAsyncOperation()
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo(HttpContext.Current.Session, HttpContext.Current.Trace);

            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(ad.TestMethod);

            Thread.Sleep(0);
            Console.WriteLine("Main thread {0} does some work.",
                Thread.CurrentThread.ManagedThreadId);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(1000,
                                                     out threadId,
                null, null);

            result.AsyncWaitHandle.WaitOne();
            Trace.Write("BeginAsyncOperation",
               string.Format("Waiting for the WaitHandle to become signaled...\nThreadId={0} \tResultState = {1}\t Caller= {2}",
               threadId, result.AsyncState, caller));

            // Perform additional processing here.
            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".",
                threadId, returnValue);

            Queue<int> threadIds = m_TaskIds;// Session["TaskIds"] as Queue<int>;
            if (threadIds != null) threadIds.Enqueue(threadId);
            if (m_AsyncResults != null) m_AsyncResults.Add(threadId, result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();
        }
        private void TryGetAsyncResult(int threadId, out string message)
        {
            AsyncMethodCaller caller;
            IAsyncResult result;
            message = "";

            Queue<int> threadIds = m_TaskIds;// Session["TaskIds"] as Queue<int>;
            if (threadIds != null && threadIds.Count > 0)
            {
                threadId = threadIds.Dequeue();
                IDictionary<int, IAsyncResult> asyncResult = m_AsyncResults;
                // Session["AsyncResults"] as Dictionary<int, IAsyncResult>;

                ///Return if the results or callers does not exist.
                if (asyncResult == null)
                {
                    message = "";
                    return;
                }

                asyncResult.TryGetValue(threadId, out result);

                ///Return if the result or caller does not exist.
                if (result == null)
                {
                    message = "";
                    return;
                }

                Trace.Write("EndAsyncOperation",
                            string.Format("ThreadId={0} \tResultValue = {1}) ", threadId,
                                          result));

                // Perform additional processing here.
                // Call EndInvoke to retrieve the results.
                string returnValue = string.Format("{0}", result);

                // Close the wait handle.
                //result.AsyncWaitHandle.Close();

                Trace.Write("EndAsyncOperation",
                            string.Format("The call executed on thread {0}, with return value \"{1}\".",
                                          threadId, returnValue));
                message = returnValue;
            }

        }
        #endregion
    }
}
