using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinavAlistirma.Models
{
    public interface IBaseUser
    {
        public Guid Id { get;  }
        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; }
        public DateTime DeleteDate { get; set; }
        public bool IsActive { get; set; }
    }
}
