using System.Linq;
using Trousers.Core.Dtos;

namespace Trousers.Core
{
    public class WorkItemsProvider : IWorkItemProvider
    {
        private readonly IRepository<WorkItemDto> _repository;
        private readonly IFilterExpressionProvider _filterExpressionProvider;

        public WorkItemsProvider(IRepository<WorkItemDto> repository, IFilterExpressionProvider filterExpressionProvider)
        {
            _repository = repository;
            _filterExpressionProvider = filterExpressionProvider;
        }

        public IQueryable<WorkItemDto> WorkItems
        {
            get { return _repository.Query(new FilterExpressionQuery(_filterExpressionProvider.FilterExpression)).AsQueryable(); }
        }
    }
}