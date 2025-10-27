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
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            entity.IsActive = true;

            _table.Add(entity);
        }

        public void Update(T entity)
        {
            entity.UpdatedDate = DateTime.Now;

            var existing = _table.FirstOrDefault(x => x.Id == entity.Id);
            if (existing != null)
            {
                _table.Remove(existing);
                _table.Add(entity);
            }
        }

        public void Delete(Guid id)
        {
            T? existing = _table.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                existing.IsActive = false;
                existing.DeletedDate = DateTime.Now;
            }
        }

        public T GetById(Guid id)
        {
            return _table.FirstOrDefault(x => x.Id == id);
        }

        public List<T> GetAll()
        {
            return _table.Where(x => x.IsActive).ToList();
        }
    }
}
