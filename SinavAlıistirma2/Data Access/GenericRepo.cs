using SinavAlıistirma2.Models;

namespace SinavAlıistirma2.Data_Access
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class, IBaseEntity, new()
    {

        static List<T> abc = new List<T>();
        public void Create(T entity)
        {
            abc.Add(entity);
        }

        public void Delete(Guid id)
        {

            T? date = abc.FirstOrDefault(x => x.Id == id);
            if (date != null)
            {
                date.DeletedDate = DateTime.Now;
                abc.Remove(date);
            }

        }

        public T Get(Guid id)
        {
            T? t = abc.FirstOrDefault(x => x.Id == id);
            if (t != null)
                return t;
            return new T();
        }

        public List<T> GetAll() => abc.Where(x => x.IsActive).ToList();

        public void Update(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
            int index = abc.IndexOf(entity);
            if (index != -1)
                abc[index] = entity;
        }



    }
}