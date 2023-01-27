using RucSu.Models;
using Rusu.Core;
using Rusu.Models;
using Rusu.ViewModels;
using Rusu.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using RucSu.Logic;
using Rusu.Lib;

namespace Rusu.Logic
{
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

        // Окна
        internal MainWindowViewModel MainWindow { get; set; }
        internal SecondWindowViewModel SecondWindow { get; set; }
        internal MessageWindowViewModel MessageWindow { get; set; }
        internal ScheduleWindow? ScheduleWindow { get; set; }
        internal TemplateEditorWindow? TemplateEditorWindow { get; set; }
        internal AboutWindow? AboutWindow { get; set; }
        internal LessonCounterWindow? LessonCounterWindow { get; set; }

        /// <summary>
        /// Сегодня
        /// </summary>
        internal Day? Today;
        /// <summary>
        /// Текущее занятие.
        /// </summary>
        internal Lesson? CurrentLesson;


        internal Controller(MainWindowViewModel mainWindow)
        {
            // Создание окон.
            MainWindow = mainWindow;
            SecondWindow = (SecondWindowViewModel)new SecondWindow().DataContext;
            MessageWindow = (MessageWindowViewModel)new MessageWindow().DataContext;

            // Команды.
            mainWindow.ActionRun = () => SecondWindow.Show();
            mainWindow.HomeCommand = new RelayCommand(x =>
            {
                if (AboutWindow is null)
                {
                    AboutWindow = new AboutWindow();
                    if (AboutWindow.DataContext is AboutWindowViewModel vm)
                    {
                        vm.Background = DataSettings.GoN("background-about") ?? Background;
                        vm.ReLoadDataCommand = new RelayCommand(a => LoadData());
                        vm.ReLoadLogicCommand = new RelayCommand(a => RunAsync());
                        vm.CleanCommand = new RelayCommand(a =>
                        {
                            if (ScheduleWindow != null)
                            {
                                ScheduleWindow.CantClose = false;
                                ScheduleWindow.Close();
                                ScheduleWindow = null;
                            }
                            if (TemplateEditorWindow != null)
                            {
                                TemplateEditorWindow.CantClose = false;
                                TemplateEditorWindow.Close();
                                TemplateEditorWindow = null;
                            }
                            if (AboutWindow != null)
                            {
                                AboutWindow.CantClose = false;
                                AboutWindow.Close();
                                AboutWindow = null;
                            }
                            if (LessonCounterWindow != null)
                            {
                                LessonCounterWindow.CantClose = false;
                                LessonCounterWindow.Close();
                                LessonCounterWindow = null;
                            }
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        });
                        vm.SendTestMessage = new RelayCommand(
                            x => MessageWindow.MessageBox("Программа сделана специально для Студенческой банды.")
                        );
                    }
                }
                AboutWindow.Show();
            });

            SecondWindow.ScheduleButton = new RelayCommand(x =>
            {
                if (ScheduleWindow is null)
                {
                    ScheduleWindow = new ScheduleWindow();
                    if (ScheduleWindow.DataContext is ScheduleWindowViewModel vm)
                        vm.Background = DataSettings.GoN("background-schedule") ?? Background;
                }
                ScheduleWindow.Show();
            });
            SecondWindow.TemplateEditorButton = new RelayCommand(x =>
            {
                if (TemplateEditorWindow is null)
                {
                    TemplateEditorWindow = new TemplateEditorWindow();
                    if (TemplateEditorWindow.DataContext is TemplateEditorWindowViewModel vm)
                        vm.Background = DataSettings.GoN("background-template-editor") ?? Background;
                }
                TemplateEditorWindow.Show();
            });
            SecondWindow.LessonCounterButton = new RelayCommand(x =>
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
            MainWindow.SecondText = today.ToShortDateString();

            // Расписание
            var Schedule = await Parser.SearchScheduleAsync(today);
            if (Schedule is null) return;
            var NextWeek = await Parser.SearchScheduleAsync(today.AddDays(7));
            if (NextWeek != null) Schedule.AddRange(NextWeek);
            Today = Schedule.Find(x => x.Date == today);

            // Ближайшие дни.
            int Adder = 0;
            while (SecondWindow.FirstDay == null && Adder < 15)
            {
                if (Today != null) SecondWindow.FirstDay = Today;
                SecondWindow.FirstDay = Schedule.Find(x => x.Date == today.AddDays(Adder));
                Adder++;
            }
            while (SecondWindow.SecondDay == null && Adder < 15)
            {
                SecondWindow.SecondDay = Schedule.Find(x => x.Date == today.AddDays(Adder));
                Adder++;
            }

            if (SecondWindow.FirstDay != null)
                SecondWindow.SelectedDay = SecondWindow.FirstDay;

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

            MainWindow.OptionalText = DataSettings.Pop("attribute") ?? "";
            /* Фон */
            {
                MainWindow.Background = DataSettings.Pop("background-main") ?? Background;
                SecondWindow.Background = DataSettings.Pop("background-second") ?? Background;
                MessageWindow.Background = DataSettings.Pop("background-message") ?? Background;
                {
                    if (ScheduleWindow?.DataContext is ScheduleWindowViewModel vm)
                        vm.Background = DataSettings.GoN("background-schedule") ?? Background;
                }
                {
                    if (TemplateEditorWindow?.DataContext is TemplateEditorWindowViewModel vm)
                        vm.Background = DataSettings.GoN("background-template-editor") ?? Background;
                }
                {
                    if (LessonCounterWindow?.DataContext is LessonCounterWindowViewModel vm)
                        vm.Background = DataSettings.GoN("background-lesson-counter") ?? Background;
                }
                {
                    if (AboutWindow?.DataContext is AboutWindowViewModel vm)
                        vm.Background = DataSettings.GoN("background-about") ?? Background;
                }
            }

            if (DataSettings is null) return;

            /* Медиа */
            {
                value = DataSettings.Pop("media");
                if (value != null) MainWindow.Media = value;

                value = DataSettings.Pop("media-vertical-alignment");
                if (value != null)
                    MainWindow.MediaVerticalAlignment = (value) switch
                    {
                        "top" => VerticalAlignment.Top,
                        "center" => VerticalAlignment.Center,
                        "bottom" => VerticalAlignment.Bottom,
                        _ => VerticalAlignment.Stretch,
                    };

                value = DataSettings.Pop("media-horizontal-alignment");
                if (value != null)
                    MainWindow.MediaHorizontalAlignment = (value) switch
                    {
                        "left" => HorizontalAlignment.Left,
                        "center" => HorizontalAlignment.Center,
                        "right" => HorizontalAlignment.Right,
                        _ => HorizontalAlignment.Stretch,
                    };

                value = DataSettings.Pop("media-stretch");
                if (value != null)
                    MainWindow.MediaStretch = (value) switch
                    {
                        "none" => System.Windows.Media.Stretch.None,
                        "fill" => System.Windows.Media.Stretch.Fill,
                        "ufill" => System.Windows.Media.Stretch.UniformToFill,
                        _ => System.Windows.Media.Stretch.Uniform
                    };

                value = DataSettings.Pop("media-volume");
                if (value != null)
                {
                    float volume;
                    if (float.TryParse(value, out volume))
                        MainWindow.MediaVolume = volume;
                }
            }
        }
    }
}
