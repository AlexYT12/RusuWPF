using Rusu.Core;
using Rusu.Logic;
using Rusu.Models;
using Rusu.ViewModels;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Rusu.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var controller = new Controller((MainWindowViewModel)DataContext);
            controller.SecondWindow.ExitButton = new RelayCommand(x => OnClosing(null));
            controller.SecondWindow.TopMostButton = new RelayCommand(x => Topmost = !Topmost);
            controller.SecondWindow.MinimizeButton = new RelayCommand(x =>
            {
                if (WindowState == WindowState.Minimized) WindowState = WindowState.Normal;
                else WindowState = WindowState.Minimized;
            });

            // Запуск
            controller.LoadData();
            controller.RunAsync();
        }

        private void WindowMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ActionRun(object sender, MouseButtonEventArgs e)
        {
            ((MainWindowViewModel)DataContext).ActionRun?.Invoke();
        }
        protected override void OnClosing(CancelEventArgs? e)
        {
            base.OnClosing(e);
            Application.Current.Shutdown();
        }
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (sender is MediaElement me)
            {
                me.Position = TimeSpan.FromMilliseconds(1);
                me.Play();
            }
        }
    }
}
