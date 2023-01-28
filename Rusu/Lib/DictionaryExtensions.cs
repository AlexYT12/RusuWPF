using System.Collections.Generic;

namespace Rusu.Lib;

/// <summary>
/// Методы расширения для Словаря
/// </summary>
internal static class DictionaryExtensions
{
    /// <summary>
    /// Получить значение или null.
    /// </summary>
    /// <param name="dict">Словарь</param>
    /// <param name="key">Ключ</param>
    /// <returns>значение или null</returns>
    internal static T? GoN<T>(this Dictionary<string, T>? dict, string key)
    {
        if (dict != null
         && dict.ContainsKey(key)) return dict[key];
        return default;
    }

    /// <summary>
    /// Получить значение и удалить елемент, или вернуть null.
    /// </summary>
    /// <param name="dict">Словарь</param>
    /// <param name="key">Ключ</param>
    /// <returns>значение или null</returns>
    internal static T? Pop<T>(this Dictionary<string, T>? dict, string key)
    {
        if (dict is null || !dict.ContainsKey(key)) return default;

        T? value = dict[key];
        dict.Remove(key);
        return value;
    }

    /// <summary>
    /// Создать ячейку.
    /// </summary>
    /// <param name="dict">Хранилище</param>
    /// <param name="key">Ключ</param>
    /// <param name="value">Значение</param>
    internal static void InC<T>(this Dictionary<string, T> dict, string key, T value)
    {
        if (dict.ContainsKey(key)) return;
        dict.Add(key, value);
    }

    /// <summary>
    /// Изменить или создать ячейку.
    /// </summary>
    /// <param name="dict">Хранилище</param>
    /// <param name="key">Ключ</param>
    /// <param name="value">Значение</param>
    internal static void SoC<T>(this Dictionary<string, T> dict, string key, T value)
    {
        if (dict.ContainsKey(key)) dict[key] = value;
        else dict.Add(key, value);
    }
}
