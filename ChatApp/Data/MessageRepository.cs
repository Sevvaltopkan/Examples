using System;
using System.Collections.Generic;
using System.Linq;
using ChatApp.Models;

namespace ChatApp.Data
{
    public class MessageRepository : InMemoryRepository<Message>, IMessageRepository
    {
        public IEnumerable<Message> GetMessagesByUser(Guid userId)
        {
            return _items.Where(m => m.SenderId == userId || m.ReceiverId == userId);
        }
    }
}
