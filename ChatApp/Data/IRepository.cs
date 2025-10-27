using System;
using System.Collections.Generic;
using ChatApp.Models;

namespace ChatApp.Data
{
    public interface IRepository<T> where T : IEntity
    {
        void Add(T entity);
        IEnumerable<T> GetAll();
        T? GetById(Guid id);
    }
}
