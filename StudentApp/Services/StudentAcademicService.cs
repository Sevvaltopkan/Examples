using StudentApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApp.Services
{
    public static class StudentAcademicService
    {
        // Ortalama puanı hesaplar ( öğrencinin tüm GradeRecords içinden )
        public static double CalculateAverageScore(Student student)
        {
            if (student == null) return 0;
            if (student.GradeRecords == null || student.GradeRecords.Count == 0)
                return 0;

            // tüm notların ortalaması
            double sum = 0;
            foreach (var gr in student.GradeRecords)
            {
                sum += gr.Score;
            }

            double avg = (double)sum / student.GradeRecords.Count;
            return avg;
        }

        // Sınıfı geçti mi?
        // threshold = geçme notu (ör: 50)
        public static bool PassedClass(Student student, int threshold = 50)
        {
            double avg = CalculateAverageScore(student);
            return avg >= threshold;
        }
    }
}
