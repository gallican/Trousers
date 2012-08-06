using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Extensions;

namespace Trousers.Core.Domain.Queries
{
    public class FilterExpressionQuery : Query<WorkItemEntity>
    {
        private const string _tokenSplitRegex = "(\".*\")|((\\w|[\\S^\"])*( |$))";

        private readonly string _expr;

        public FilterExpressionQuery(string expr)
        {
            _expr = expr;
        }

        public override IQueryable<WorkItemEntity> Execute(IQueryable<WorkItemEntity> source)
        {
            if (string.IsNullOrWhiteSpace(_expr)) return source;

            var tokens = Tokens(_expr).ToArray();
            return source
                .AsParallel()
                .AsOrdered()
                .Where(wi => TokensMatch(wi, tokens))
                .AsQueryable()
                ;
        }

        private static IEnumerable<string> Tokens(string search)
        {
            var splitter = RegexFactory.FetchCompiledRegex(_tokenSplitRegex);

            return splitter.Matches(search.ToLowerInvariant())
                .Cast<Match>()
                .Where(token => token.Value.Trim(' ', '"') != string.Empty)
                .Select(token => token.Value.Trim(' ', '"'));
        }

        private static bool TokensMatch(WorkItemEntity wi, IEnumerable<string> tokens)
        {
            // each token must exist in at least one of the searchable fields
            foreach (var token in tokens)
            {
                IDictionary<string, string> searchFields;
                string searchToken;

                if (token.Contains(":"))
                {
                    // the token contains a field specifier.  it must be in that specific field.

                    var fieldTokens = token.Split(new[] {':'});
                    if (fieldTokens.Length != 2) continue; // misplaced colon? bad syntax? bail.

                    // searchable fields are those that match the field specifier.
                    var normalizedKey = fieldTokens[0].ToUpperInvariant();

                    searchFields = wi.Fields
                        .Where(kvp => kvp.Key.ToUpperInvariant().StartsWith(normalizedKey))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    // the token is the value component of the "key:value" pair
                    searchToken = fieldTokens[1];
                }
                else
                {
                    // the token may be in any of the searchable fields
                    searchFields = wi.Fields;

                    // the search token is just the token
                    searchToken = token;
                }

                var searchRegex = RegexFactory.FetchCompiledRegex(searchToken);
                if (searchRegex != null)
                {
                    if (!searchFields.Any(kvp => searchRegex.IsMatch(kvp.Value))) return false;
                }
                else
                {
                    var normalizedToken = searchToken.ToLowerInvariant();
                    if (!searchFields.Any(kvp => kvp.Value.Contains(normalizedToken))) return false;
                }
            }

            return true;
        }
    }
}