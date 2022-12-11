using System.ComponentModel;
using System.Windows;

namespace Rusu.Views
{
    /// <summary>
    /// Логика взаимодействия для ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        internal bool CantClose = true;

        public ScheduleWindow()
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
