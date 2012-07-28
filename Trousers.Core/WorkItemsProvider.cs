using System.Linq;
using Trousers.Core.Dtos;

namespace Trousers.Core
{
    public class WorkItemsProvider : IWorkItemProvider
    {
        private readonly IRepository<WorkItemDto> _repository;

        public WorkItemsProvider(IRepository<WorkItemDto> repository)
        {
            _repository = repository;
        }

        public void SetQuery(string expr)
        {
        }

        public IQueryable<WorkItemDto> WorkItems
        {
            get { return _repository.Query(new AllWorkItemsQuery()).AsQueryable(); }
        }
    }
}