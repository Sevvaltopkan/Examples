using ChatApp.Models;
using System.Collections.Generic;

namespace ChatApp.Data
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByUsername(string username);
        IEnumerable<User> SearchByName(string text);
    }
}
