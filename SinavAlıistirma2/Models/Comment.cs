using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SinavAlıistirma2.Program;

namespace SinavAlıistirma2.Models
{
    public class Comment :BaseEntity
    {
        public Guid PostId { get; set; }
        public Guid MemberId { get; set; }
        public string Content { get; set; } = null!;
        // NP
        public Post Post { get; set; } = null!;
        public Members Member { get; set; } = null!;
    }
}
