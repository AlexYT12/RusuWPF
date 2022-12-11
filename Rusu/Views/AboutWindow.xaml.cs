using System.ComponentModel;
using System.Windows;

namespace Rusu.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        internal bool CantClose = true;
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = CantClose;
            Hide();
        }
    }
}
