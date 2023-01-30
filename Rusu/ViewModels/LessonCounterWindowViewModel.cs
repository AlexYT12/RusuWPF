using RucSu.Models;
using Rusu.Core;
using Rusu.Logic;
using Rusu.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rusu.ViewModels;

public sealed class LessonCounterWindowViewModel : ObservableObject
{
    // Фон
    private string _Background = "White";
    public string Background
    {
        get { return _Background; }
        set { _Background = value; OnPropertyChanged(); }
    }

    // Отмена загрузки
    private CancellationTokenSource? _DownloadWorking;

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

    // Выбор предмета
    public RelayCommand ItemClickCommand { get; set; }

    // Список предметов
    public ObservableCollection<LessonCounterModel> Items { get; set; } = new ObservableCollection<LessonCounterModel>();

    // Список занятий
    private List<string>? _ItemDays;
    public List<string>? ItemDays
    {
        get { return _ItemDays; }
        set { _ItemDays = value; OnPropertyChanged(); }
    }

    // Лекций
    private int _LessonsCount;
    public int LessonsCount
    {
        get { return _LessonsCount; }
        set { _LessonsCount = value; OnPropertyChanged(); }
    }

    // Дней
    private int _DaysCount;
    public int DaysCount
    {
        get { return _DaysCount; }
        set { _DaysCount = value; OnPropertyChanged(); }
    }

    private async void UpdateAsync()
    {
        if (_DownloadWorking != null)
        {
            _DownloadWorking.Cancel();
            await Task.Delay(1000);
        }

        using (_DownloadWorking = new CancellationTokenSource())
        {
            // Подготовка
            Items.Clear();

            LessonsCount = 0;
            DaysCount = 0;

            List<Day> days = new();

            if (FirstDate > SecondDate) (FirstDate, SecondDate) = (SecondDate, FirstDate);
            DaysCount = (int)(SecondDate - FirstDate).TotalDays + 1;

            // Получения расписаний
            DateTime before = SecondDate.AddDays(7);
            for (DateTime date = FirstDate; date <= before; date = date.AddDays(7))
            {
                List<Day>? week = await Parser.SearchScheduleAsync(date);
                if (week != null) days.AddRange(week);
            }

            // Конвертирование занятий в строки.
            var items = new Dictionary<string, List<string>>();
            foreach (Day day in days)
                if (day.Date > SecondDate) break;
                else if (day.Date < FirstDate) continue;
                else foreach (Lesson lesson in day.Lessons)
                    {
                        if (_DownloadWorking?.Token.IsCancellationRequested ?? true) return;
                        LessonsCount++;
                        if (!items.ContainsKey(lesson.Name)) items.Add(lesson.Name, new List<string>());

                        items[lesson.Name].Add($"{lesson.Id}.  {day.Date.ToShortDateString()}, {lesson.PositionEdited}");
                    }

            // Моделирование
            foreach (KeyValuePair<string, List<string>> kv in items)
            {
                if (_DownloadWorking.Token.IsCancellationRequested) return;
                int lectures = kv.Value.Where(x => x.Contains("лекции")).Count();
                Items.Add(new LessonCounterModel
                {
                    Text = $"{kv.Key}: {lectures}/{kv.Value.Count}",
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
