using Rusu.Core;

namespace Rusu.ViewModels
{
    internal sealed class AboutWindowViewModel : ObservableObject
    {
        private string _Background = @"White";
        public string Background
        {
            get { return _Background; }
            set { _Background = value; OnPropertyChanged(); }
        }

        private RelayCommand? _SendTestMessage;
        public RelayCommand? SendTestMessage
        {
            get { return _SendTestMessage; }
            set { _SendTestMessage = value; OnPropertyChanged(); }
        }

        private RelayCommand? _ReLoadDataCommand;
        public RelayCommand? ReLoadDataCommand
        {
            get { return _ReLoadDataCommand; }
            set { _ReLoadDataCommand = value; OnPropertyChanged(); }
        }

        private RelayCommand? _ReLoadLogicCommand;
        public RelayCommand? ReLoadLogicCommand
        {
            get { return _ReLoadLogicCommand; }
            set { _ReLoadLogicCommand = value; OnPropertyChanged(); }
        }

        private RelayCommand? _CleanCommand;
        public RelayCommand? CleanCommand
        {
            get { return _CleanCommand; }
            set { _CleanCommand = value; OnPropertyChanged(); }
        }
    }
}
