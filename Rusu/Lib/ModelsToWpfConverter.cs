using RucSu.Logic;
using RucSu.Models;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace Rusu.Lib
{
    public class ModelsToWpfConverter : IValueConverter
    {
        private const string _dayTemplatePath = "data/Templates/ProgramDayTemplate.txt";
        private const string? _lessonTemplatePath = "data/Templates/ProgramLessonTemplate.txt";

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Day day) return StringFormater.DayAsString(day,
                File.Exists(_dayTemplatePath) ? File.ReadAllText(_dayTemplatePath) : null
                , lessons: false);
            if (value is Lesson lesson) return StringFormater.LessonAsString(lesson,
                File.Exists(_lessonTemplatePath) ? File.ReadAllText(_lessonTemplatePath) : null);
            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
