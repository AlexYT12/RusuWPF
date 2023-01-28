using Rusu.Views;
using System;

namespace Rusu.ViewModels;

public sealed class MessageWindowViewModel : Core.ObservableObject
{
    // Фон
    private string _Background = @"White";
    public string Background
    {
        get { return _Background; }
        set { _Background = value; OnPropertyChanged(); }
    }

    internal MessageWindow? View { get; set; }
    public MessageWindowViewModel()
    {
        Command = new Core.RelayCommand(x =>
        {
            if (View is null) return;
            View.Hide();
            Action?.Invoke();
        });

    }

    /// <summary>
    /// Показать сообщение.
    /// </summary>
    /// <param name="text">Текст</param>
    /// <param name="button">Кнопка</param>
    /// <param name="action">Действие при нажатии на кнопку</param>
    /// <exception cref="Exception"></exception>
    internal void MessageBox(string text, string button = "Ок", Action? action = null)
    {
        Text = text;
        ButtonText = button;
        Action = action;
        if (View is null) throw new Exception("Window is null");
        View.Topmost = true;
        View.Show();
    }

    private string _Text = "Пусто";
    public string Text
    {
        get { return _Text; }
        set { _Text = value; OnPropertyChanged(); }
    }

    private string _ButtonText = "Ок";
    public string ButtonText
    {
        get { return _ButtonText; }
        set { _ButtonText = value; OnPropertyChanged(); }
    }
    internal Action? Action;

    public Core.RelayCommand Command { get; set; }
}
