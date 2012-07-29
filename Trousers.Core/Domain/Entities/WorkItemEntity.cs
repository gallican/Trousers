using System;
using System.Collections.Generic;

namespace Trousers.Core.Domain.Entities
{
    public class WorkItemEntity : IIdentifiable, IDeactivatable
    {
        private readonly int _id;
        private readonly int _revision;
        private readonly DateTime _lastModified;
        private readonly bool _isCurrent;
        private readonly IDictionary<string, string> _fields;

        public WorkItemEntity(int id, int revision, DateTime lastModified, bool isCurrent, IDictionary<string, string> fields)
        {
            _id = id;
            _revision = revision;
            _lastModified = lastModified;
            _isCurrent = isCurrent;
            _fields = fields;
        }

        public int Id
        {
            get { return _id; }
        }

        public int Revision
        {
            get { return _revision; }
        }

        public DateTime LastModified
        {
            get { return _lastModified; }
        }

        public bool IsCurrent
        {
            get { return _isCurrent; }
        }

        public IDictionary<string, string> Fields
        {
            get { return _fields; }
        }

        public override int GetHashCode()
        {
            long combinedHash = _id.GetHashCode() * _revision.GetHashCode();
            return combinedHash.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            return obj.GetHashCode() == GetHashCode();
        }
    }
}