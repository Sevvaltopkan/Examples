using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApp.DataAccess
{
    public interface IStudentRepository : IRepository<Student>
    {
        // İleride öğrenciye özel metotlar (ör: FindByNumber) eklenebilir.
    }
}
