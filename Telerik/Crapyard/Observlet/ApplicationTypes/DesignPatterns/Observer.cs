using System;
using System.Windows.Forms;
using ApplicationTypes.Maintenance;
using System.Web;
using System.Diagnostics;
using Timer=System.Threading.Timer;

namespace ApplicationTypes.DesignPatterns
{
    /// <summary>
    /// Observer Pattern Judith Bishop Jan 2007
    /// The Subject runs in a thread and changes
    /// independently. At each change, it notifies its Observers.
    /// Adapted by D.S. Modiwirijo.
    /// </summary>
    public class Observer : IObserver
    {
        private readonly Subject m_Subject;

        /// Note: Logging to be replaced by enterprise library or the like for IO devices/streams:
        #region Logging to standard IO devices/streams

        /// <summary>
        /// Note: to be replaced by enterprise library or the like.
        /// </summary>
        public static void StartLogging()
        {
            if (HttpContext.Current != null) HttpContext.Current.Trace.IsEnabled = true;
            Log.StartLogging(Utility.AssemblyLocation);
        }

        /// <summary>
        /// Note: to be replaced by enterprise library or the like.
        /// </summary>
        public static void StopLogging()
        {
            Log.StopLogging();
        }
        /// <summary>
        /// Note: to be replaced by enterprise library or the like.
        /// </summary>
        public void WriteLine(object value)
        {
            Log.Writeline(value);
        }
        /// <summary>
        /// Note: to be replaced by enterprise library or the like.
        /// </summary>
        public void LogWriteLine(string format, params object[] args)
        {
            Log.Writeline(format, args);
            
        }

        #endregion

        /// <summary>
        /// The Observer subscribes to 2 events and delegates it.
        /// </summary>
        /// <param name="publisher"></param>
        public Observer(Subject publisher)
        {
            StartLogging();
            m_Subject = publisher;
            m_Subject.NotifyLogger += UpdateLog;
            m_Subject.NotifyProgress += UpdateProgress;            
        }

        public int ProgressbarValue { get; set; }

        public void UpdateLog(string text)
        {
            try
            {
                LogWriteLine(text);
                if (HttpContext.Current != null) HttpContext.Current.Trace.Write("Callback UpdateLog ", text);
                m_Subject.Message.Text += text;

                //scrolldown.
                //m_Subject.Message.SelectionStart = m_Subject.Message.Text.Length;
                //m_Subject.Message.ScrollToCaret();
            }
            catch (Exception ex)
            {
                ///TODO: exception handler.
                //MessageBox.Show(" UpdateLog" + ex.Message);
                WriteLine(ex);
            }
        }

        public void UpdateProgress(int progress)
        {
            ProgressbarValue+=progress;
            m_Subject.ProgressBarLogger.Value = progress;
        }

        public void Pause()
        {
            try
            {
                MessageBox.Show("Paused");
            }
            catch (Exception ex)
            {
                ///TODO: exception handler.
                //MessageBox.Show(" UpdateLog" + ex.Message);
                WriteLine(ex);
            }
        }

    }

    public interface IObserver
    {
        void UpdateLog(string text);
        void UpdateProgress(int progress);

    }

}