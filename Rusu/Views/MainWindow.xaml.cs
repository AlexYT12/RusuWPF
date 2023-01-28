using Rusu.Core;
using Rusu.Logic;
using Rusu.ViewModels;
using System;
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
            if (DataContext is MainWindowViewModel vm) vm.View = this;

            _controller = new Controller(this);

            _controller.MainWindow.ExitButton = new RelayCommand(x => Close());

            // Запуск
            _controller.LoadData();
            _controller.RunAsync();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
