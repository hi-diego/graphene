using Pluralize.NET.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Graphene.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ToSingular(this string s)
            => new Pluralizer().Singularize(s);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ToPlural(this string s)
            => new Pluralizer().Pluralize(s);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string DbSetName(this string s)
            => ToSingular(ToCamelCase(s, true));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string UcFirst(this string s)
            => char.ToUpper(s[0]) + s.Substring(1);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RemovePrefix(this string s, string prefix)
            => s.StartsWith(prefix)
                ? RemovePrefix(s.Substring(1), prefix)
                : s;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string RemoveSuffix(this string s, string prefix)
            => s.StartsWith(prefix)
                ? RemovePrefix(s.Substring(s.Length - 1, 1), prefix)
                : s;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string s, bool pascal = false, bool keepSpaces = false)
        {
            Regex pattern = new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+");
            var str = new string(
              new CultureInfo("en-US", false).TextInfo.ToTitleCase(
                  string.Join(" ", pattern.Matches(s)).ToLower()
                ));
            if (!keepSpaces) str = str.Replace(@" ", "");
            return new string(str.Select((x, i) => i == 0 && !pascal ? char.ToLower(x) : x).ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
