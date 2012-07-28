using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Trousers.Core.Extensions
{
    public static class RegexFactory
    {
        private static readonly Dictionary<string, Regex> _compiledRegexes = new Dictionary<string, Regex>();

        public static Regex FetchCompiledRegex(string expression)
        {
            Regex result;
            if (_compiledRegexes.TryGetValue(expression, out result)) return result;

            lock(_compiledRegexes)
            {
                if (_compiledRegexes.TryGetValue(expression, out result)) return result;

                try
                {
                    result = new Regex(expression, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);
                    _compiledRegexes[expression] = result;
                }
                catch (ArgumentException)
                {
                    _compiledRegexes[expression] = null;
                }

                return result;
            }
        }
    }
}