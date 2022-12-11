using Rusu.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Rusu.Views
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();
            if (DataContext is MessageWindowViewModel vm) vm.View = this;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
