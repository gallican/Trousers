using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Trousers.Core.Domain.Entities
{
    [DebuggerDisplay("{Id} {Revision} {LastModified}")]
    [Serializable]
    public class WorkItemEntity : IIdentifiable, IVersionable
    {
        private readonly long _id;
        private readonly long _revision;
        private readonly DateTime _lastModified;
        private readonly IDictionary<string, string> _fields;

        public WorkItemEntity(int id, int revision, DateTime lastModified, bool isCurrent, IDictionary<string, string> fields)
        {
            _id = id;
            _revision = revision;
            _lastModified = lastModified;
            _fields = fields;
        }

        public long Id
        {
            get { return _id; }
        }

        public long Revision
        {
            get { return _revision; }
        }

        public DateTime LastModified
        {
            get { return _lastModified; }
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