using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApp.Models
{
    public class Course : BaseEntity
    {
        public string CourseName { get; set; }    // "Matematik 1"
        public int Credit { get; set; }           // Kredi değeri

        // Bu dersi veren öğretmen
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        // Bu derse kayıtlı öğrenciler (Enrollment üzerinden temsil edilir)
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public override string ToString()
        {
            return $"{CourseName} ({Credit} kredi) - {Teacher?.FirstName} {Teacher?.LastName}";
        }
    }
}
