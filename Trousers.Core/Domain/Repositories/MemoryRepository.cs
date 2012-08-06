using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Trousers.Core.Domain.Entities;
using Trousers.Core.Domain.Events;
using Trousers.Core.Domain.Queries;
using Trousers.Core.Extensions;

namespace Trousers.Core.Domain.Repositories
{
    public class MemoryRepository<T> : IRepository<T> where T : class, IIdentifiable, IVersionable
    {
        private readonly List<T> _items;

        public MemoryRepository()
        {
            try
            {
                _items = LoadFromDisk();
            }
            catch (Exception)
            {
                _items = new List<T>();
            }
        }

        private string FilePath
        {
            get { return "C:\\Temp\\{0}.dat".FormatWith(GetType().Name); }
        }

        public void AddOrUpdate(T item)
        {
            AddOrUpdateInternal(item);
            DomainEvents.Raise(new WorkItemsUpdatedEvent());
            SaveToDisk();
        }

        public void AddOrUpdate(IEnumerable<T> items)
        {
            foreach (var item in items) AddOrUpdateInternal(item);
            DomainEvents.Raise(new WorkItemsUpdatedEvent());
            SaveToDisk();
        }

        public IQueryable<T> Query(Query<T> query)
        {
            return query.Execute(_items.AsQueryable());
        }

        private void AddOrUpdateInternal(T item)
        {
            var existing = _items
                .Where(x => x.Id == item.Id)
                .Where(x => x.Revision == item.Revision)
                .FirstOrDefault();
            if (existing != null) _items.Remove(existing);

            _items.Add(item);
        }

        private void SaveToDisk()
        {
            var serializer = new BinaryFormatter();
            using (var stream = File.OpenWrite(FilePath))
            {
                serializer.Serialize(stream, _items);
            }
        }

        private List<T> LoadFromDisk()
        {
            var serializer = new BinaryFormatter();
            using (var stream = File.OpenRead(FilePath))
            {
                return (List<T>) serializer.Deserialize(stream);
            }
        }
    }
}