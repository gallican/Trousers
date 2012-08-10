using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Domain.Queries
{
    public class AllRevisionsByIdQuery<T> : Query<T> where T : class, IIdentifiable
    {
        private readonly long _id;

        public AllRevisionsByIdQuery(long id)
        {
            _id = id;
        }

        public override IQueryable<T> Execute(IQueryable<T> source)
        {
            return source.Where(item => item.Id == _id);
        }
    }
}