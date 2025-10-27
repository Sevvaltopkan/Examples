using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinavAlistirma.Models
{
    public class Customer : User
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime BirthDate { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} {BirthDate} {Username}";
        }
    }
}
