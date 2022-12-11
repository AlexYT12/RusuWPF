using System.Collections.Generic;

namespace Rusu.Models
{
    public struct LessonCounterModel
    {
        public LessonCounterModel(string text, List<string> days)
        {
            Text = text;
            Days = days;
        }

        public string Text { get; set; }
        public List<string> Days { get; set; }
    }
}
