using Rusu.Core;
using Rusu.Logic;
using Rusu.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Rusu.ViewModels
{
    public sealed class LessonCounterWindowViewModel : ObservableObject
    {
        // Задний фон
        private string _Background = "White";
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


        private int _DaysCount;
        public int DaysCount
        {
            get { return _DaysCount; }
            set { _DaysCount = value; OnPropertyChanged(); }
        }

        private async void UpdateAsync()
        {
            Items.Clear();
            DaysCount = 0;
            if (FirstDate > SecondDate)
            {
                var timed = FirstDate;
                FirstDate = SecondDate;
                SecondDate = timed;
            }
            List<Day> days = new List<Day>();
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
                    DaysCount++;
                    foreach (Lesson lesson in day.Lessons)
                    {
                        if (!items.ContainsKey(lesson.Name)) items.Add(lesson.Name, new List<string>());

                        var text = lesson.Id + ".  " + day.Date.ToShortDateString();

                        if (lesson.IsOnline) text += " онлайн";

                        items[lesson.Name].Add(text);
                    }
                }
            foreach (var kv in items)
                Items.Add(new LessonCounterModel
                {
                    Text = $"{kv.Key}: {kv.Value.Where(x => x.Contains("онлайн")).Count()}/{kv.Value.Count}",
                    Days = kv.Value
                });
        }

        public LessonCounterWindowViewModel()
        {
            ItemClickCommand = new RelayCommand(x => ItemDays = (List<string>?)x);
            UpdateAsync();
        }
    }
}
