using RucSu.Logic;
using RucSu.Models;
using Rusu.Core;
using Rusu.Models;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Rusu.ViewModels;

public sealed class MainWindowViewModel : ObservableObject
{
    // Фон
    private string _Background = @"White";
    public string Background
    {
        get { return _Background; }
        set { _Background = value; OnPropertyChanged(); }
    }

    public MainWindowViewModel()
    {
        // Команды.
        ChangeButton = new RelayCommand(x =>
        {
            SelectedDay = SelectedDay == FirstDay ? SecondDay : FirstDay;
        }); // Смена расписания

        CopyButton = new RelayCommand(x =>
        {
            if (SelectedDay is null) return;
            string text = StringFormater.DayAsString(SelectedDay, File.Exists(Data.DayTemplatePath) ? File.ReadAllText(Data.DayTemplatePath) : null,
                lessonTemplate: File.Exists(Data.LessonTemplatePath) ? File.ReadAllText(Data.LessonTemplatePath) : null);
            Clipboard.SetText(text);
        }); // Копировать расписание

        VersionButton = new RelayCommand(async x =>
        {
            string? saved = null;
            if (VersionText == Data.VersionText) return;
            saved = Data.VersionText;
            VersionText = saved;
            await Task.Delay(5000);
            if (VersionText == saved) VersionText = Data.VersionName;
        });
    }

    // Дни
    public Day? FirstDay { get; set; }
    public Day? SecondDay { get; set; }

    // Выбранный день
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

    // Сообщение
    private string? _Text;
    public string? Text
    {
        get { return _Text; }
        set { _Text = value; OnPropertyChanged(); }
    }

    // Текст версии
    private string _VersionText = Data.VersionName;
    public string VersionText
    {
        get { return _VersionText; }
        set { _VersionText = value; OnPropertyChanged(); }
    }

    // Текст дня расписания
    public string ScheduleCurrentDateName
    {
        get
        {
            if (SelectedDay == null) return "На ближайшее время расписания нет";
            return $"Расписание на {StringFormater.ShortDateName(SelectedDay?.Date) ?? SelectedDay?.DayOfWeek}";
        }
    }

    #region Команды
    public RelayCommand ChangeButton { get; set; }

    public RelayCommand CopyButton { get; set; }

    public RelayCommand VersionButton { get; set; }

    private RelayCommand? _ExitButton;
    public RelayCommand? ExitButton
    {
        get { return _ExitButton; }
        set { _ExitButton = value; OnPropertyChanged(); }
    }

    private RelayCommand? _ScheduleButton;
    public RelayCommand? ScheduleButton
    {
        get { return _ScheduleButton; }
        set { _ScheduleButton = value; OnPropertyChanged(); }
    }

    private RelayCommand? _LessonCounterButton;
    public RelayCommand? LessonCounterButton
    {
        get { return _LessonCounterButton; }
        set { _LessonCounterButton = value; OnPropertyChanged(); }
    }

    private RelayCommand? _TeacherSniperButton;
    public RelayCommand? TeacherSniperButton
    {
        get { return _TeacherSniperButton; }
        set { _TeacherSniperButton = value; OnPropertyChanged(); }
    }
    #endregion
}
