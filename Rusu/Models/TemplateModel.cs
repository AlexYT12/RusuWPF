using System;
using System.Text;

namespace Rusu.Models
{
    public abstract class TemplateModel
    {
        public struct TemplateProperty
        {
            public string Name;

            public string? Template;
            public string? Description;

            public TemplateProperty(string name, string? template = null, string? description = null)
            {
                Name = name;

                Template = template;
                Description = description;
            }
        }

        public abstract TemplateProperty[] Parameters { get; }

        public abstract string? GetValue(string name);

        public virtual string GetByTemplate(string template)
        {
            foreach (var parameter in Parameters)
            {
                var start = $"<s${parameter.Name}>";
                var end = $"<e${parameter.Name}>";

                var value = GetValue(parameter.Name);
                if (value == null) template = RemoveByLabels(template, start, end);
                else template = template.Replace(start, "")
                                        .Replace(end, "")
                                        .Replace($"<${parameter.Name}>", value);
            }
            return template;
        }

        public static string GetDocumentation(TemplateProperty[] parameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("\"<$Название>\" будут заменены на значение \"Название\".");
            builder.AppendLine("\"<s$Название> Тут текст <e$Название>\" - эта часть будет удалена, если параметр \"Название\" будет пустым.");
            foreach (TemplateProperty property in parameters)
            {
                builder.Append("Название: ");
                builder.AppendLine(property.Name);
                builder.Append("Описание: ");
                builder.AppendLine(property.Description);
                builder.Append("Пример : \"");
                builder.AppendLine(property.Template + '\"');
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public static string RemoveByLabels(string text, string start, string end)
        {
            while (text.Contains(start))
            {
                if (!text.Contains(end)) break;
                var startPosition = text.IndexOf(start);
                text = text.Remove(startPosition, text.IndexOf(end) + end.Length - startPosition);
            }
            return text;
        }
    }
}
