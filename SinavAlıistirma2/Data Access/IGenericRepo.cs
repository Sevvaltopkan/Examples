using SinavAlıistirma2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinavAlıistirma2.Data_Access
{
    public interface IGenericRepo<T> where T : class, IBaseEntity, new()
    {
        T Get(Guid id);
        List<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(Guid id);

    }
}
