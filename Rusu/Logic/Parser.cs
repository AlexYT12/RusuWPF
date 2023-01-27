using RucSu.Models;
using Rusu.Models;
using Rusu.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rusu.Logic
{
    internal static class Parser
    {
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
        /// <param name="_object">Объект</param>
        /// <param name="c">Символ разделения ключа и значения</param>
        /// <returns>Текст</returns>
        public static string SerializeByLineAndChar(Dictionary<string, string?> _object, char c = '=')
        {
            StringBuilder text = new StringBuilder();
            foreach (var kv in _object)
                if (kv.Value == null) text.AppendLine(kv.Key);
                else text.AppendLine(kv.Key + c + kv.Value);
            return text.ToString().Remove(text.Length - Environment.NewLine.Length);
        }

        internal static Task<List<Day>?> SearchScheduleAsync(DateTime today)
        {
            return RucSu.Logic.Parser.ScheduleAsync(Data.ScheduleParserContent + "&date-search=" + today.ToString("yyyy-MM-dd"));
        }
    }
}
