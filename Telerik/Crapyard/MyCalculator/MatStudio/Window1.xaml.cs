using System.Windows;
using MatStudio.Forms;

namespace MatStudio
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class Window1 : Window
// ReSharper restore RedundantExtendsListEntry
    {
        public Window1()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
            ParentStudio form = new ParentStudio();
            form.ShowDialog();
            Close();
        }
    }
}
