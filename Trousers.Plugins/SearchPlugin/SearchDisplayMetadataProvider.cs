using System.Linq;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Events;
using Trousers.Core.Domain.Repositories;

namespace Trousers.Plugins.SearchPlugin
{
    public class SearchDisplayMetadataProvider : IHandleEvent<WorkItemsUpdatedEvent>, ISearchDisplayMetadataProvider
    {
        private readonly IRepository<WorkItemEntity> _repository;

        private readonly string[] _displayFields = new[] { "ID", "Work Item Type", "Title", "Assigned To", "State", "Reason", "Priority" };
        private string[] _longFields = new string[0];

        public SearchDisplayMetadataProvider(IRepository<WorkItemEntity> repository)
        {
            _repository = repository;
        }

        public string[] DisplayFields
        {
            get { return _displayFields; }
        }

        public string[] LongFields
        {
            get { return _longFields; }
        }

        public void Handle(WorkItemsUpdatedEvent domainEvent)
        {
            var allFields = _repository.GetAll()
                .SelectMany(wi => wi.Fields)
                .Where(kvp => kvp.Value != null)
                .ToArray();

            _longFields = allFields
                .Where(kvp => kvp.Value.Length > 255)
                .Select(kvp => kvp.Key)
                .Distinct()
                .ToArray();
        }
    }
}