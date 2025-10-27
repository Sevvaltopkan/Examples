using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SinavAlıistirma2.Program;

namespace SinavAlıistirma2.Models
{
    public class Author : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        // RP
        public Members Members { get; set; } = null!;
    }
   
}