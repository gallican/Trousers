using System.Collections.Generic;
using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Domain.Queries
{
    public class AllCurrentItemsQuery<T> : Query<T> where T : IIdentifiable, IVersionable
    {
        private readonly IDictionary<long, long> _mostRecentRevisions;

        public AllCurrentItemsQuery(IDictionary<long, long> mostRecentRevisions)
        {
            _mostRecentRevisions = mostRecentRevisions;
        }

        public override IQueryable<T> Execute(IQueryable<T> source)
        {
            return source.Where(IsMostRecent).AsQueryable();
        }

        private bool IsMostRecent(T x)
        {
            long mostRecentRevision;
            if (_mostRecentRevisions.TryGetValue(x.Id, out mostRecentRevision))
            {
                return mostRecentRevision == x.Revision;
            }

            return true;
        }
    }
}