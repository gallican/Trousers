using System.Linq;
using Trousers.Core.Domain.Entities;

namespace Trousers.Core.Dtos
{
    public class WorkItemDto
    {
        private readonly string[] _keys;
        private readonly string[] _values;

        private WorkItemDto(string[] keys, string[] values)
        {
            _keys = keys;
            _values = values;
        }

        public string[] Keys
        {
            get { return _keys; }
        }

        public string[] Values
        {
            get { return _values; }
        }

        public static WorkItemDto FromEntity(WorkItemEntity wi)
        {
            var keys = wi.Fields.Keys.ToArray();
            var values = keys.Select(k => wi.Fields[k]).ToArray();
            return new WorkItemDto(keys, values);
        }
    }
}