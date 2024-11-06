using System.Text.RegularExpressions;

namespace VpokefficencyFramework
{
    public static class StringExtension
    {

        public static string TrimAndRemoveWhiteSpace(this string str)
        {
            return RemoveWhitespaces(str).Trim();
        }

        public static string RemoveWhitespaces(this string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }
        public static bool HasJustletters(this string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z]+$");
        }

        
    }
}
