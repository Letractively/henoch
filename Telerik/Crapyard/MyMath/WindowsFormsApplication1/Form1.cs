using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            List<string> list = new List<string>
                                    {
                                        "tabel1.kol1", 
                                        "tabel1.kol2", 
                                        "tabel2.kol1b", 
                                        "tabel2.kol1b"
                                    };

            int i = 0;
            ILookup<string, string> lookup = list.ToLookup(
                p => p.Substring(0, 6),
                p => p.Substring(7, p.Length-7));

            var kols = lookup["tabel1"].ToList();
            comboBox1.DataSource = kols;
        }
    }
}
