namespace MatStudio.DesignPatterns
{
    /// <summary>
    /// Any class who is an subject but NOT an usercontrol in the observer pattern must inherit LoggingBase for Logging.
    /// </summary>
    public abstract class LoggingBase:ISubject
    {
        private string m_LogId;

        public event CallbackNotifyer NotifyLogger;
                                                        // ReSharper disable InconsistentNaming
        protected CallbackNotifyer m_NotifyLogger;//Logger
                                                        // ReSharper restore InconsistentNaming

        public delegate void CallbackNotifyer(string textMessage);

        public string LogId
        {
            get { return m_LogId; }
            set { m_LogId = value; }
        }

        #region Implementation of ISubject

        /// <summary>
        /// Subscribe to the log-event delegating to the observer.
        /// </summary>
        public void Subscribe()
        {
            m_NotifyLogger += NotifyLogger;
        }
        /// <summary>
        /// Unsubbscribe to the log-event delegating to the observer.
        /// </summary>
        public void Unsubscribe()
        {
            m_NotifyLogger -= NotifyLogger;
        }

        #endregion
    }
}