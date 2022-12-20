using Rusu.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Rusu.Models
{
    internal static class Theme
    {
        public static Brush TextColor { get; set; } = new SolidColorBrush(Colors.White);

        public static Brush BorderBackground { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#65000000"));
        public static Brush BorderColor { get; set; } = new SolidColorBrush(Colors.Black);

        public static Brush ButtonBackground { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A4000000"));
        public static Brush ButtonMouseOverBackground { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B2222222"));
        public static Brush ButtonPressedBackground { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B0000000"));

        public static Brush ButtonNoBackgroundColor { get; set; } = new SolidColorBrush(Colors.DarkGray);
        public static Brush ButtonNoBackgroundMouseOverColor { get; set; } = new SolidColorBrush(Colors.Gray);
        public static Brush ButtonNoBackgroundPressedColor { get; set; } = new SolidColorBrush(Colors.Black);

        public static Brush RadioButtonDefaultColor { get; set; } = new SolidColorBrush(Colors.Gray);
        public static Brush RadioButtonSelectedColor { get; set; } = new SolidColorBrush(Colors.White);
    }
}
