namespace MatStudio.DesignPatterns
{
    /// <summary>
    /// the Subject role of the Observer Pattern must implement this interface.
    /// </summary>
    public interface ISubject
    {
        void Subscribe();
        void Unsubscribe();
        event LoggingBase.CallbackNotifyer NotifyLogger;//Logger 
    }
}