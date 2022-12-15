using Rusu.Core;
using Rusu.Logic;
using System;
using System.Collections.Generic;
using System.IO;

namespace Rusu.Models
{
    internal static class Data
    {
        internal static readonly string ScheduleParserContent = new RequestContentBuilder()
            .AP("branch", "4935b3ff-0858-11e0-8be3-005056bd3ce5")
            .AP("year", "828ab065-a6a6-11ec-b157-3cecef02455b")
            .AP("group", "8888e406-a6bf-11ec-b157-3cecef02455b")
            .AP("search-date=search-date").Parameters;

        // Версия
        internal const string VersionName = @"Solo leveling";
        internal const string VersionText = @"Версия: 1.9.7.4";

        /// <summary>
        /// Времена начала и конца занятий.
        /// </summary>
        internal static readonly (TimeSpan, TimeSpan)[] Times = new (TimeSpan, TimeSpan)[5]
        {
            (new TimeSpan(09, 00, 0), new TimeSpan(10, 35, 0)),

            (new TimeSpan(10, 45, 0), new TimeSpan(12, 20, 0)),

            (new TimeSpan(12, 40, 0), new TimeSpan(14, 15, 0)),

            (new TimeSpan(14, 30, 0), new TimeSpan(16, 05, 0)),

            (new TimeSpan(16, 20, 0), new TimeSpan(17, 55, 0)),
        };

        // Шаблоны
        internal static readonly string DayTemplatePath = "data/Templates/DayTemplate.txt";
        internal static readonly string ProgramDayTemplatePath = "data/Templates/ProgramDayTemplate.txt";

        internal static readonly string LessonTemplatePath = "data/Templates/LessonTemplate.txt";
        internal static readonly string ProgramLessonTemplatePath = "data/Templates/ProgramLessonTemplate.txt";

        /// <summary>
        /// Ссылки.
        /// </summary>
        internal static Dictionary<string, string> Links { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// За сколько минут напомнить о начале занатия.
        /// </summary>
        internal static int LessonRemindMinutes { get; set; } = 5;

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        internal static Dictionary<string, string?>? Load()
        {
            // Ссылки
            Links.Clear();
            if (File.Exists("data/links.txt"))
                foreach (var kv in Parser.ParseByLineAndChar(File.ReadAllLines("data/links.txt")))
                {
                    if (kv.Value == null) continue;
                    Links.Add(kv.Key, kv.Value);
                }

            // Параметры
            if (!File.Exists("data/data.txt")) return null;
            Dictionary<string, string?> data = Parser.ParseByLineAndChar(File.ReadAllLines("data/data.txt"));
            var remind = data.GoN("remind");
            if (remind != null)
            {
                int value;
                if (int.TryParse(remind, out value))
                    LessonRemindMinutes = value;
            }
            return data;
        }
    }
}
