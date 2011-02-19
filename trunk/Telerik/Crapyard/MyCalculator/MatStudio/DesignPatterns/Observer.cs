using System;
using System.Windows.Forms;
using MatStudio.Forms;

namespace MatStudio.DesignPatterns
{
    public class ObserverPattern
    {
        // Observer Pattern Judith Bishop Jan 2007
        // The Subject runs in a thread and changes its state
        // independently. At each change, it notifies its Observers.
        // Adapted by D.S. Modiwirijo.
        public interface IObserver
        {
            void Update(double width, double height);
            void UpdateLog(string text);
        }
        public class Observer : IObserver
        {

            private Form _subjectManager;
            private Form _LogApplication;
            private int m_ProgressbarValue;

            public Observer(Form subject, Form observerChildForm)
            {
                
                _subjectManager = subject ;
                _LogApplication = observerChildForm;
                LayoutManager manager =  subject.Controls[0] as LayoutManager;
                if (manager != null)
                {
                    manager.Notify += Update;
                    manager.NotifyLogger += UpdateLog;
                }
            }


            public Form SubjectManager
            {
                get { return _subjectManager; }
            }

            public Form LogApplication
            {
                get { return _LogApplication; }
                //set { _LogApplication = value; }
            }

            public int ProgressbarValue
            {
                get { return m_ProgressbarValue; }
                set { m_ProgressbarValue = value; }
            }


            public void Update(double widthPerc, double heightPerc)
            {
                try
                {
                    double width = (widthPerc / 10000) * Convert.ToDouble( LogApplication.Width);
                    LogApplication.Controls[0].Width = (int) width;

                    ///_observerChildForm.Text = _observerChildForm.Controls[0].Width.ToString();
                    ///_observerChildForm.Controls[0].Height = (int) (heightPerc / 100) * (_observerChildForm.Height - 50);
                    Console.WriteLine((widthPerc / 100) * LogApplication.Width);
                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    Console.WriteLine(widthPerc + "\t" + heightPerc);
                }
            }

            public void UpdateLog(string text)
            {
                try
                {
                    if (LogApplication.Controls != null)
                    {
                        LogControl logger = LogApplication.Controls[0] as LogControl;
                        if (logger != null)
                        {
                            logger.Message.Text += text;
                            logger.ProgressBarLogger.Value = ProgressbarValue++;
                            //scrolldown.
                            logger.Message.SelectionStart = logger.Message.Text.Length;
                            logger.Message.ScrollToCaret();
                        }
                    }
                }
                catch(Exception ex)
                {
                    ///TODO: exception handler.
                    //MessageBox.Show(" UpdateLog" + ex.Message);
                }
                finally
                {

                }
            }
        }
    }
}