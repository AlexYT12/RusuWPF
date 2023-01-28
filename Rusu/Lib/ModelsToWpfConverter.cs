using RucSu.Logic;
using RucSu.Models;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace Rusu.Lib;

public class ModelsToWpfConverter : IValueConverter
{
    // Пути к шаблонам.
    private const string _dayTemplatePath = "data/Templates/ProgramDayTemplate.txt";
    private const string _lessonTemplatePath = "data/Templates/ProgramLessonTemplate.txt";

    // Конвертировать объект для GUI.
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // День конвертировать в строку по шаблону.
        if (value is Day day) return StringFormater.DayAsString(day,
            File.Exists(_dayTemplatePath) ? File.ReadAllText(_dayTemplatePath) : null
           , lessons: false);

        // Занятие конвертировать в строку по шаблону.
        if (value is Lesson lesson) return StringFormater.LessonAsString(lesson,
            File.Exists(_lessonTemplatePath) ? File.ReadAllText(_lessonTemplatePath) : null);

        // Вернуть значение таким, каким пришло.
        return value;
    }
    
    // В обратной конвертации нет нужды.
    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Сообщает, что свойство не имеет значений.
        return DependencyProperty.UnsetValue;
    }
}
