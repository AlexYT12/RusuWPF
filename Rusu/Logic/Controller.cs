using RucSu.Models;
using Rusu.Core;
using Rusu.Lib;
using Rusu.Models;
using Rusu.ViewModels;
using Rusu.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Rusu.Logic;

internal sealed class Controller
{
    /// <summary>
    /// Настройки загруженные из файла data/data.txt
    /// </summary>
    internal Dictionary<string, string?>? DataSettings { get; set; }

    /// <summary>
    /// Задний фон по умолчанию.
    /// </summary>
    string Background = "white";

    #region Окна
    internal MainWindowViewModel MainWindow { get; set; }
    internal ScheduleWindow? ScheduleWindow { get; set; }
    internal LessonCounterWindow? LessonCounterWindow { get; set; }
    internal TeacherSniperWindow? TeacherSniperWindow { get; set; }
    #endregion

    /// <summary>
    /// Сегодня
    /// </summary>
    internal Day? Today;


    internal Controller(MainWindow mainWindow)
    {
        // Создание окон.
        MainWindow = (MainWindowViewModel)mainWindow.DataContext;

        // Команды.
        MainWindow.ScheduleButton = new RelayCommand(x =>
        {
            if (ScheduleWindow is null)
            {
                ScheduleWindow = new ScheduleWindow();
                if (ScheduleWindow.DataContext is ScheduleWindowViewModel vm)
                    vm.Background = DataSettings.GoN("background-schedule") ?? Background;
            }
            ScheduleWindow.Show();
        });
        MainWindow.TeacherSniperButton = new RelayCommand(x =>
        {
            if (TeacherSniperWindow is null)
            {
                TeacherSniperWindow = new TeacherSniperWindow();
                if (TeacherSniperWindow.DataContext is TeacherSniperWindowViewModel vm)
                    vm.Background = DataSettings.GoN("background-teacher-sniper") ?? Background;
            }
            TeacherSniperWindow.Show();
        });
        MainWindow.LessonCounterButton = new RelayCommand(x =>
        {
            if (LessonCounterWindow is null)
            {
                LessonCounterWindow = new LessonCounterWindow();
                if (LessonCounterWindow.DataContext is LessonCounterWindowViewModel vm)
                    vm.Background = DataSettings.GoN("background-lesson-counter") ?? Background;
            }
            LessonCounterWindow.Show();
        });
    }

    internal async void RunAsync()
    {
        // Переменные.
        var today = DateTime.Today;

        // Расписание
        var Schedule = await Parser.SearchScheduleAsync(today);
        if (Schedule is null) return;
        var NextWeek = await Parser.SearchScheduleAsync(today.AddDays(7));
        if (NextWeek != null) Schedule.AddRange(NextWeek);
        Today = Schedule.Find(x => x.Date == today);

        // Нужно ли отслеживать изменения расписания?
        if (DataSettings != null && DataSettings.ContainsKey("save"))
        {
            // Существует ли сохраннённое расписание.
            if (File.Exists("data/save.txt"))
            {
                var days = JsonSerializer.Deserialize<List<Day>>(File.ReadAllText("data/save.txt"));
                if (days != null)
                {
                    StringBuilder sb = new StringBuilder();

                    // Проверка каждого дня из сохранных дней.
                    foreach (Day day in days)
                    {
                        if (day.Date < DateTime.Today) continue; // Если день уже прошёл - пропустить его.

                        var newDay = Schedule.Find(x => x.Date == day.Date); // Найти день с такой же датой в расписании с сайта.
                        if (newDay == null) continue; // Если день не найден - пропустить его.

                        if (newDay.Lessons.Count != day.Lessons.Count) // Если количество занятий изменилось.
                            sb.AppendLine("Изменения на " + day.ShortDate);
                        else
                            for (int i = 0; i < day.Lessons.Count; i++)
                            {
                                Lesson update = newDay.Lessons[i]; // Занятие с сайта
                                Lesson was = day.Lessons[i]; // Занятие с файла

                                if (update.Id == was.Id
                                 && update.Name == was.Name
                                 && update.Position == was.Position
                                 && update.Teacher == was.Teacher) continue; // Если изменений нет, пропустить.

                                sb.AppendLine("Изменения на " + day.ShortDate);
                                break;
                            }
                    }

                    // Вывод сообщения об изменениях.
                    string message = sb.ToString();
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        MainWindow.Text = message;
                    }
                }
            }

            // Обновить расписание в файле.
            File.WriteAllText("data/save.txt", JsonSerializer.Serialize(Schedule));
        }

        // Ближайшие дни.
        int Adder = 0;
        while (MainWindow.FirstDay == null && Adder < 15)
        {
            if (Today != null) MainWindow.FirstDay = Today;
            MainWindow.FirstDay = Schedule.Find(x => x.Date == today.AddDays(Adder));
            Adder++;
        }
        while (MainWindow.SecondDay == null && Adder < 15)
        {
            MainWindow.SecondDay = Schedule.Find(x => x.Date == today.AddDays(Adder));
            Adder++;
        }

        if (MainWindow.FirstDay != null)
            MainWindow.SelectedDay = MainWindow.FirstDay;

        Schedule = null;
    }

    internal void LoadData()
    {
        // Загрузка данных.
        DataSettings = Data.Load();

        string? value = DataSettings.Pop("background");

        if (value != null)
            Background = value;
        else if (File.Exists("assets/Background.jpg"))
            Background = "assets/Background.jpg";
        else if (File.Exists("assets/Background.png"))
            Background = "assets/Background.png";

        #region Фон
        MainWindow.Background = DataSettings.Pop("background-main") ?? Background;
        {
            if (ScheduleWindow?.DataContext is ScheduleWindowViewModel vm)
                vm.Background = DataSettings.GoN("background-schedule") ?? Background;
        }
        {
            if (TeacherSniperWindow?.DataContext is TeacherSniperWindowViewModel vm)
                vm.Background = DataSettings.GoN("background-teacher-sniper") ?? Background;
        }
        {
            if (LessonCounterWindow?.DataContext is LessonCounterWindowViewModel vm)
                vm.Background = DataSettings.GoN("background-lesson-counter") ?? Background;
        }
        #endregion

        if (DataSettings is null) return;
    }
}
