using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApp.Models
{
    public class Teacher : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Öğretmenin verdiği dersler
        public List<Course> Courses { get; set; } = new List<Course>();

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
