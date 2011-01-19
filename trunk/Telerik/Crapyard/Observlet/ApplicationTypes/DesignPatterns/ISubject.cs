namespace ApplicationTypes.DesignPatterns
{
    /// <summary>
    /// the Subject role of the Observer Pattern must implement this interface.
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// Subscribes to an public event which has subscribers
        /// </summary>
        void Attach();
        /// <summary>
        /// Unsubscribes to an public event which has subscribers
        /// </summary>
        void Detach();
        event LoggingBase.CallbackNotifyer NotifyLogger;//Logger 
        event LoggingBase.CallbackNotifyProgress NotifyProgress;
    }
}