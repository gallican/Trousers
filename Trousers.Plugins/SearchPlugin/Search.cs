using System.Linq;
using Trousers.Core.Dtos;
using Trousers.Core.Infrastructure;
using Trousers.Core.Responses;

namespace Trousers.Plugins.SearchPlugin
{
    public class Search : IPlugin
    {
        private readonly IWorkItemProvider _workItemsProvider;
        private readonly ISearchDisplayMetadataProvider _searchMetadataProvider;

        public Search(IWorkItemProvider workItemsProvider, ISearchDisplayMetadataProvider searchMetadataProvider)
        {
            _workItemsProvider = workItemsProvider;
            _searchMetadataProvider = searchMetadataProvider;
        }

        public Response Query()
        {
            var dtos = _workItemsProvider.WorkItems
                .Select(wi => WorkItemDto.FromEntity(wi))
                .Take(201)
                .ToArray();

            var response = new SearchResponse(_searchMetadataProvider.DisplayFields, _searchMetadataProvider.LongFields, dtos);
            return response;
        }
    }
}