using Rusu.Core;
using Rusu.Logic;
using System.ComponentModel;
using System.Windows;

namespace Rusu.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller _controller;
        public MainWindow()
        {
            InitializeComponent();

            _controller = new Controller(this);

            _controller.MainWindow.ExitButton = new RelayCommand(x => Close());

            // Запуск
            _controller.LoadData();
            _controller.RunAsync();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _controller.Exit();
            base.OnClosing(e);
        }
    }
}
