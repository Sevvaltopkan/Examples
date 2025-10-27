using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinavAlistirma.Models
{
    public abstract class User : IBaseUser
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsActive { get; set; }  
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        public DateTime DeleteDate { get; set; }

    }
}
