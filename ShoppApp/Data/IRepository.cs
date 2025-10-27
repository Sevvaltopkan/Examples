using System;
using System.Collections.Generic;
using ShopApp.Models;

namespace ShopApp.Data
{
    public interface IRepository<T> where T : IEntity
    {
        void Add(T entity);
        IEnumerable<T> GetAll();
        T? GetById(Guid id);
    }
}
