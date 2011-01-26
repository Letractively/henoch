using System;

namespace TelerikExample
{
    /// <summary>
    /// Represents the observer from the observer pattern.
    /// </summary>
    /// <typeparam name="TSubject"></typeparam>
    /// <typeparam name="TSubscriber"></typeparam>
    public class Observer<TSubject, TSubscriber> : IDisposable
        where TSubject : ISubject
        where TSubscriber : ISubscriber
    {
        private readonly TSubject m_Subject;
        private readonly TSubscriber m_Subscriber;

        public Observer(TSubject subject, TSubscriber subscriber)
        {
            m_Subject = subject;
            m_Subscriber = subscriber;
            //m_Subject.NotifyUpdateHandler += DoNotifyUpdate;
            m_Subject.NotifyLogHandler += DoNotify;
            m_Subscriber.NotifyHaltHandler += DoNotifyStop;
        }

        private void DoNotify(object sender, NotifyObserverEventargs e)
        {
            //use explicit implementation;
            ISubscriber subscriber = m_Subscriber;
            if (subscriber != null) subscriber.Log(e.Message);
        }
        private void DoNotifyStop(object sender, NotifyObserverEventargs notifyObserverEventargs)
        {
            //use explicit implementation;
            ISubject subject= m_Subject;
            if (subject != null) subject.Stop();
        }
        #region Formalized Disposal Pattern

        // Used to determine if Dispose()
        // has already been called.
        private bool disposed;

        public void Dispose()
        {
            // Call our helper method.
            // Specifying "true" signifies that
            // the object user triggered the cleanup.
            CleanUp(true);
            // Now suppress finalization.
            GC.SuppressFinalize(this);
        }

        private void CleanUp(bool disposing)
        {
            // Be sure we have not already been disposed!
            if (!disposed)
            {
                m_Subject.NotifyLogHandler -= DoNotify;
                // If disposing equals true, dispose all managed resources.
                if (disposing)
                {
                    // Dispose managed resources. 
                }
                // Clean up unmanaged resources here.
            }
            disposed = true;
        }

        ~Observer()
        {
            // Call our helper method.
            // Specifying "false" signifies that
            // the GC triggered the cleanup.
            CleanUp(false);
        }

        #endregion
    }

    public interface ISubscriber
    {
        event EventHandler<NotifyObserverEventargs> NotifyHaltHandler;
        void NotifyHalt(NotifyObserverEventargs args);

        void Log(string message);
    }

    /// <summary>
    /// the Subject role of the Observer Pattern must implement this interface.
    /// </summary>
    public interface ISubject
    {
        //event EventHandler<NotifyObserverEventargs> NotifyUpdateHandler;
        event EventHandler<NotifyObserverEventargs> NotifyLogHandler;       
        //void NotifyObserverUpdate(NotifyObserverEventargs args);
        void NotifyObserverLog(NotifyObserverEventargs args);

        void Stop();
    }
    public abstract class SubjectBase : ISubject
    {
        protected bool _Stop;

        public delegate void NotificationEventHandler(object sender, NotifyObserverEventargs e);

        #region Implementation of ISubject

        public event EventHandler<NotifyObserverEventargs> NotifyLogHandler;

        public void NotifyObserverLog(NotifyObserverEventargs args)
        {
            NotifyLogHandler.Invoke(this,args);
        }
        public void Stop()
        {
            _Stop = true;
        }

        #endregion
    }


    public class NotifyObserverEventargs : EventArgs
    {
        public readonly string Message;        
        private NotifyObserverEventargs()
        {
            //clients must update at least 1 property.
        }

        public NotifyObserverEventargs(string message)
        {
            Message = message;
        }
    }
}