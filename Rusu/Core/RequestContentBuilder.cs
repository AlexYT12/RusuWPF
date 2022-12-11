using System;

namespace Rusu.Core
{
    /// <summary>
    /// Удобный класс для запросов.
    /// </summary>
    internal class RequestContentBuilder
    {
        /// <summary>
        /// Параметры.
        /// </summary>
        internal string Parameters { get; set; }

        /// <summary>
        /// Создать пустой запрос.
        /// </summary>
        internal RequestContentBuilder() { Parameters = ""; }

        /// <summary>
        /// Аналог метода AddParameter для кода в одну строчку.
        /// </summary>
        internal RequestContentBuilder AP(string parameter)
        {
            AddParameter(parameter);
            return this;
        }

        /// <summary>
        /// Аналог метода AddParameter для кода в одну строчку.
        /// </summary>
        internal RequestContentBuilder AP(string key, string value)
        {
            AddParameter(key, value);
            return this;
        }

        /// <summary>
        /// Аналог метода ReplaceParameter для кода в одну строчку.
        /// </summary>
        internal RequestContentBuilder RP(string key, string value)
        {
            ReplaceParameter(key, value);
            return this;
        }

        /// <summary>
        /// Аналог метода RemoveParameter для кода в одну строчку.
        /// </summary>
        internal RequestContentBuilder RM(string key)
        {
            RemoveParameter(key);
            return this;
        }

        /// <summary>
        /// Аналог метода SetOrCreate для кода в одну строчку.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        internal RequestContentBuilder SoC(string key, string value)
        {
            SetOrCreate(key, value);
            return this;
        }

        /// <summary>
        /// Добавить параметр запроса.
        /// </summary>
        internal void AddParameter(string parameter)
        {
            if (string.IsNullOrEmpty(Parameters)) Parameters = parameter;
            else Parameters += '&' + parameter;
        }

        /// <summary>
        /// Добавить параметр запроса.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        internal void AddParameter(string key, string value)
        {
            if (string.IsNullOrEmpty(Parameters)) Parameters = $"{key}={value}";
            else Parameters += '&' + $"{key}={value}";
        }

        /// <summary>
        /// Заменить значение параметра.
        /// </summary>
        /// <param name="key">Ключ параметра</param>
        /// <param name="value">Новое значение параметра</param>
        internal void ReplaceParameter(string key, string value)
        {
            var index = Parameters.IndexOf(key);
            if (index == -1) throw new Exception($"Not found parameter key {key}");
            index += key.Length + 1;
            var length = Parameters.IndexOf('&', index);
            if (length == -1)
            {
                Parameters = Parameters.Remove(index);
                Parameters += value;
            }
            else
            {
                Parameters = Parameters.Remove(index, length - index);
                Parameters = Parameters.Insert(index, value);
            }
        }

        /// <summary>
        /// Удаляет параметр.
        /// </summary>
        /// <param name="key">Ключ удаляемого параметра.</param>
        internal void RemoveParameter(string key)
        {
            var index = Parameters.IndexOf(key);
            if (index == -1) throw new Exception($"Not found parameter key {key}");
            var length = Parameters.IndexOf('&', index + key.Length);
            if (length == -1) Parameters = Parameters.Remove(index == 0 ? index : index - 1);
            else Parameters = Parameters.Remove(index, length - index + 1);
        }

        /// <summary>
        /// Создаёт или менет значение параметра.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        internal void SetOrCreate(string key, string value)
        {
            var index = Parameters.IndexOf(key);
            if (index == -1) AddParameter(key, value);
            else ReplaceParameter(key, value);
        }

        internal RequestContentBuilder Clone()
        {
            return new RequestContentBuilder { Parameters = Parameters };
        }
    }
}
