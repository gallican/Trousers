using System.Collections.Generic;
using System.Linq;

namespace Trousers.Core
{
    public abstract class Query<T>
    {
        public abstract IEnumerable<T> Execute(IQueryable<T> source);
    }
}