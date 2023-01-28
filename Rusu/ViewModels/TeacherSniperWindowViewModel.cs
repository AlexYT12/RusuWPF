using RucSu.Models;
using Rusu.Core;
using System;
using System.Collections.Generic;

namespace Rusu.ViewModels;

internal class TeacherSniperWindowViewModel : ObservableObject
{
    // Фон
    private string _Background = @"White";
    public string Background
    {
        get { return _Background; }
        set { _Background = value; OnPropertyChanged(); }
    }

    public TeacherSniperWindowViewModel()
    {
        LoadValues();

        NextButton = new RelayCommand(x => { if (x is string s) Date = Date.AddDays(s == "Add" ? 7 : -7); });
    }

    // Загрузка списка преподавателей.
    public async void LoadValues()
    {
        Dictionary<string, Dictionary<string, string>>? values = await RucSu.Logic.Parser.GetValues(true, "4935b3ff-0858-11e0-8be3-005056bd3ce5");
        if (values == null || !values.ContainsKey("employee")) return;
        Teachers = values["employee"];
    }

    // Загрузка расписания.
    private async void UpdateAsync()
    {
        if (Teachers != null && SelectedTeacher != null)
            Days = await Logic.Parser.SearchScheduleAsync(Date, Teachers[SelectedTeacher]);
    }

    #region Параметры
    private Dictionary<string, string>? _Teachers;

    public Dictionary<string, string>? Teachers
    {
        get { return _Teachers; }
        set { _Teachers = value; OnPropertyChanged(); }
    }

    private string? _SelectedTeacher;

    public string? SelectedTeacher
    {
        get { return _SelectedTeacher; }
        set { _SelectedTeacher = value; OnPropertyChanged(); UpdateAsync(); }
    }

    private DateTime _Date = DateTime.Today;
    public DateTime Date
    {
        get { return _Date; }
        set { _Date = value; OnPropertyChanged(); UpdateAsync(); }
    }

    private List<Day>? _Days;
    public List<Day>? Days
    {
        get { return _Days; }
        set { _Days = value; OnPropertyChanged(); }
    }

    public RelayCommand NextButton { get; set; }
    #endregion
}
