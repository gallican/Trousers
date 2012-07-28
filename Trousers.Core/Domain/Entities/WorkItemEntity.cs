using System;
using System.Collections.Generic;

namespace Trousers.Core.Domain.Entities
{
    public class WorkItemEntity: IIdentifiable
    {
        private readonly int _id;
        private readonly DateTime _lastModified;
        private readonly IDictionary<string, string> _fields;

        public WorkItemEntity(int id, DateTime lastModified, IDictionary<string, string> fields)
        {
            _id = id;
            _lastModified = lastModified;
            _fields = fields;
        }

        public int Id
        {
            get { return _id; }
        }

        public DateTime LastModified
        {
            get { return _lastModified; }
        }

        public IDictionary<string, string> Fields
        {
            get { return _fields; }
        }
    }
}