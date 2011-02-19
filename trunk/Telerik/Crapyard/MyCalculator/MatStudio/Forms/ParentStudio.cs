using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using MatStudio.DesignPatterns;

namespace MatStudio.Forms
{
    public partial class ParentStudio : Form
    {
        ///protected Form optionForm;
        private int m_ChildFormNumber;
        
        public event LoggingBase.CallbackNotifyer NotifyLogger;//Logger

        private IDictionary<string,ObserverPattern.Observer> _ObserverManager = new Dictionary<string,ObserverPattern.Observer>();
        ///private Form _OptionForm;

        public ParentStudio()
        {
            InitializeComponent();
        }

        public int ChildFormNumber
        {
            get { return m_ChildFormNumber; }
            set { m_ChildFormNumber = value; }
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Grafiek " + m_ChildFormNumber++;
            Layout graphLayout = new Layout();
            HandleConstruction(childForm, graphLayout);

            foreach (var optionForm in MdiChildren)
            {
                if (optionForm.Text.StartsWith("Option"))
                {
                    _ObserverManager.Add(optionForm.Text, new ObserverPattern.Observer(optionForm, childForm));
                }
            }
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                #pragma warning disable 168
                string fileName = openFileDialog.FileName;
                #pragma warning restore 168
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                #pragma warning disable 168
                string fileName = saveFileDialog.FileName;
                #pragma warning restore 168
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (true)
            {
                Form optionForm;  optionForm = new Form();
                optionForm.MdiParent = this;
                optionForm.Text = "Option";
                m_ChildFormNumber++;
                LayoutManager layoutManager = new LayoutManager();
                HandleConstruction(optionForm, layoutManager);

                optionForm.FormClosing += optionForm_Closing;
                foreach (var childForm in MdiChildren)
                {
                    if (childForm.Text.StartsWith("Grafiek"))
                    {
                       new ObserverPattern.Observer(optionForm, childForm);
                    }
                }
            }
        }

        private void optionForm_Closing(object sender, EventArgs e)
        {
            LayoutManager subject = (sender as Form).Controls[0] as LayoutManager;
            subject.BackgroundWorker.Dispose();
            ///MessageBox.Show("Disposing...");
            //optionForm = null;
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

        private void buttonLogging_Click(object sender, EventArgs e)
        {
            {
                Form logForm = new Form();
                logForm.MdiParent = this;
                logForm.Text = "Log_" + m_ChildFormNumber;
                m_ChildFormNumber++;
                LogControl logControl = new LogControl();
                HandleConstruction(logForm, logControl);

                logForm.FormClosing += logForm_Closing;
                try
                {
                    foreach (var optionForm in MdiChildren)
                    {
                        if (optionForm.Text.StartsWith("Option"))
                        {

                            _ObserverManager.Add(logForm.Text, new ObserverPattern.Observer(optionForm, logForm));
                            LayoutManager optionControl = optionForm.Controls[0] as LayoutManager;
                            optionControl.Subscribe();

                        }
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }



        private void logForm_Closing(object sender, FormClosingEventArgs e)
        {

            try
            {
                foreach (var manager in _ObserverManager)
                {
                    if (manager.Key.Contains("Log_"))
                    {
                        ObserverPattern.Observer observer = manager.Value;
                        LayoutManager optionForm = observer.SubjectManager.Controls[0] as LayoutManager;
                        optionForm.Unsubscribe();

                        ///observer.UnsubscribeLogger();                    
                    }
                }
            }
            catch (Exception)
            {

            }


            //MessageBox.Show("Disposing Logger...");

        }

    }
}