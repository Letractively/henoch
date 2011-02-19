using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatStudio.Forms
{
    public partial class LogControl : UserControl
    {
        public LogControl()
        {
            InitializeComponent();
        }

        public RichTextBox Message
        {
            get
            {
                return richTextBox1;   
            }
             set
             {
                 richTextBox1 = value;
             }
        }
        public ProgressBar ProgressBarLogger
        {
            get
            {
                return progressBar1;
            }
            set
            {
                progressBar1 = value;
            }  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button1.Enabled = false;
        }
    }
    
}
