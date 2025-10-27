using System;

namespace ShopApp.Models
{
    public interface IEntity
    {
        Guid Id { get; }
        DateTime CreatedDate { get; }
    }
}
