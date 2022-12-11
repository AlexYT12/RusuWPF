using Rusu.Core;
using Rusu.Models;
using System;

namespace Rusu.Logic
{
    public static class StringFormater
    {
        /// <summary>
        /// Дата вида "Завтра".
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Дата в строке</returns>
        public static string? ShortDateName(DateTime? date)
        {
            var today = DateTime.Today;
            if (date is null) return null;
            if (date == today) return "Сегодня";
            if (date == today.AddDays(1)) return "Завтра";
            if (date == today.AddDays(2)) return "Послезавтра";
            return null;
        }

        /// <summary>
        /// Занятие в строку.
        /// </summary>
        /// <param name="lesson">Занятие</param>
        /// <returns>Результат</returns>
        public static string LessonAsString(Lesson lesson, string path)
        {
            // Чтение шаблона.
            var text = Helper.ReadText(path);
            if (text == null) return lesson.ToString();

            // Проверка на онлайн занятие.
            if (!lesson.IsOnline) text = TemplateModel.RemoveByLabels(text, "<s$Url>", "<e$Url>");

            return lesson.GetByTemplate(text);
        }

        /// <summary>
        /// День в строку.
        /// </summary>
        /// <param name="day">День</param>
        /// <param name="date">Дата</param>
        /// <returns>Результат</returns>
        public static string DayAsString(Day day, string path, string? date = null, bool lessons = true)
        {
            // Чтение шаблона.
            var text = Helper.ReadText(path);
            if (text is null) text = date ?? day.ToString();
            else
            {
                if (date != null)
                    text = text = text.Replace("<$ShortDate>", date)
                                      .Replace("<$WeekDate>", date);
                text = day.GetByTemplate(text); // Замена.
            }
            if (lessons)
                foreach (Lesson lesson in day.Lessons)
                    text += Environment.NewLine + lesson.ByTemplate;
            return text;
        }
    }
}
