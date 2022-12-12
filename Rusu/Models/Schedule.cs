using Rusu.Core;
using Rusu.Logic;
using System;
using System.Collections.Generic;

namespace Rusu.Models
{
    public sealed class Day : TemplateModel
    {
        public Day(DateTime date, string dayOfWeek)
        {
            Date = date;
            DayOfWeek = dayOfWeek;
        }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// День недели
        /// </summary>
        public string DayOfWeek { get; set; }
        /// <summary>
        /// Занятия
        /// </summary>
        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
        /// <summary>
        /// Короткая дата: Завтра.
        /// </summary>
        public string ShortDate
        {
            get
            {
                var value = StringFormater.ShortDateName(Date);
                if (value == null) return WeekDate;
                return $"{value} ({Date.ToString("dd.MM")})";
            }
        }
        /// <summary>
        /// День недели с датой: Понедельник (26.07).
        /// </summary>
        /// <returns></returns>
        public string WeekDate
        {
            get
            {
                return Date.ToString($"{DayOfWeek} (dd.MM)");
            }
        }
        /// <summary>
        /// Получить текст дня по программному шаблону.
        /// </summary>
        public string ByTemplateProgram
        {
            get { return StringFormater.DayAsString(this, Data.ProgramDayTemplatePath, lessons: false); }
        }

        public override TemplateProperty[] Parameters => _Parameters;
        public static TemplateProperty[] _Parameters = new TemplateProperty[]
        {
            new TemplateProperty("Date", "01.09.2022", "Дата"),
            new TemplateProperty("DayOfWeek", "Понедельник", "День недели"),
            new TemplateProperty("ShortDate", "Завтра", "Короткая дата"),
            new TemplateProperty("WeekDate", "Понедельник(26.07)", "День недели с датой"),
        };

        public override string? GetValue(string name)
        {
            return name switch
            {
                "Date" => Date.ToShortDateString(),
                "DayOfWeek" => DayOfWeek,
                "ShortDate" => ShortDate,
                "WeekDate" => WeekDate,
                _ => null
            };
        }

        /// <summary>
        /// День недели с датой: Понедельник (26.07).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return WeekDate;
        }
    }
    public sealed class Lesson : TemplateModel
    {
        public Lesson(byte id, string name, string teacher, string position, string lessonType)
        {
            Id = id;
            Name = name;
            Teacher = teacher;
            Position = position;
            LessonType = lessonType;
        }

        public byte Id { get; set; }
        /// <summary>
        /// Название предмета.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Имя учителя.
        /// </summary>
        public string Teacher { get; set; }
        /// <summary>
        /// Место где будет проводится занятие.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Тип занятия
        /// </summary>
        public string LessonType { get; set; }

        /// <summary>
        /// Время начала занятия.
        /// </summary>
        public TimeSpan Start { get { return Data.Times[Id - 1].Item1; } }
        /// <summary>
        /// Время начала конца.
        /// </summary>
        public TimeSpan End { get { return Data.Times[Id - 1].Item2; } }

        /// <summary>
        /// Ссылка
        /// </summary>
        public string? Url
        {
            get
            {
                return Data.Links.GoN(Name);
            }
        }

        public bool IsOnline
        {
            get
            {
                return Position.Contains("онлайн");
            }
        }

        /// <summary>
        /// Редактированное место проведения занятия.
        /// </summary>
        public string PositionEdited
        {
            get
            {
                return (IsOnline ? "Онлайн: " + Teacher : Position.Replace("ауд.", "Ауд.")) + ", " + LessonType;
            }
        }

        /// <summary>
        /// Время занятия.
        /// </summary>
        public string Time
        {
            get
            {
                return $"{Start.Hours:d2}:{Start.Minutes:d2}—{End.Hours:d2}:{End.Minutes:d2}";
            }
        }

        /// <summary>
        /// Получить текст занятия по шаблону.
        /// </summary>
        public string ByTemplate
        {
            get
            {
                return StringFormater.LessonAsString(this, Data.LessonTemplatePath);
            }
        }
        /// <summary>
        /// Получить текст занятия по программному шаблону.
        /// </summary>
        public string ByTemplateProgram
        {
            get
            {
                return StringFormater.LessonAsString(this, Data.ProgramLessonTemplatePath);
            }
        }

        public override TemplateProperty[] Parameters => _Parameters;
        public static TemplateProperty[] _Parameters = new TemplateProperty[]
        {
            new TemplateProperty("Id", "3", "Номер занятие"),
            new TemplateProperty("Name", "Физика", "Название предмета"),
            new TemplateProperty("Teacher", "Иван Иванович Иванов", "Имя преподавателя"),
            new TemplateProperty("Position", "ауд. онлайн 2, лекции", "Место проведения занятия"),

            new TemplateProperty("Start", "12.40.00", "В сколько начнётся занятие"),
            new TemplateProperty("End", "14.15.00" , "В сколько закончится занятие"),

            new TemplateProperty("Url", "https://meet.google.com",  "Ссылка на занятие"),
            new TemplateProperty("IsOnline", "да", "Онлайн ли занятие"),
            new TemplateProperty("LessonType", "лекции", "Тип занятия."),
            new TemplateProperty("PositionEdited", "Онлайн: Иван Иванович Иванов", "Редактированное место проведения занятия"),
            new TemplateProperty("Time", "12:40—14:15", "Время проведения пары"),
        };

        public override string? GetValue(string name)
        {
            return (name) switch
            {
                "Id" => Id.ToString(),
                "Name" => Name,
                "Teacher" => Teacher,
                "Position" => Position,

                "Start" => $"{Start.Hours:d2}:{Start.Minutes:d2}",
                "End" => $"{End.Hours:d2}:{End.Minutes:d2}",

                "Url" => Url,
                "IsOnline" => IsOnline ? "да" : "нет",
                "LessonType" => LessonType,
                "PositionEdited" => PositionEdited,
                "Time" => Time,
                _ => null
            };
        }

        /// <summary>
        /// Текст занятия.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Id}. {Name}.{Environment.NewLine}{PositionEdited}.{Environment.NewLine}Время: {Time}.";
        }
    }
}
