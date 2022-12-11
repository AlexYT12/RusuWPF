using System;
using System.Windows;
using System.Windows.Media;

namespace Rusu.ViewModels
{
    public sealed class MainWindowViewModel : Core.ObservableObject
    {
        internal Action? ActionRun { get; set; }
        private string _MainText = "Привет";
        public string MainText
        {
            get { return _MainText; }
            set { _MainText = value; OnPropertyChanged(); }
        }

        private string _SecondText = "Загрузка...";
        public string SecondText
        {
            get { return _SecondText; }
            set { _SecondText = value; OnPropertyChanged(); }
        }

        private string _OptionalText = "";
        public string OptionalText
        {
            get { return _OptionalText; }
            set { _OptionalText = value; OnPropertyChanged(); }
        }

        private string _Background = @"White";
        public string Background
        {
            get { return _Background; }
            set { _Background = value; OnPropertyChanged(); }
        }

        private Core.RelayCommand? _HomeCommand;
        public Core.RelayCommand? HomeCommand
        {
            get { return _HomeCommand; }
            set { _HomeCommand = value; OnPropertyChanged(); }
        }

        private string _Media = "";
        public string Media
        {
            get { return _Media; }
            set { _Media = value; OnPropertyChanged(); }
        }

        private float _MediaVolume = 1F;
        public float MediaVolume
        {
            get { return _MediaVolume; }
            set { _MediaVolume = value; OnPropertyChanged(); }
        }

        private VerticalAlignment _MediaVerticalAlignment = VerticalAlignment.Stretch;
        public VerticalAlignment MediaVerticalAlignment
        {
            get { return _MediaVerticalAlignment; }
            set { _MediaVerticalAlignment = value; OnPropertyChanged(); }
        }

        private HorizontalAlignment _MediaHorizontalAlignment = HorizontalAlignment.Stretch;
        public HorizontalAlignment MediaHorizontalAlignment
        {
            get { return _MediaHorizontalAlignment; }
            set { _MediaHorizontalAlignment = value; OnPropertyChanged(); }
        }

        private Stretch _MediaStretch = Stretch.Uniform;
        public Stretch MediaStretch
        {
            get { return _MediaStretch; }
            set { _MediaStretch = value; OnPropertyChanged(); }
        }
    }
}
