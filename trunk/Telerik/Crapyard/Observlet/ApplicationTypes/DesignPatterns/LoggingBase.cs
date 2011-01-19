using System;
using System.Web;
using System.Web.SessionState;

namespace ApplicationTypes.DesignPatterns
{
    /// <summary>
    /// Any class who is an subject but NOT an usercontrol in the observer pattern must inherit LoggingBase for Logging.
    /// </summary>
    public abstract class LoggingBase:ISubject
    {
        public bool Cancel { get; set; }
        protected string m_Message;
        protected HttpSessionState m_Session;
        protected TraceContext m_Trace;
        protected HttpApplicationState m_Application;

        public event CallbackNotifyer NotifyLogger;
        // ReSharper disable InconsistentNaming
        protected CallbackNotifyer m_NotifyLogger;//Logger
        // ReSharper restore InconsistentNaming

        public delegate void CallbackNotifyer(string textMessage);

        public event CallbackNotifyProgress NotifyProgress;
        protected CallbackNotifyProgress m_NotifyProgress;
        public delegate void CallbackNotifyProgress(int progress);

        public string LogId { get; set; }
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
            var args = new NotificationEventArgs(message);
            OnNotificationEvent(args);//for desktop
        }
        /// <summary>
        /// Web variant.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="message"></param>
        public void NotifyObserver(HttpSessionState session, string message)
        {
            if (NotifyLogger != null) NotifyLogger(message); //+ "\n"
            var args = new NotificationEventArgs(session, message);
            OnAsyncNotificationEvent(args);//for web
        }
        public void NotifyObserver(HttpSessionState session, bool isFinised)
        {
            const string message = "finished.";
            if (NotifyLogger != null) NotifyLogger(message); //+ "\n"
            var args = new NotificationEventArgs(session, message, isFinised);
            OnAsyncNotificationEvent(args);//for web
        }
        #region Implementation of ISubject

        /// <summary>
        /// Subscribe to the log-event delegating to the observer.
        /// </summary>
        public void Attach()
        {
            m_NotifyLogger += NotifyLogger;
            m_NotifyProgress += NotifyProgress;
        }
        /// <summary>
        /// Unsubscribe to the log-event delegating to the observer.
        /// </summary>
        public void Detach()
        {
            m_NotifyLogger -= NotifyLogger;
            m_NotifyProgress -= NotifyProgress;
        }

        #endregion

        #region Eventhandler for updating the observer's logapplication

        public event NotificationEventHandlerAsync AsyncNotificationEvent;
        public event NotificationEventHandler NotificationEvent;

        private void OnNotificationEvent(NotificationEventArgs e)
        {
            var handler = NotificationEvent;
            if (handler != null) handler(this, e);
        }
        private void OnAsyncNotificationEvent(NotificationEventArgs e)
        {
            var handler = AsyncNotificationEvent;
            if (handler != null) handler(this, e);
        }
        /// <summary>
        /// Delegate declaration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);
        public delegate void NotificationEventHandlerAsync(object sender, NotificationEventArgs e);

        /// <summary>
        /// Class that contains the data for 
        /// the Notification event. Derives from System.EventArgs.
        /// </summary>
        public class NotificationEventArgs : EventArgs
        {
            private readonly string m_Message;
            private readonly HttpSessionState m_Session;
            private readonly bool m_IsFinished;
            //Constructor.
            //
            public NotificationEventArgs(string message)
            {
                m_Message = message;
            }

            //Constructor.
            //
            public NotificationEventArgs(HttpSessionState session, string message)
            {
                m_Message = message;
                m_Session = session;
            }
            public NotificationEventArgs(HttpSessionState session, string message, bool isFinished)
            {
                m_IsFinished = isFinished;
                m_Message = message;
                m_Session = session;
            }

            public bool IsFinished
            {
                get { return m_IsFinished; }
            }

            /// <summary>
            /// Represents Current session.
            /// </summary>
            public HttpSessionState Session
            {
                get { return m_Session; }
            }

            public string Message
            {
                get { return m_Message; }
            }

        }


        #endregion
    }
}