using RucSu.Logic;
using RucSu.Models;
using Rusu.Core;
using Rusu.Logic;
using Rusu.Models;
using Rusu.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Rusu.ViewModels;

public sealed class ScheduleWindowViewModel : ObservableObject
{
    public ScheduleWindow? View { get; set; }

    private DateTime? _Date;
    public DateTime? Date
    {
        get { return _Date; }
        set { _Date = value; OnPropertyChanged(); DateUpdatedAsync(); }
    }

    private List<Day>? _Days;
    public List<Day>? Days
    {
        get { return _Days; }
        set { _Days = value; OnPropertyChanged(); }
    }

    public ScheduleWindowViewModel()
    {
        Date = DateTime.Today;
        DateUpdatedAsync();

        CopyButton = new RelayCommand(x =>
        {
            if (Days != null)
            {
                string text = "";
                foreach (Day day in Days)
                    text += StringFormater.DayAsString(day,
                        File.Exists(Data.DayTemplatePath) ? File.ReadAllText(Data.DayTemplatePath) : null,
                        date: day.ToString(),
                        lessonTemplate: File.Exists(Data.LessonTemplatePath) ? File.ReadAllText(Data.LessonTemplatePath) : null) + Environment.NewLine + Environment.NewLine;
                Clipboard.SetText(text.Remove(text.Length - Environment.NewLine.Length * 2));
            }
        });
        NextButton = new RelayCommand(x => { if (x is string s) Date = Date.Value.AddDays(s == "Add" ? 7 : -7); });
    }

    public void Show()
    {
        if (View is null) return;
        View.Show();
    }

    public async void DateUpdatedAsync()
    {
        if (Date.HasValue) Days = await Rusu.Logic.Parser.SearchScheduleAsync(Date.Value);
    }

    private string _Background = @"White";
    public string Background
    {
        get { return _Background; }
        set { _Background = value; OnPropertyChanged(); }
    }

    public RelayCommand CopyButton { get; set; }

    public RelayCommand NextButton { get; set; }
}
