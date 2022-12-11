using System.Collections.Generic;

namespace Rusu.Models
{
    public struct LessonCounterModel
    {
        public string Text { get; set; }
        public List<string> Days { get; set; }
        public int Online { get; set; }
    }
}
