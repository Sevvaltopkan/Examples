using SinavAlistirma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinavAlistirma.DataAccess
{
    public interface IGenericRepo<T> where T : class, IBaseUser, new()
    {
        T Get(Guid id);
        List<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void HardDelete(Guid id);
        void SoftDelete (Guid id);
    }
}
