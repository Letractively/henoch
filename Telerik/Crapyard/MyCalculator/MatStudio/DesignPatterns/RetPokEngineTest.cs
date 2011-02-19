using System;
using System.Globalization;
using System.Threading;

namespace MatStudio.DesignPatterns
{
    public class RetPokEngineTest: LoggingBase, IDisposable
    {
        public bool Cancel { get; set; }
        private string m_Message;

        public string Message
        {
            get { return m_Message; }
        }


        public void DoWork()
        {
            try
            {
                
                for (int i = 0; i < 30; i++)
                {

                    
                    double result = (i);

                    m_Message = "Message\t" + DateTime.Now + "\t:" + string.Format(CultureInfo.InvariantCulture, "precision = {0}\n\r",
                            Convert.ToDouble(result).ToString("F16", CultureInfo.InvariantCulture));
                    if (m_NotifyLogger != null) m_NotifyLogger(m_Message);

                    if (Cancel)
                    {
                        m_Message = "Geannuleerd.";
                        break;
                    }
                    m_Message = "Gereed.";
                    Thread.Sleep(200);
                }


            }
            catch (ThreadInterruptedException)
            {
                m_Message = "interrupted..";
                Console.WriteLine("~~~~ thread2 interrupted...");
            }
            catch (Exception)
            {
                m_Message = "interrupted...";
                Console.WriteLine("~~~~ thread2 error...");
            }
            finally
            {
                Console.WriteLine("~~~~ thread2 initData2 ends ...~~~~");

            }
        }


        #region Disposing resources
        //public void Unsubscribe(LoggingBase.CallbackNotifyer notifyLogger)
        //{
        //    m_NotifyLogger -= notifyLogger;
        //}
        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~RetPokEngineTest()      
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #region Implementation of IDisposable
        private bool Disposed;
        public void Dispose()
        {
            // Implement IDisposable.
            // Do not make this method virtual.
            // A derived class should not be able to override this method.
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);

        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be Disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be Disposed.
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!Disposed)
            {

                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    //MyDataConsumer.Dispose();
                    Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                Disposed = true;

            }
        }

        #endregion

        #endregion

    }
}
