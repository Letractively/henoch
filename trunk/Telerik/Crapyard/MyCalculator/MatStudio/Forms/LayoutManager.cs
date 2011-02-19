using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MatStudio.DesignPatterns;
using RetPok.Net;

namespace MatStudio.Forms
{


    /// <summary>
    /// Represents the manager who playes the subject role in the Observer Pattern.
    /// </summary>
    public partial class LayoutManager : UserControl, ISubject
    {
        private static BackgroundWorker backgroundWorker = new BackgroundWorker();
        private static IDictionary<string, BackgroundWorker> m_Qprocesses= new  Dictionary<string,BackgroundWorker>();
        private static IDictionary<string, RetPokEngineTest> m_Qworkers = new Dictionary<string, RetPokEngineTest>();

        private RetPokEngineTest m_Worker;

        public BackgroundWorker BackgroundWorker
        {
            get{ return backgroundWorker;}set{ backgroundWorker = value;}
        }

        private IDictionary<string, ObserverPattern.Observer> _ObserverManager = new Dictionary<string, ObserverPattern.Observer>();
        private string message;
        private int maxWidth;
        private int maxHeight;
        private InitData Result;
        ///private Form logForm;

        public LayoutManager()
        {
            InitializeComponent();
            maxWidth = 10000;
            maxHeight = 10000;

            hScrollBar1.Maximum = maxWidth;
            hScrollBar2.Maximum = maxHeight;
            hScrollBar1.Value = maxWidth;
            hScrollBar2.Value = maxHeight;
            
        }

        #region Subject role in Observer Pattern
        public event LoggingBase.CallbackNotifyer NotifyLogger;//Logger

        /// <summary>
        /// Lets the worker subscribe to the log-event delegating to the observer.
        /// </summary>
        public void Subscribe()
        {
            if (m_Worker == null) m_Worker = new RetPokEngineTest{LogId = Tag.ToString()};
            m_Qworkers.TryGetValue(m_Worker.LogId, out m_Worker);

            if (m_Worker == null)
            {
                m_Worker = new RetPokEngineTest { LogId = Tag.ToString() };
                m_Qworkers.Add(m_Worker.LogId, m_Worker);
            }
            m_Worker.NotifyLogger += NotifyLogger;
            m_Worker.Cancel = false;
        }

        protected void InitWorker(RetPokEngineTest worker)
        {
            worker.NotifyLogger += NotifyLogger;
            worker.Cancel = false;
            //worker.Subscribe();
        }

        /// <summary>
        /// Lets the worker unsubscribe to the log-event delegating to the observer.
        /// </summary>
        public void Unsubscribe()
        {
            m_Qworkers.TryGetValue(Tag.ToString(), out m_Worker);
            if (m_Worker != null)
            {
                m_Worker.NotifyLogger -= NotifyLogger;
                m_Worker.Cancel = true;
                //m_Worker.Unsubscribe();
            }
            BackgroundWorker processWorker;
            m_Qprocesses.TryGetValue(Tag.ToString(), out processWorker);
            if (processWorker!=null) processWorker.CancelAsync();

           
        }
        #endregion

        #region Other Subject role but not refactored into interface and abstract class
        public delegate void Callback(double width, double height);
        public event Callback Notify;
                
        void Run()
        {
            Notify(hScrollBar1.Value, hScrollBar2.Value);
        }
/*
        void RunLogger()
        {
            ///NotifyLogger("Message\t" + DateTime.Now + "\t:");
        }
*/

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

            new Thread(new ThreadStart(Run)).Start();
        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = ((CheckBox) sender).Checked;
            
            if(isChecked)
            {
                ShowNewLogger();
            }
            else
            {
                Form logForm;
                logForm = GetLogForm();
                
                logForm.Dispose();
                logForm = null;
                CleanLoggerAndCheckNewLogger();
            }
        }

        private void ShowNewLogger()
        {
            Form logForm;
            logForm = new Form();
            ParentStudio parent = (this.Parent as Form).ParentForm as ParentStudio;
            logForm.MdiParent = parent;
            logForm.Text = "Log_" + parent.ChildFormNumber;
            logForm.Tag = parent.ChildFormNumber;
            Tag = parent.ChildFormNumber;
            ParentForm.Tag = Tag;

            parent.ChildFormNumber++;
            LogControl logControl = new LogControl();
            HandleConstruction(logForm, logControl);

            logForm.FormClosing += logForm_Closing;
            try
            {
                _ObserverManager.Add(logForm.Tag.ToString(), new ObserverPattern.Observer(Parent as Form, logForm));
                LayoutManager optionControl = Parent.Controls[0] as LayoutManager;
                optionControl.Subscribe();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private Form GetLogForm()
        {
            Form logForm;
            ParentStudio parent = (this.Parent as Form).ParentForm as ParentStudio;
            var form = parent.MdiChildren.Where(child => child.Tag!= null && child.Tag.ToString().Equals(Tag.ToString())   
                                                         && child.Text.StartsWith("Log_"));
            logForm = form.FirstOrDefault();
            return logForm;
        }


        private void logForm_Closing(object sender, FormClosingEventArgs e)
        {

            try
            {
                ParentStudio parent = (this.Parent as Form).ParentForm as ParentStudio;
                var form1 = parent.MdiChildren.Where(child => child.Tag != null && child.Tag.ToString().Equals(Tag.ToString())
                                && child.Text.StartsWith("Option"));
                Form optionsForm = form1.FirstOrDefault();
                LayoutManager container = optionsForm.Controls[0] as LayoutManager;
                container.checkBox1.Checked = false;
                LayoutManager layoutManager = optionsForm.Controls[0] as LayoutManager;
                layoutManager.Unsubscribe();

            }
            catch (Exception)
            {

            }


            //MessageBox.Show("Disposing Logger...");

        }
        /// <summary>
        /// Adds control to the form.
        /// </summary>
        /// <param name="childForm"></param>
        /// <param name="control"></param>
        private void HandleConstruction(Form childForm, Control control)
        {
            control.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            control.Width = childForm.Width;
            control.Height = childForm.Height - 50;
            childForm.Controls.Add(control);
            childForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblStatusMessage.Text = "Busy...";
            button1.Enabled = false;
            Result = InitData.UniqueInstance;
            backgroundWorker = new BackgroundWorker();
            ///CleanLogger();
            TryToAttachToLogger();

            backgroundWorker.DoWork += backgroundWorker_Berekening;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            if (Tag == null) Tag = (ParentForm.MdiParent as ParentStudio).ChildFormNumber;

            BackgroundWorker bWorker;
            m_Qprocesses.TryGetValue(Tag.ToString(), out bWorker);

            if (bWorker == null) m_Qprocesses.Add(Tag.ToString(), backgroundWorker);

            backgroundWorker.RunWorkerAsync("Message to worker: " + Tag);

            Console.WriteLine("~thread2 started...");

            Console.WriteLine("~~~~concurrent thread1 ~~~~");

            Thread.Sleep(1000);
            //thread2.Suspend();
        }

        private void TryToAttachToLogger()
        {
            Form logForm = GetLogForm();

            RetPokEngineTest worker = null;
            try
            {
                m_Qworkers.TryGetValue(Tag.ToString(), out worker);
                if(worker==null)
                {
                    ///MessageBox.Show("NULL-Worker");
                    ObserverPattern.Observer observer;
                    _ObserverManager.TryGetValue(logForm.Tag.ToString(), out observer);
                    LogControl logDevice = observer.LogApplication.Controls[0] as LogControl;
                    //logDevice.Message.Se
                    logDevice.Message.Text = "";
                    logDevice.ProgressBarLogger.Value = 0;
                    observer.ProgressbarValue = 0;
                    Subscribe();
                }   

            }
            catch (Exception)
            {
                
            }
        }

        private void CleanLoggerAndCheckNewLogger()
        {
            Form logForm = GetLogForm();

            RetPokEngineTest worker = null;
            try
            {
                m_Qworkers.TryGetValue(Tag.ToString(), out worker);
            }
            catch (Exception)
            {
            }
            if (logForm != null && worker != null && worker.Message != null)///first time
            {
                MessageBox.Show("1");
                logForm.Close();
                logForm = null;
                ///ShowNewLogger();      
                checkBox1.Checked = true;          
            }
            else
                if(logForm != null && worker == null )///Second time
                {
                    MessageBox.Show("2");
                    logForm.Close();
                    logForm = null;
                    ///ShowNewLogger();
                    checkBox1.Checked = true;
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
                lblStatusMessage.Text = message;
                button1.Enabled = true;
                                
                BackgroundWorker bworker;
                m_Qprocesses.TryGetValue(Tag.ToString(), out bworker);
                if (bworker != null)
                {
                    bworker.DoWork -= backgroundWorker_Berekening;                    
                    m_Qprocesses.Remove(Tag.ToString());
                }
                RetPokEngineTest worker;
                m_Qworkers.TryGetValue(Tag.ToString(), out worker);

                if (worker != null)
                {
                    worker.Unsubscribe();
                    worker.Cancel = false;
                    m_Qworkers.Remove(worker.LogId);
                }
                Thread.Sleep(1000);
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                button1.Enabled = true;
            }
            //Unsubscribe();
        }

        void backgroundWorker_Berekening(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (NotifyLogger!=null) NotifyLogger("Message\t" + e.Argument);

                RetPokEngineTest worker;
                m_Qworkers.TryGetValue(Tag.ToString(), out worker);
                if (worker == null)
                {
                    worker = new RetPokEngineTest();

                } 

                worker.Subscribe();
                worker.Cancel = false;
                worker.DoWork();
                message = worker.Message;
                
            }
            catch (ThreadInterruptedException)
            {
                message = "interrupted!";
                Console.WriteLine("~~~~ thread2 interrupted...");
            }                
            catch (Exception ex)
            {
                message = "interrupted!!" + ex.Message;
                Console.Out.Write(ex);
                Console.WriteLine("~~~~ thread2 error...");
            }
            finally
            {
                Console.WriteLine("~~~~ thread2 initData2 ends ...~~~~");
                //button1.Enabled = true;
            }
            
        }
    }


}
