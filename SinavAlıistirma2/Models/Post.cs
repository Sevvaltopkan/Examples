using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinavAlıistirma2.Models
{
    public class Post : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        // NP
        public Category Category { get; set; } = null!;

    }
}
