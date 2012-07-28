using System.Linq;
using Trousers.Core;
using Trousers.Core.Infrastructure;
using Trousers.Core.Responses;

namespace Trousers.Plugins.SearchPlugin
{
    public class Search : IPlugin
    {
        private readonly IWorkItemProvider _workItemsProvider;

        public Search(IWorkItemProvider workItemsProvider)
        {
            _workItemsProvider = workItemsProvider;
        }

        public Response Query()
        {
            var strings = _workItemsProvider.WorkItems
                .Select(wi => string.Format("{0}<br />", wi.Fields["Title"]))
                .ToArray();
            var html = string.Join(string.Empty, strings);

            var response = new HtmlResponse(html);
            return response;
        }
    }
}