using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApp.Models
{
    public class Student : BaseEntity
    {
        public string StudentNumber { get; set; }     // Okul numarası
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Bu öğrencinin aldığı derslere ait ilişkiler
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        // Bu öğrencinin aldığı not kayıtları
        public List<GradeRecord> GradeRecords { get; set; } = new List<GradeRecord>();

        public override string ToString()
        {
            return $"{StudentNumber} - {FirstName} {LastName}";
        }
    }
}
