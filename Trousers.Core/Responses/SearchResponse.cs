using Trousers.Core.Dtos;

namespace Trousers.Core.Responses
{
    public class SearchResponse : JsonResponse
    {
        private readonly string[] _displayFields;
        private readonly string[] _longFields;
        private readonly WorkItemDto[] _workItems;

        public SearchResponse(string[] displayFields, string[] longFields, WorkItemDto[] workItems)
        {
            _displayFields = displayFields;
            _longFields = longFields;
            _workItems = workItems;
        }

        public bool IsWorkItems
        {
            get { return true; }
        }

        public string[] DisplayFields
        {
            get { return _displayFields; }
        }

        public string[] LongFields
        {
            get { return _longFields; }
        }

        public WorkItemDto[] WorkItems
        {
            get { return _workItems; }
        }
    }
}