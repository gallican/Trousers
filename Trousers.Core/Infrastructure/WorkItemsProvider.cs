using System.Linq;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Queries;
using Trousers.Core.Domain.Repositories;

namespace Trousers.Core.Infrastructure
{
    public class WorkItemsProvider : IWorkItemProvider
    {
        private readonly IRepository<WorkItemEntity> _repository;
        private readonly IFilterExpressionProvider _filterExpressionProvider;

        public WorkItemsProvider(IRepository<WorkItemEntity> repository, IFilterExpressionProvider filterExpressionProvider)
        {
            _repository = repository;
            _filterExpressionProvider = filterExpressionProvider;
        }

        public IQueryable<WorkItemEntity> WorkItems
        {
            get { return _repository.Query(new FilterExpressionQuery(_filterExpressionProvider.FilterExpression)).AsQueryable(); }
        }
    }
}