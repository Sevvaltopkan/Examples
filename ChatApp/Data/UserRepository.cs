using System.Collections.Generic;
using System.Linq;
using ChatApp.Models;

namespace ChatApp.Data
{
    public class UserRepository : InMemoryRepository<User>, IUserRepository
    {
        public User? GetByUsername(string username)
        {
            return _items
                .FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
        }

        public IEnumerable<User> SearchByName(string text)
        {
            text = text.ToLower();
            return _items.Where(u =>
                u.FirstName.ToLower().Contains(text) ||
                u.LastName.ToLower().Contains(text));
        }
    }
}
