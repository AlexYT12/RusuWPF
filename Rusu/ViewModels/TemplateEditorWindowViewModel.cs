using Rusu.Core;
using Rusu.Models;
using System;
using System.IO;

namespace Rusu.ViewModels
{
    public sealed class TemplateEditorWindowViewModel : ObservableObject
    {
        private static readonly Day dayTest = new Day(DateTime.Today, "Понедельник");
        private static readonly Lesson lessonTest = new Lesson
        (
            2,
            "Иванов Иван Иванович",
            "Физика",
            "ауд. 12"
        );
        private string _Text = "";
        public string Text
        {
            get { return _Text; }
            set { _Text = value; OnPropertyChanged(); }
        }
        private string? TextSaved;

        private int _Selected;
        public int Selected
        {
            get { return _Selected; }
            set { _Selected = value; OnPropertyChanged(); Load(); }
        }
        private string? Path
        {
            get
            {
                return Selected switch
                {
                    0 => Data.LessonTemplatePath,
                    1 => Data.DayTemplatePath,
                    2 => Data.ProgramLessonTemplatePath,
                    3 => Data.ProgramDayTemplatePath,
                    _ => null
                };
            }
        }

        public TemplateEditorWindowViewModel()
        {
            SaveCommand = new RelayCommand(x => Save());
            TestCommand = new RelayCommand(x =>
            {
                if (Selected == -1) return;
                if (TextSaved is null)
                {
                    TemplateModel obj;
                    switch (Selected)
                    {
                        case 0:
                        case 2: obj = lessonTest; break;
                        case 1:
                        case 3: obj = dayTest; break;
                        default: return;
                    }
                    TextSaved = Text;
                    Text = "Внимание! Внесённые изменения в тестовом режиме не будут сохранены."
                         + Environment.NewLine
                         + obj.GetByTemplate(Text);
                }
                else
                {
                    Text = TextSaved;
                    TextSaved = null;
                }

            });
            VariablesCommand = new RelayCommand(x =>
            {
                switch (Selected)
                {
                    case 0:
                    case 2: Text = TemplateModel.GetDocumentation(Lesson._Parameters); break;
                    case 1:
                    case 3: Text = TemplateModel.GetDocumentation(Day._Parameters); break;
                    default: return;
                }
            });
            Load();
        }

        private void Load()
        {
            var path = Path;
            if (string.IsNullOrEmpty(path)) return;
            path = Helper.ReadText(path);
            if (string.IsNullOrEmpty(path)) return;
            if (TextSaved is null) Text = path;
            else TextSaved = path;
        }
        private void Save()
        {
            if (!Directory.Exists("data/Templates"))
                Directory.CreateDirectory("data/Templates");
            var path = Path;
            if (path is null) return;
            File.WriteAllText(path, TextSaved ?? Text);
        }

        private string _Background = @"White";
        public string Background
        {
            get { return _Background; }
            set { _Background = value; OnPropertyChanged(); }
        }

        public RelayCommand VariablesCommand { get; set; }

        public RelayCommand SaveCommand { get; set; }

        public RelayCommand TestCommand { get; set; }
    }
}
