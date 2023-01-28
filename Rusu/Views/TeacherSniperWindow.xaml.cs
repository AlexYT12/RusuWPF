using System.ComponentModel;
using System.Windows;

namespace Rusu.Views
{
    /// <summary>
    /// Логика взаимодействия для TeacherSniperWindow.xaml
    /// </summary>
    public partial class TeacherSniperWindow : Window
    {
        internal bool CantClose = true;

        public TeacherSniperWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = CantClose;
            Hide();
        }
    }
}
