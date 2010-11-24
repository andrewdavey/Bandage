using System.Text.RegularExpressions;

namespace MvcSample
{
    public class Util
    {
        public static string GetSlug(string s)
        {
            // just a sample - don't use in production code!
            return Regex.Replace(s.ToLower(), @"[^a-z0-9\-_]+", "-");
        }
    }
}