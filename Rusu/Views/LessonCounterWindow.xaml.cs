using System.ComponentModel;
using System.Windows;

namespace Rusu.Views
{
    /// <summary>
    /// Логика взаимодействия для LessonCounterWindow.xaml
    /// </summary>
    public partial class LessonCounterWindow : Window
    {
        public LessonCounterWindow()
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
