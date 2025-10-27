using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinavAlistirma.Models
{
    public class Seller : User
    {

        public string CompanyName { get; set; } = null!;
        public string CompanyAddress { get; set; } = null!;


    }
}
