﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MustHave.Utilities
{
    public static class StringExtensionMethods
    {
        public static bool Contains(this string text, string substring, StringComparison comp)
        {
            if (substring == null)
            {
                throw new ArgumentNullException("substring", "substring cannot be null.");
            }
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
            {
                throw new ArgumentException("comp is not a member of StringComparison", "comp");
            }
            return text.IndexOf(substring, comp) >= 0;
        }

        public static string TrimStart(this string text, string prefix)
        {
            return text.TrimStart(prefix.ToCharArray());
        }

        public static string TrimEnd(this string text, string suffix)
        {
            return text.TrimEnd(suffix.ToCharArray());
        }

        public static string TrimPrefix(this string text, string prefix)
        {
            return text.StartsWith(prefix) ? text.Substring(prefix.Length) : text;
        }

        public static string TrimSuffix(this string text, string suffix)
        {
            return text.EndsWith(suffix) ? text.Substring(0, text.Length - suffix.Length) : text;
        }

        public static bool TryParseToEnum<T>(this string name, out T enumName)
        {
            try
            {
                enumName = (T)Enum.Parse(typeof(T), name);
                return true;
            }
            catch (ArgumentException)
            {
                Debug.LogWarning("ParseToEnum: failed to parse " + name + " to " + typeof(T).ToString());
                enumName = default;
                return false;
            }
        }

        public static T ParseToEnum<T>(this string name)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), name);
            }
            catch (ArgumentException)
            {
                Debug.LogWarning("ParseToEnum: failed to parse " + name + " to " + typeof(T).ToString());
                return default;
            }
        }

        public static bool EqualsToEnum<T>(this string name, T enumName)
        {
            return name.Equals(enumName.ToString());
        }

        public static bool EqualsToOneOfNames(this string name, params string[] array)
        {
            return array.ToList().FindIndex(item => item.Equals(name)) >= 0;
        }

        public static bool EqualsToOneOfNames<T>(this string name, params T[] array)
        {
            return array.ToList().FindIndex(item => item.ToString().Equals(name)) >= 0;
        }

        public static string RemoveWhitespace(this string input)
        {
            //return string.Join("", input.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            return new string(input.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        public static string FirstToUpper(this string text)
        {
            text = text.ToLower();
            string[] words = text.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].First().ToString().ToUpper() + words[i].Substring(1);
            }
            return string.Join(" ", words);
        }
    }
}
