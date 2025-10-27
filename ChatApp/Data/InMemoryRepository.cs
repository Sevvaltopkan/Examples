using System;
using System.Collections.Generic;
using System.Linq;
using ChatApp.Models;

namespace ChatApp.Data
{
    public class InMemoryRepository<T> : IRepository<T> where T : IEntity
    {
        protected readonly List<T> _items = new List<T>();

        public virtual void Add(T entity)
        {
            _items.Add(entity);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _items;
        }

        public virtual T? GetById(Guid id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }
    }
}
