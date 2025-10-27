using System;
using System.Collections.Generic;
using ChatApp.Models;

namespace ChatApp.Data
{
    public interface IMessageRepository : IRepository<Message>
    {
        IEnumerable<Message> GetMessagesByUser(Guid userId);
    }
}
