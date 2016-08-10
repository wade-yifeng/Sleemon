namespace Sleemon.Common
{
    using System.Text.RegularExpressions;

    public static class SqlDbHelper
    {
        public static string GetColumnName(this string emdxPropertyName)
        {
            var match = Regex.Match(emdxPropertyName, @"^C([0-9][0-9A-Za-z_]*)$");
            if (match.Success)
            {
                return string.Format("[{0}]", match.Result("$1"));
            }
            else
            {
                return string.Format("[{0}]", emdxPropertyName);
            }
        }
    }
}
