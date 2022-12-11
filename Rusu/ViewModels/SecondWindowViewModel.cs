using Rusu.Core;
using Rusu.Logic;
using Rusu.Models;
using Rusu.Views;
using System.Threading.Tasks;
using System.Windows;

namespace Rusu.ViewModels
{
    public sealed class SecondWindowViewModel : ObservableObject
    {
        public SecondWindow? View { get; set; }

        public Day? FirstDay { get; set; }
        public Day? SecondDay { get; set; }

        private Day? _SelectedDay;
        public Day? SelectedDay
        {
            get { return _SelectedDay; }
            set
            {
                _SelectedDay = value;
                OnPropertyChanged();
                OnPropertyChanged("ScheduleCurrentDateName");
            }
        }

        private string? _Text;
        public string? Text
        {
            get { return _Text; }
            set { _Text = value; OnPropertyChanged(); }
        }

        private string _VersionText = Data.Version;
        public string VersionText
        {
            get { return _VersionText; }
            set { _VersionText = value; OnPropertyChanged(); }
        }

        public string ScheduleCurrentDateName
        {
            get { return $"Расписание на {StringFormater.ShortDateName(SelectedDay?.Date) ?? SelectedDay?.DayOfWeek}"; }
        }

        public SecondWindowViewModel()
        {
            // Команды.
            ChangeButton = new RelayCommand(x =>
            {
                SelectedDay = SelectedDay == FirstDay ? SecondDay : FirstDay;
            });
            CopyButton = new RelayCommand(x =>
            {
                if (SelectedDay is null) return;
                string text = StringFormater.DayAsString(SelectedDay, Data.DayTemplatePath, SelectedDay.ShortDate);
                Clipboard.SetText(text);
            });
            VersionButton = new RelayCommand(async x =>
            {
                string? saved = null;
                if (VersionText == Data.Version)
                    saved = Data.UpdateDescription;
                if (saved is null) return;
                VersionText = saved;
                await Task.Delay(5000);
                if (VersionText == saved) VersionText = Data.Version;
            });
        }

        private string _Background = @"White";
        public string Background
        {
            get { return _Background; }
            set { _Background = value; OnPropertyChanged(); }
        }

        private RelayCommand? _ScheduleButton;
        public RelayCommand? ScheduleButton
        {
            get { return _ScheduleButton; }
            set { _ScheduleButton = value; OnPropertyChanged(); }
        }

        public RelayCommand ChangeButton { get; set; }

        public RelayCommand CopyButton { get; set; }

        private RelayCommand? _MinimizeButton;
        public RelayCommand? MinimizeButton
        {
            get { return _MinimizeButton; }
            set { _MinimizeButton = value; OnPropertyChanged(); }
        }

        private RelayCommand? _TopMostButton;
        public RelayCommand? TopMostButton
        {
            get { return _TopMostButton; }
            set { _TopMostButton = value; OnPropertyChanged(); }
        }

        private RelayCommand? _ExitButton;
        public RelayCommand? ExitButton
        {
            get { return _ExitButton; }
            set { _ExitButton = value; OnPropertyChanged(); }
        }

        public RelayCommand VersionButton { get; set; }

        private RelayCommand? _TemplateEditorButton;
        public RelayCommand? TemplateEditorButton
        {
            get { return _TemplateEditorButton; }
            set { _TemplateEditorButton = value; OnPropertyChanged(); }
        }

        private RelayCommand? _LessonCounterButton;
        public RelayCommand? LessonCounterButton
        {
            get { return _LessonCounterButton; }
            set { _LessonCounterButton = value; OnPropertyChanged(); }
        }


        public void Show()
        {
            View?.Show();
        }
    }
}
