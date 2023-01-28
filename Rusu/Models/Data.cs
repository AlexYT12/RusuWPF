using Rusu.Logic;
using System.Collections.Generic;
using System.IO;

namespace Rusu.Models;

internal static class Data
{
    internal const string ScheduleParserContent =
"branch=4935b3ff-0858-11e0-8be3-005056bd3ce5&year=828ab065-a6a6-11ec-b157-3cecef02455b&group=8888e406-a6bf-11ec-b157-3cecef02455b&search-date=search-date";

    // Версия
    internal const string VersionName = @"К истокам";
    internal const string VersionText = @"Версия: 2.0";

    // Пути к шаблонам.
    internal const string DayTemplatePath = "data/Templates/DayTemplate.txt";
    internal const string LessonTemplatePath = "data/Templates/LessonTemplate.txt";

    /// <summary>
    /// Загрузка данных.
    /// </summary>
    internal static Dictionary<string, string?>? Load()
    {
        // Параметры
        if (!File.Exists("data/data.txt")) return null;
        return Parser.ParseByLineAndChar(File.ReadAllLines("data/data.txt"));
    }
}
