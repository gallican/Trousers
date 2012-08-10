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
        private readonly CurrentWorkItemsProvider _currentWorkItemsProvider;

        public WorkItemsProvider(IRepository<WorkItemEntity> repository, IFilterExpressionProvider filterExpressionProvider, CurrentWorkItemsProvider currentWorkItemsProvider)
        {
            _repository = repository;
            _filterExpressionProvider = filterExpressionProvider;
            _currentWorkItemsProvider = currentWorkItemsProvider;
        }

        public IQueryable<WorkItemEntity> WorkItems
        {
            get { return _repository.Query(new FilteredActiveItemsQuery(_filterExpressionProvider.FilterExpression, _currentWorkItemsProvider.ActiveWorkItemRevisions)); }
        }
    }
}