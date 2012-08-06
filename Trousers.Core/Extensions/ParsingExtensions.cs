namespace Trousers.Core.Extensions
{
    public static class ParsingExtensions
    {
        public static int ToIntOrDefault(this string s, int defaultValue)
        {
            int result;
            if (int.TryParse(s, out result)) return result;
            return defaultValue;
        }
    }
}