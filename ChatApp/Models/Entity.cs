using System;

namespace ChatApp.Models
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime CreatedDate { get; }
    }
}
