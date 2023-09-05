using Pluralize.NET.Core;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.IO.Compression;

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

        /// <summary>
        /// Compresses a string and returns a deflate compressed, Base64 encoded string.
        /// </summary>
        /// <param name="uncompressedString">String to compress</param>
        public static string Compress(this string uncompressedString)
        {
            byte[] compressedBytes;
            using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
            {
                using (var compressedStream = new MemoryStream())
                { 
                    // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                    // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                    // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
                    using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Fastest, true))
                    {
                        uncompressedStream.CopyTo(compressorStream);
                    }

                    // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                    compressedBytes = compressedStream.ToArray();
                }
            }
            return Convert.ToBase64String(compressedBytes);
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public static string Decompress(this string compressedString)
        {
            byte[] decompressedBytes;

            var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                using (var decompressedStream = new MemoryStream())
                {
                    decompressorStream.CopyTo(decompressedStream);

                    decompressedBytes = decompressedStream.ToArray();
                }
            }
            return Encoding.UTF8.GetString(decompressedBytes);
        }

        
        public static Guid FromBase64 (this string guid) {
            try { return new Guid(WebEncoders.Base64UrlDecode(guid)); }
            catch { return Guid.Empty; }
        }
    }
}
