using Rusu.Core;
using Rusu.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Rusu.Logic
{
    internal static class Parser
    {
        /// <summary>
        /// Шаблон дня.
        /// </summary>
        private readonly static Regex _dayRegex = new Regex("bold\">\\s+(.*?)\\s+\\((.*?)\\)\\s+</div.*?b>(.*?)</div>\\s+</div>", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Шаблон занятия.
        /// </summary>
        private readonly static Regex _lessonRegex = new Regex("([0-5])\\. (.*?)<.*?/>\\s+(.*?)<br/>\\s+(.*?)<", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Парсинг расписания.
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Список дней</returns>
        internal static async Task<List<Day>?> ScheduleAsync(DateTime date)
        {
            if (!Data.License) return null;

            // Получение данных.
            string raw;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage hrm = await client.PostAsync("https://schedule.ruc.su/",
                    new StringContent(
                        Data.ScheduleParserContent
                        + $"&date-search={date.Year}-{date.Month}-{date.Day}",
                        Encoding.UTF8, "application/x-www-form-urlencoded"));
                if (hrm.IsSuccessStatusCode) raw = await hrm.Content.ReadAsStringAsync();
                else return null;
            }

            // Парсинг.
            var matches = _dayRegex.Matches(raw);
            var schedule = new List<Day>();

            // Моделирование.
            foreach (Match match in matches)
            {
                // День.
                var day = new Day(DateTime.Parse(match.Groups[1].Value), match.Groups[2].Value);

                // Занятия.
                var lessonMatches = _lessonRegex.Matches(match.Groups[3].Value);
                foreach (Match lessonMatch in lessonMatches)
                {
                    var id = Convert.ToByte(lessonMatch.Groups[1].Value);
                    var name = lessonMatch.Groups[2].Value;
                    var lesson = day.Lessons.Find(x => x.Id == id && x.Name == name);
                    var position = lessonMatch.Groups[4].Value;
                    if (lesson is null)
                        day.Lessons.Add(new Lesson(

                            id,
                            name,
                            lessonMatch.Groups[3].Value,
                            position
                        ));
                    else
                        lesson.Position += Environment.NewLine + position;
                }

                schedule.Add(day);
            }
            return schedule;
        }

        /// <summary>
        /// Создать объект по шаблону: key=value.
        /// </summary>
        /// <param name="lines">Строки</param>
        /// <param name="c">Символ разделения ключа и значения</param>
        /// <returns>Объект</returns>
        internal static Dictionary<string, string?> ParseByLineAndChar(string[] lines, char c = '=')
        {
            Dictionary<string, string?> result = new Dictionary<string, string?>();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.Contains(c))
                {
                    var keyValue = line.Split(c);
                    for (int i = 2; i < keyValue.Length; i++) keyValue[1] += c + keyValue[i];
                    result.InC(keyValue[0], keyValue[1]);
                }
                else result.InC(line, null);
            }
            return result;
        }

        /// <summary>
        /// Преобразовать проект в текст по шаблону: key=value.
        /// </summary>
        /// <param name="Object">Объект</param>
        /// <param name="c">Символ разделения ключа и значения</param>
        /// <returns>Текст</returns>
        public static string SerializeByLineAndChar(Dictionary<string, string?> Object, char c = '=')
        {
            StringBuilder text = new StringBuilder();
            foreach (var kv in Object)
                if (kv.Value == null) text.AppendLine(kv.Key);
                else text.AppendLine(kv.Key + c + kv.Value);
            return text.ToString().Remove(text.Length - Environment.NewLine.Length);
        }
    }
}
