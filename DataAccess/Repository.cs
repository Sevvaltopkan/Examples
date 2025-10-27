using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApp.DataAccess
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        // In-Memory static tablo
        private static List<T> _table = new List<T>();

        public void Add(T entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            entity.IsActive = true;

            _table.Add(entity);
        }

        public void Update(T entity)
        {
            entity.UpdatedAt = DateTime.Now;

            var existing = _table.FirstOrDefault(x => x.Id == entity.Id);
            if (existing != null)
            {
                _table.Remove(existing);
                _table.Add(entity);
            }
        }

        public void Delete(Guid id)
        {
            var existing = _table.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                _table.Remove(existing);
            }
        }

        public T GetById(Guid id)
        {
            return _table.FirstOrDefault(x => x.Id == id);
        }

        public List<T> GetAll()
        {
            return new List<T>(_table);
        }
    }
}
