using RucSu.Models;
using Rusu.Lib;
using Rusu.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rusu.Logic;

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
            // Пропустить строчку если она пуста или является комментарием.
            if (string.IsNullOrWhiteSpace(line)
            || (line.Length > 1
             && line[0] == '/'
             && line[0] == '/')) continue;

            if (line.Contains(c)) // Есть ли значение у параметра?
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

    /// <summary>
    /// Получить расписание по дате.
    /// </summary>
    /// <param name="today">Дата</param>
    /// <param name="cancel">Токен отмены</param>
    /// <returns></returns>
    internal static Task<List<Day>?> SearchScheduleAsync(DateTime date, string? employee = null, CancellationToken? cancel = null)
    {
        if (string.IsNullOrWhiteSpace(employee))
            return RucSu.Logic.Parser.ScheduleAsync(Data.ScheduleParserContent
            + "&date-search=" + date.ToString("yyyy-MM-dd"), cancel: cancel);

        return RucSu.Logic.Parser.ScheduleAsync(Data.ScheduleParserContent
            + "&date-search=" + date.ToString("yyyy-MM-dd") + "&employee=" + employee, true, cancel);
    }
}
