using Rusu.Core;
using Rusu.Logic;
using Rusu.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rusu.ViewModels
{
    public sealed class LessonCounterWindowViewModel : ObservableObject
    {
        // Задний фон
        private string _Background = "White";
        private CancellationTokenSource? _DownloadWorking;
        public string Background
        {
            get { return _Background; }
            set { _Background = value; OnPropertyChanged(); }
        }

        // Даты
        private DateTime _FirstDate = DateTime.Today;
        public DateTime FirstDate
        {
            get { return _FirstDate; }
            set { _FirstDate = value; OnPropertyChanged(); UpdateAsync(); }
        }

        private DateTime _SecondDate = DateTime.Today;
        public DateTime SecondDate
        {
            get { return _SecondDate; }
            set { _SecondDate = value; OnPropertyChanged(); UpdateAsync(); }
        }

        public RelayCommand ItemClickCommand { get; set; }

        public ObservableCollection<LessonCounterModel> Items { get; set; } = new ObservableCollection<LessonCounterModel>();
        private List<string>? _ItemDays;
        public List<string>? ItemDays
        {
            get { return _ItemDays; }
            set { _ItemDays = value; OnPropertyChanged(); }
        }

        // Анализ
        private int _LessonsCount;
        public int LessonsCount
        {
            get { return _LessonsCount; }
            set { _LessonsCount = value; OnPropertyChanged(); }
        }

        private int _DaysCount;
        public int DaysCount
        {
            get { return _DaysCount; }
            set { _DaysCount = value; OnPropertyChanged(); }
        }
        private int _OnlyOnlineDaysCount;
        public int OnlyOnlineDaysCount
        {
            get { return _OnlyOnlineDaysCount; }
            set { _OnlyOnlineDaysCount = value; OnPropertyChanged(); }
        }

        private int _PracticeDaysCount;
        public int PracticeDaysCount
        {
            get { return _PracticeDaysCount; }
            set { _PracticeDaysCount = value; OnPropertyChanged(); }
        }

        private async void UpdateAsync()
        {
            if (_DownloadWorking != null)
            {
                _DownloadWorking.Cancel();
                await Task.Delay(1000);
            }

            // Подготовка
            using (_DownloadWorking = new CancellationTokenSource())
            {
                Items.Clear();

                LessonsCount = 0;
                DaysCount = 0;
                OnlyOnlineDaysCount = 0;
                PracticeDaysCount = 0;

                List<Day> days = new List<Day>();

                if (FirstDate > SecondDate)
                {
                    var timed = FirstDate;
                    FirstDate = SecondDate;
                    SecondDate = timed;
                }
                DaysCount = (int)(SecondDate - FirstDate).TotalDays + 1;

                // Получения расписаний
                for (DateTime date = FirstDate; date <= SecondDate; date = date.AddDays(7))
                {
                    var week = await Parser.ScheduleAsync(date);
                    if (week != null) days.AddRange(week);
                }

                var items = new Dictionary<string, List<string>>();
                foreach (Day day in days)
                    if (day.Date > SecondDate) break;
                    else if (day.Date < FirstDate) continue;
                    else
                    {
                        int online = 0;
                        foreach (Lesson lesson in day.Lessons)
                        {
                            if (_DownloadWorking?.Token.IsCancellationRequested ?? true) return;
                            LessonsCount++;
                            if (!items.ContainsKey(lesson.Name)) items.Add(lesson.Name, new List<string>());

                            var text = $"{lesson.Id}.  {day.Date.ToShortDateString()}, {lesson.PositionEdited}";

                            items[lesson.Name].Add(text);
                        }
                        if (online == day.Lessons.Count) OnlyOnlineDaysCount++;
                        else PracticeDaysCount++;
                    }
                foreach (var kv in items)
                {
                    if (_DownloadWorking.Token.IsCancellationRequested) return;
                    var online = kv.Value.Where(x => x.Contains("Онлайн")).Count();
                    Items.Add(new LessonCounterModel
                    {
                        Text = $"{kv.Key}: {online}/{kv.Value.Count}",
                        Online = online,
                        Days = kv.Value
                    });
                }
            }
            _DownloadWorking = null;
        }

        public LessonCounterWindowViewModel()
        {
            ItemClickCommand = new RelayCommand(x =>
            {
                if (x is LessonCounterModel lcm)
                {
                    ItemDays = lcm.Days;
                }
            });
            UpdateAsync();
        }
    }
}
