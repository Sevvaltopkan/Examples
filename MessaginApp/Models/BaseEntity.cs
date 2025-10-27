using System;

namespace ChatApp.Models
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
        }

        public override string ToString()
        {
            // İstersen burada generic bir özet yazarsın
            return $"[{Id}] Created: {CreatedDate}";
        }
    }
}
