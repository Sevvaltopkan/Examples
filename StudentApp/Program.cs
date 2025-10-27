using System;
using StudentApp.DataAccess;
using StudentApp.Models;
using StudentApp.Services;

namespace StudentApp
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // Repositories
            ITeacherRepository teacherRepo = new TeacherRepository();
            ICourseRepository courseRepo = new CourseRepository();
            IStudentRepository studentRepo = new StudentRepository();
            IEnrollmentRepository enrollmentRepo = new EnrollmentRepository();
            IGradeRecordRepository gradeRepo = new GradeRecordRepository();

            while (true)
            {
                Console.WriteLine("===== OKUL YÖNETİM SİSTEMİ =====");
                Console.WriteLine("1 - Öğretmen Ekle");
                Console.WriteLine("2 - Ders Oluştur");
                Console.WriteLine("3 - Öğrenci Ekle");
                Console.WriteLine("4 - Öğrenciyi Derse Kaydet");
                Console.WriteLine("5 - Not Gir (Öğrenci + Ders)");
                Console.WriteLine("6 - Öğrencileri Listele");
                Console.WriteLine("7 - Dersleri Listele");
                Console.WriteLine("8 - Öğrencinin Aldığı Dersleri Göster");
                Console.WriteLine("9 - Öğrencinin Not Dökümünü Göster");
                Console.WriteLine("10 - Öğrencinin Ortalama / Geçti mi Kaldı mı?");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçim: ");

                var secim = Console.ReadLine();
                Console.WriteLine();

                if (secim == "0")
                {
                    Console.WriteLine("Program sonlandırıldı.");
                    break;
                }

                switch (secim)
                {
                    case "1":
                        AddTeacher(teacherRepo);
                        break;

                    case "2":
                        CreateCourse(teacherRepo, courseRepo);
                        break;

                    case "3":
                        AddStudent(studentRepo);
                        break;

                    case "4":
                        EnrollStudentToCourse(studentRepo, courseRepo, enrollmentRepo);
                        break;

                    case "5":
                        EnterGrade(studentRepo, courseRepo, gradeRepo);
                        break;

                    case "6":
                        ListStudents(studentRepo);
                        break;

                    case "7":
                        ListCourses(courseRepo);
                        break;

                    case "8":
                        ShowStudentEnrollments(studentRepo, enrollmentRepo, courseRepo);
                        break;

                    case "9":
                        ShowStudentGrades(studentRepo, gradeRepo, courseRepo);
                        break;

                    case "10":
                        CheckStudentPassStatus(studentRepo);
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim.\n");
                        break;
                }
            }
        }

        // 1 - Öğretmen ekleme
        static void AddTeacher(ITeacherRepository teacherRepo)
        {
            var teacher = new Teacher();

            Console.Write("Öğretmen Adı: ");
            teacher.FirstName = Console.ReadLine();

            Console.Write("Öğretmen Soyadı: ");
            teacher.LastName = Console.ReadLine();

            teacherRepo.Add(teacher);

            Console.WriteLine("Öğretmen eklendi.");
            Console.WriteLine($"ID: {teacher.Id} | {teacher.FirstName} {teacher.LastName}\n");
        }

        // 2 - Ders oluşturma (öğretmene bağlayarak)
        static void CreateCourse(ITeacherRepository teacherRepo, ICourseRepository courseRepo)
        {
            var allTeachers = teacherRepo.GetAll();
            if (allTeachers.Count == 0)
            {
                Console.WriteLine("Ders açmak için önce öğretmen ekleyin.\n");
                return;
            }

            var course = new Course();

            Console.Write("Ders Adı: ");
            course.CourseName = Console.ReadLine();

            Console.Write("Kredi (sayı): ");
            int credit;
            int.TryParse(Console.ReadLine(), out credit);
            course.Credit = credit;

            Console.WriteLine("\nDersi verecek öğretmeni seçin:");
            for (int i = 0; i < allTeachers.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {allTeachers[i].FirstName} {allTeachers[i].LastName}  ({allTeachers[i].Id})");
            }
            Console.Write("Seçim: ");
            int tIndex;
            int.TryParse(Console.ReadLine(), out tIndex);

            if (tIndex < 1 || tIndex > allTeachers.Count)
            {
                Console.WriteLine("Geçersiz seçim.\n");
                return;
            }

            var selectedTeacher = allTeachers[tIndex - 1];
            course.Teacher = selectedTeacher;
            course.TeacherId = selectedTeacher.Id;

            // çift yönlü ilişki
            selectedTeacher.Courses.Add(course);

            // repo'ya kaydet
            courseRepo.Add(course);
            teacherRepo.Update(selectedTeacher);

            Console.WriteLine("\nDers oluşturuldu.");
            Console.WriteLine($"ID: {course.Id} | {course.CourseName} | Öğretmen: {selectedTeacher.FirstName} {selectedTeacher.LastName}\n");
        }

        // 3 - Öğrenci ekleme
        static void AddStudent(IStudentRepository studentRepo)
        {
            var student = new Student();

            Console.Write("Öğrenci No: ");
            student.StudentNumber = Console.ReadLine();

            Console.Write("Ad: ");
            student.FirstName = Console.ReadLine();

            Console.Write("Soyad: ");
            student.LastName = Console.ReadLine();

            studentRepo.Add(student);

            Console.WriteLine("Öğrenci eklendi.");
            Console.WriteLine($"ID: {student.Id} | {student.StudentNumber} - {student.FirstName} {student.LastName}\n");
        }

        // 4 - Öğrenciyi derse kaydetme
        static void EnrollStudentToCourse(
            IStudentRepository studentRepo,
            ICourseRepository courseRepo,
            IEnrollmentRepository enrollmentRepo)
        {
            var allStudents = studentRepo.GetAll();
            var allCourses = courseRepo.GetAll();

            if (allStudents.Count == 0 || allCourses.Count == 0)
            {
                Console.WriteLine("Öğrenci veya ders bulunamadı.\n");
                return;
            }

            Console.WriteLine("Öğrenci seçin:");
            for (int i = 0; i < allStudents.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {allStudents[i].StudentNumber} | {allStudents[i].FirstName} {allStudents[i].LastName}");
            }
            Console.Write("Seçim: ");
            int sIndex;
            int.TryParse(Console.ReadLine(), out sIndex);

            if (sIndex < 1 || sIndex > allStudents.Count)
            {
                Console.WriteLine("Geçersiz seçim.\n");
                return;
            }

            var selectedStudent = allStudents[sIndex - 1];

            Console.WriteLine("\nDers seçin:");
            for (int i = 0; i < allCourses.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {allCourses[i].CourseName} ({allCourses[i].Id})");
            }
            Console.Write("Seçim: ");
            int cIndex;
            int.TryParse(Console.ReadLine(), out cIndex);

            if (cIndex < 1 || cIndex > allCourses.Count)
            {
                Console.WriteLine("Geçersiz seçim.\n");
                return;
            }

            var selectedCourse = allCourses[cIndex - 1];

            // Enrollment oluştur
            var enrollment = new Enrollment();
            enrollment.Student = selectedStudent;
            enrollment.StudentId = selectedStudent.Id;
            enrollment.Course = selectedCourse;
            enrollment.CourseId = selectedCourse.Id;

            enrollmentRepo.Add(enrollment);

            // ilişkileri modelde de güncelle
            selectedStudent.Enrollments.Add(enrollment);
            selectedCourse.Enrollments.Add(enrollment);

            studentRepo.Update(selectedStudent);
            courseRepo.Update(selectedCourse);

            Console.WriteLine("\nÖğrenci derse kaydedildi.");
            Console.WriteLine($"{selectedStudent.FirstName} {selectedStudent.LastName} -> {selectedCourse.CourseName}\n");
        }

        // 5 - Not girme
        static void EnterGrade(
            IStudentRepository studentRepo,
            ICourseRepository courseRepo,
            IGradeRecordRepository gradeRepo)
        {
            var allStudents = studentRepo.GetAll();
            if (allStudents.Count == 0)
            {
                Console.WriteLine("Öğrenci yok.\n");
                return;
            }

            Console.WriteLine("Not verilecek öğrenciyi seçin:");
            for (int i = 0; i < allStudents.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {allStudents[i].StudentNumber} | {allStudents[i].FirstName} {allStudents[i].LastName}");
            }
            Console.Write("Seçim: ");
            int sIndex;
            int.TryParse(Console.ReadLine(), out sIndex);
            if (sIndex < 1 || sIndex > allStudents.Count)
            {
                Console.WriteLine("Geçersiz seçim.\n");
                return;
            }

            var selectedStudent = allStudents[sIndex - 1];

            // Bu öğrencinin hangi dersleri var?
            if (selectedStudent.Enrollments.Count == 0)
            {
                Console.WriteLine("Bu öğrenci henüz hiçbir derse kayıtlı değil.\n");
                return;
            }

            Console.WriteLine("\nHangi ders için not verilecek?");
            for (int i = 0; i < selectedStudent.Enrollments.Count; i++)
            {
                var course = selectedStudent.Enrollments[i].Course;
                Console.WriteLine($"{i + 1} - {course.CourseName}");
            }
            Console.Write("Seçim: ");
            int cIndex;
            int.TryParse(Console.ReadLine(), out cIndex);
            if (cIndex < 1 || cIndex > selectedStudent.Enrollments.Count)
            {
                Console.WriteLine("Geçersiz seçim.\n");
                return;
            }

            var chosenEnrollment = selectedStudent.Enrollments[cIndex - 1];
            var chosenCourse = chosenEnrollment.Course;

            Console.Write("Not (0-100): ");
            int score;
            int.TryParse(Console.ReadLine(), out score);

            var grade = new GradeRecord();
            grade.Student = selectedStudent;
            grade.StudentId = selectedStudent.Id;
            grade.Course = chosenCourse;
            grade.CourseId = chosenCourse.Id;
            grade.Score = score;

            gradeRepo.Add(grade);

            selectedStudent.GradeRecords.Add(grade);
            studentRepo.Update(selectedStudent);

            Console.WriteLine("\nNot kaydedildi.");
            Console.WriteLine($"{selectedStudent.FirstName} {selectedStudent.LastName} / {chosenCourse.CourseName} : {score}\n");
        }

        // 6 - Öğrencileri listele
        static void ListStudents(IStudentRepository studentRepo)
        {
            var all = studentRepo.GetAll();
            if (all.Count == 0)
            {
                Console.WriteLine("Öğrenci yok.\n");
                return;
            }

            Console.WriteLine("=== Öğrenciler ===");
            foreach (var s in all)
            {
                Console.WriteLine($"{s.StudentNumber} - {s.FirstName} {s.LastName} (ID: {s.Id})");
            }
            Console.WriteLine();
        }

        // 7 - Dersleri listele
        static void ListCourses(ICourseRepository courseRepo)
        {
            var all = courseRepo.GetAll();
            if (all.Count == 0)
            {
                Console.WriteLine("Ders yok.\n");
                return;
            }

            Console.WriteLine("=== Dersler ===");
            foreach (var c in all)
            {
                Console.WriteLine($"{c.CourseName} | Kredi: {c.Credit} | Öğretmen: {c.Teacher?.FirstName} {c.Teacher?.LastName} | ID: {c.Id}");
            }
            Console.WriteLine();
        }

        // 8 - Bir öğrencinin aldığı dersleri göster
        static void ShowStudentEnrollments(
            IStudentRepository studentRepo,
            IEnrollmentRepository enrollmentRepo,
            ICourseRepository courseRepo)
        {
            var allStudents = studentRepo.GetAll();
            if (allStudents.Count == 0)
            {
                Console.WriteLine("Öğrenci yok.\n");
                return;
            }

            Console.WriteLine("Öğrenci seç:");
            for (int i = 0; i < allStudents.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {allStudents[i].StudentNumber} | {allStudents[i].FirstName} {allStudents[i].LastName}");
            }
            Console.Write("Seçim: ");
            int index;
            int.TryParse(Console.ReadLine(), out index);
            if (index < 1 || index > allStudents.Count)
            {
                Console.WriteLine("Geçersiz seçim.\n");
                return;
            }

            var student = allStudents[index - 1];

            if (student.Enrollments.Count == 0)
            {
                Console.WriteLine("Bu öğrenci hiçbir derse kayıtlı değil.\n");
                return;
            }

            Console.WriteLine($"\n{student.FirstName} {student.LastName} öğrencisinin dersleri:");
            foreach (var enr in student.Enrollments)
            {
                Console.WriteLine($"- {enr.Course.CourseName}");
            }
            Console.WriteLine();
        }

        // 9 - Bir öğrencinin not dökümünü göster
        static void ShowStudentGrades(
            IStudentRepository studentRepo,
            IGradeRecordRepository gradeRepo,
            ICourseRepository courseRepo)
        {
            var allStudents = studentRepo.GetAll();
            if (allStudents.Count == 0)
            {
                Console.WriteLine("Öğrenci yok.\n");
                return;
            }

            Console.WriteLine("Öğrenci seç:");
            for (int i = 0; i < allStudents.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {allStudents[i].StudentNumber} | {allStudents[i].FirstName} {allStudents[i].LastName}");
            }
            Console.Write("Seçim: ");
            int stuIx;
            int.TryParse(Console.ReadLine(), out stuIx);
            if (stuIx < 1 || stuIx > allStudents.Count)
            {
                Console.WriteLine("Geçersiz seçim.\n");
                return;
            }

            var selectedStudent = allStudents[stuIx - 1];

            if (selectedStudent.GradeRecords.Count == 0)
            {
                Console.WriteLine("Bu öğrenci için henüz not girilmemiş.\n");
                return;
            }

            Console.WriteLine($"\n{selectedStudent.FirstName} {selectedStudent.LastName} not dökümü:");
            foreach (var gr in selectedStudent.GradeRecords)
            {
                Console.WriteLine($"{gr.Course.CourseName} : {gr.Score}");
            }
            Console.WriteLine();
        }

        // 10 - Öğrencinin ortalaması ve geçip geçmediği
        static void CheckStudentPassStatus(IStudentRepository studentRepo)
        {
            var allStudents = studentRepo.GetAll();
            if (allStudents.Count == 0)
            {
                Console.WriteLine("Öğrenci yok.\n");
                return;
            }

            Console.WriteLine("Öğrenci seç:");
            for (int i = 0; i < allStudents.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {allStudents[i].StudentNumber} | {allStudents[i].FirstName} {allStudents[i].LastName}");
            }

            Console.Write("Seçim: ");
            int index;
            int.TryParse(Console.ReadLine(), out index);

            if (index < 1 || index > allStudents.Count)
            {
                Console.WriteLine("Geçersiz seçim.\n");
                return;
            }

            var student = allStudents[index - 1];

            // ortalamayı hesapla
            double avg = StudentAcademicService.CalculateAverageScore(student);

            // geçti mi kaldı mı kontrol et
            bool passed = StudentAcademicService.PassedClass(student, 50); // 50 geçme notu

            Console.WriteLine();
            Console.WriteLine($"Öğrenci: {student.FirstName} {student.LastName}");
            Console.WriteLine($"Ortalama: {avg}");

            if (student.GradeRecords.Count == 0)
            {
                Console.WriteLine("Henüz not girilmemiş, durum hesaplanamıyor.");
            }
            else if (passed)
            {
                Console.WriteLine("Durum: GEÇTİ ");
            }
            else
            {
                Console.WriteLine("Durum: KALDI ");
            }

            Console.WriteLine();
        }
    }
}
