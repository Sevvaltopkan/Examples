using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApp.DataAccess
{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        // Öğrenci-Ders kayıt ilişkisine özel şeyler buraya eklenebilir
    }
}
