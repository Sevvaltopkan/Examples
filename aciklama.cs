using System;

public class Class1
{
	public Class1()
	{
        /*
		 * Harika, şimdi bu projeyi birlikte “mimari olarak” okuyalım. Amacımız şu olsun:

* Bu proje hangi katmanlardan oluşuyor?
* Generic yapı (Repository<T>) neden var?
* Interface’ler ne işe yarıyor?
* Program.cs içindeki static fonksiyonlar neden böyle yazılmış?
* Sen başka bir projede buna bakarak nasıl benzer mantığı kurarsın?

Aynı dili kullanayım, C# ile backend / console tarzı minik bir “Okul Yönetim Sistemi” bu.

---

## 1. Genel Mimari Yapı

Proje kabaca 3 katmandan oluşuyor:

1. **Models/**

   * Domain (iş) nesneleri: `Student`, `Teacher`, `Course`, `Enrollment`, `GradeRecord`
   * Ortak base sınıf: `BaseEntity`, `IBaseEntity`

2. **DataAccess/**

   * Veriyi saklayan ve yöneten katman (Repository katmanı)
   * `Repository<T>`: generic CRUD mantığı
   * Onu implement eden spesifik repository’ler: `StudentRepository`, `TeacherRepository`, vs.
   * Ayrıca bu repository’lerin interface’leri (`IStudentRepository`, `ITeacherRepository`, …)

3. **Services/**

   * İş kuralları / hesaplama mantığı: `StudentAcademicService`
   * Mesela ortalama hesaplama, geçti mi kaldı mı gibi “iş zekâsı”.

4. **Program.cs**

   * Console UI (menü, kullanıcıdan input al, ilgili servisi çağır, sonuç yazdır)
   * Yani bu katman “presentasyon” gibi davranıyor.

Bu yapı aslında mini bir “layered architecture”:

* **Presentation Layer** → `Program.cs`
* **Business / Service Layer** → `Services/StudentAcademicService.cs`
* **Data Access Layer** → `DataAccess/`
* **Domain Layer** → `Models/`

Bu dört parçayı ayırmak, projeyi büyütürken seni kurtarır. Çünkü mantık şunu diyor:

* “Not hesaplama kuralı değişirse Program.cs değişmez, sadece Service değişir.”
* “Veri artık memory’de değil de gerçek bir database’de tutulacaksa Program.cs yine değişmez, sadece Repository’nin içi değişir.”

Bu güzel 👍

---

## 2. Model Katmanı (Models/)

### BaseEntity ve IBaseEntity

```csharp
public interface IBaseEntity
{
    Guid Id { get; set; }
    DateTime CreatedDate { get; set; }
    DateTime UpdatedDate { get; set; }
    DateTime DeletedDate { get; set; }
    bool IsActive { get; set; }
}
```

```csharp
public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime DeletedDate { get; set; }
    public bool IsActive { get; set; }
}
```

Ne amaçla yapılıyor?

* Tüm varlıkların (`Student`, `Teacher`, `Course` vb.) ortak alanları olsun diye.
* Mesela hepsinde `Id`, `CreatedDate`, `IsActive` gibi alanlar var.
* Bu tekrar eden alanları her class’a tek tek yazmamak için `BaseEntity` oluşturuluyor ve diğer modeller `: BaseEntity` diyerek bunu miras alıyor.

Bu sana şunu sağlar:

* Repository generic çalışabilir çünkü biliyor ki her T’nin `Id`’si var, `IsActive`’i var.

### Asıl modeller:

* `Student`: numara, ad-soyad, aldığı dersler (`Enrollments`) ve not kayıtları (`GradeRecords`)
* `Teacher`: ad-soyad, verdiği dersler (`Courses`)
* `Course`: ders adı, kredi, bunu veren öğretmen
* `Enrollment`: hangi öğrenci hangi derse kayıtlı
* `GradeRecord`: hangi öğrenci hangi dersten kaç aldı

Bunlar tamamen “okul dünyasını” temsil ediyor. Bunlara “entity” diyebilirsin.

Hepsinin `ToString()` override etmesi güzel; konsola yazdırırken düzgün dursun diye.

---

## 3. DataAccess Katmanı (Repository yapısı)

### 3.1 IRepository<T> (interface)

```csharp
public interface IRepository<T> where T : IBaseEntity
{
    void Add(T entity);
    void Update(T entity);
    void Delete(Guid id);
    T GetById(Guid id);
    List<T> GetAll();
}
```

Bu interface şunu söylüyor:

* “Ben bir depoyum. Herhangi bir T tipi için (öğrenci olabilir, öğretmen olabilir...) ekleme, güncelleme, silme, listeleme yapabilirim.”

Önemli noktalar:

* `where T : IBaseEntity` kısıtı.
  Bu şu anlama geliyor:
  “Bu repository sadece Id, CreatedDate, IsActive gibi zorunlu alanları olan varlıklarla çalışır.”

  Neden önemli?
  Çünkü `Delete` metodunda `IsActive = false` yapıyor. Bunu yapabilmesi için her T’nin `IsActive` alanı olduğundan emin olmak lazım. Bu garanti `IBaseEntity` ile sağlanıyor.

Bu interface bir sözleşme. Diyor ki:

* “Benden türeyen her repository Add, Update, Delete, GetById, GetAll fonksiyonlarını sağlayacak.”

Bu sayede:

* Program.cs üst seviye kodunda `IStudentRepository` ile konuşuyorsun, implementasyonu değiştirmek çok kolay.

### 3.2 Repository<T> (generic class)

```csharp
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private static List<T> _table = new List<T>();

    public void Add(T entity)
    {
        entity.Id = Guid.NewGuid();
        entity.CreatedDate = DateTime.Now;
        entity.IsActive = true;
        _table.Add(entity);
    }

    public void Update(T entity)
    {
        var existing = _table.FirstOrDefault(x => x.Id == entity.Id);
        if (existing != null)
        {
            entity.UpdatedDate = DateTime.Now;
            // burada gerçek bir map yok ama normalde field field kopyalanır
        }
    }

    public void Delete(Guid id)
    {
        var existing = _table.FirstOrDefault(x => x.Id == id);
        if (existing != null)
        {
            existing.IsActive = false;
            existing.DeletedDate = DateTime.Now;
        }
    }

    public T GetById(Guid id)
    {
        return _table.FirstOrDefault(x => x.Id == id);
    }

    public List<T> GetAll()
    {
        return _table.Where(x => x.IsActive).ToList();
    }
}
```

Burası projenin kalbi gibi.

#### Burada ne yapılıyor?

* `_table` diye static bir `List<T>` var.
  Bu bir çeşit sahte veritabanı. RAM üstünde duruyor. Uygulama kapanınca veri gider ama demo için yeterli.

* `Add` metodu:

  * Id otomatik atıyor (`Guid.NewGuid()`).
  * Oluşturulma zamanını yazıyor.
  * IsActive = true ediyor.
  * Listeye ekliyor.

  Yani `Id` oluşturma sorumluluğu entity tarafında değil repo tarafında. Bu da güzel bir pattern.

* `Update` metodu:

  * Aynı Id’ye sahip nesneyi buluyor.
  * `UpdatedDate` güncelliyor.
  * Normalde burada `existing` içindeki alanları yeni `entity` ile set etmek gerekir. Bu örnek kodda sadece tarih atılmış ama problem değil, skeleton mantık bu.

* `Delete` metodu:

  * Gerçekten silmiyor.
  * Soft delete yapıyor: `IsActive = false`, `DeletedDate = now`.
  * Bu çok profesyonel bir yaklaşım çünkü kaydı tamamen yok etmiyorsun (log açısından önemli).

* `GetAll` metodu:

  * Sadece aktif kayıtları döndürüyor (`IsActive == true`).
  * Yani silinmiş gibi işaretlenenler gelmez.

Bu generic yapı ne işe yarıyor?

* `StudentRepository` yazarken bütün bu Add/Update/Delete kodunu tekrar yazmıyorsun.
* `TeacherRepository`, `CourseRepository`, `EnrollmentRepository`, `GradeRecordRepository` hepsi sadece şöyle:

```csharp
public class StudentRepository : Repository<Student>, IStudentRepository
{
}
```

Yani:

* Ortak CRUD -> `Repository<T>` içinde
* Tip spesifik, gelecekte ekstra fonksiyon gerekirse -> `IStudentRepository`, `StudentRepository`

Bu sana şu avantajı verir:

* Yarın “öğrenciyi numarasına göre bul” gibi özel bir şey istersen:

  * `IStudentRepository` içine `Student FindByNumber(string no);`
  * `StudentRepository` içine bunun implementasyonunu yazarsın.
  * Ama genel Add/Delete işlerine dokunmazsın.

Bu, gerçek dünyadaki Repository Pattern’in aynısıdır.

---

## 4. Service Katmanı (Services/StudentAcademicService.cs)

```csharp
public static class StudentAcademicService
{
    public static double CalculateAverageScore(Student student)
    {
        if (student == null) return 0;
        if (student.GradeRecords == null || student.GradeRecords.Count == 0)
            return 0;

        double sum = 0;
        foreach (var gr in student.GradeRecords)
        {
            sum += gr.Score;
        }

        double avg = (double)sum / student.GradeRecords.Count;
        return avg;
    }

    public static bool PassedClass(Student student, int threshold = 50)
    {
        double avg = CalculateAverageScore(student);
        return avg >= threshold;
    }
}
```

Bu sınıf ne yapıyor?

* Öğrencinin not ortalamasını hesaplıyor
* Geçip geçmediğine karar veriyor

Neden ayrı bir service sınıfında?

* Bu tamamen iş kuralı.
* Not hesaplama formülü yarın değişirse sadece burayı değiştirirsin.
* `Program.cs` de, repository’ler de bozulmaz.
* Tek sorumluluk prensibi (Single Responsibility Principle) uygulanmış.

Statik olmasının sebebi:
Durum tutmuyor. (state yok, sadece hesaplıyor)
Bu yüzden `new StudentAcademicService()` diye instance oluşturmaya gerek yok. Direkt `StudentAcademicService.PassedClass(student)` diyorsun.

---

## 5. Program.cs (Presentation / UI kısmı)

`Program.cs` bir konsol menüsü. İçinde şu pattern var:

* Main içinde repository instance’ları yaratılıyor:

  ```csharp
  ITeacherRepository teacherRepo = new TeacherRepository();
  ICourseRepository courseRepo = new CourseRepository();
  IStudentRepository studentRepo = new StudentRepository();
  IEnrollmentRepository enrollmentRepo = new EnrollmentRepository();
  IGradeRecordRepository gradeRepo = new GradeRecordRepository();
  ```

  DİKKAT: burada interface değişken + concrete class ataması var.
  Örnek:

  ```csharp
  IStudentRepository studentRepo = new StudentRepository();
  ```

  Bu çok önemli bir alışkanlık. Neden?

  * Yarın `StudentRepository` artık SQL’e bağlanan bir repository olur.
  * Ya da dosyaya yazan bir repository olur.
  * Main kodunu değiştirmeden sadece `new SqlStudentRepository()` dersin.
  * Geri kalan tüm kod aynen çalışır çünkü interface aynı.

* Kullanıcıya bir menü gösteriliyor (`Console.WriteLine(...)`).

* Kullanıcı seçim yapıyor.

* Bir `switch` var ve her case bir static metoda gidiyor:

  ```csharp
  case "1":
      AddTeacher(teacherRepo);
      break;
  case "2":
      CreateCourse(teacherRepo, courseRepo);
      break;
  ...
  case "10":
      CheckStudentPassStatus(studentRepo);
      break;
  ```

### Buradaki static fonksiyonlar ne işe yarıyor?

Örnek: `AddTeacher(...)`, `AddStudent(...)`, `EnrollStudentToCourse(...)`, `EnterGrade(...)`, `ListStudents(...)`, `ShowStudentGrades(...)` vs.

Bunların ortak özelliği:

* Hepsi `static` tanımlı.
* Hepsi doğrudan kullanıcıyla konuşuyor (Console.ReadLine / Console.WriteLine).
* Hepsi repo kullanarak model yaratıyor/güncelliyor/listeleme yapıyor.

Neden sınıf içinde static fonksiyon yapıldı?

* Bu bir console app, dependency injection yok, ekstra service class’ları ile uğraşılmamış.
* `Program` sınıfının içinde yardımcı “ekran akışı” fonksiyonları gibi düşünülmüş.
* `static` olduğu için `var p = new Program()` demene gerek yok. Direkt `AddStudent(studentRepo)` diyorsun.

Yani `Program` içindeki static fonksiyonlar aslında “use case” akışını temsil ediyor:

* Öğretmen eklemek
* Ders açmak ve öğretmene bağlamak
* Öğrenciyi derse kaydetmek (Enrollment)
* Öğrenciye not vermek (GradeRecord)
* Listeleme, raporlama
* Öğrencinin ortalamasını gösterip ‘GEÇTİ/KALDI’ yazmak (burada `StudentAcademicService` çağrılıyor)

Bunlar controller gibi davranıyor diyebiliriz (MVC’de Controller ne yapıyorsa burada onu yapıyor).

---

## 6. Bu projede mantıksız / hatalı bir yapı var mı?

Ana fikir olarak mimari doğru:

* Modeller ayrı
* Repository pattern var
* Service var
* UI ayrı
  Bu çok güzel.

Küçük noktalar (bunlar öğrenme amaçlı, patlatmaz ama bilmek iyi):

1. **Repository.Update() şu anda eksik**

   ```csharp
   var existing = _table.FirstOrDefault(x => x.Id == entity.Id);
   if (existing != null)
   {
       entity.UpdatedDate = DateTime.Now;
       // normalde existing'in alanlarını entity'den kopyalamalıyız
   }
   ```

   Şu haliyle liste içindeki elemanı gerçekten güncellemiyor. Ama konsept doğru.

2. **Repository layer static List<T> kullanıyor.**

   * Bu demek ki her tip için tek bir tablo paylaşılıyor (in-memory). Demo için ok ama gerçek hayatta DB olur.

3. **Navigation property’lerin (Student.Enrollments, Course.Enrollments vs.) elle senkron tutulması gerekiyor**

   * `EnrollStudentToCourse` içinde hem repositaory’ye `enrollment` ekleniyor hem `student.Enrollments.Add(enrollment)` hem `course.Enrollments.Add(enrollment)` yapılıyor.
   * Bunu unutursan ilişkiler kopar. Gerçek hayatta bunu ORM (Entity Framework gibi) otomatik yapar.

4. `StudentAcademicService` ortalamayı Student içindeki notlardan hesaplıyor. Başka bir yerde buna benzer bir hesap yapmıyoruz, merkezi tek yer orası → bu iyi tasarım.

Genel olarak mimari mantıksız değil, aksine ders için baya temiz.

---

## 7. Bu yapıyı başka projede nasıl kullanırsın?

Sen kendi projen için şunu yapabilirsin:

1. **Models/**

   * Ortak alanları `BaseEntity` içine koy.
   * Her tabloyu/modeli (`User`, `Product`, `Order` vs.) bundan türet.

2. **DataAccess/**

   * `IRepository<T>` tanımla (Add, Update, Delete, GetAll, GetById).
   * `Repository<T>` yaz, içini ister in-memory yap ister gerçek DB bağlantısı koy.
   * Her entity için:

     * `IUserRepository : IRepository<User>`
     * `UserRepository : Repository<User>, IUserRepository`

   Neden böyle?
   Çünkü bazı entity’lere özel ihtiyaç doğacak. Örn:

   * `IUserRepository` içine `User GetByUsername(string username)` ekleyebilirsin.
   * `UserRepository` içinde bunu gerçekleştirirsin.

3. **Services/**

   * İş kurallarını buraya koy.
   * Mesela “kullanıcı şifre doğrulama”, “sipariş toplam fiyatı hesaplama”, “kullanıcı premium mu?” gibi hesaplar bu katmanda.

4. **Program.cs / Controller / API Layer**

   * Kullanıcıdan input al.
   * Repository’lere ve service’lere çağrı yap.
   * Sonucu göster / döndür.

Bunu yaptığında elinde şu olur:

* Değiştirilebilir/veri kaynağından bağımsız bir iş mantığı
* Tek sorumluluklar
* Okuması kolay akış

---

## 8. Özetle ana cevap

* **Mimari:** Katmanlı yapı var (Models → DataAccess → Services → Program.cs UI).
* **Generic yapı (`Repository<T>`):**

  * Tüm entity’ler için ortak CRUD mantığını tek yerde toplamak için kullanılmış.
  * `where T : BaseEntity` kısıtı sayesinde kod bu entity’lerin sahip olduğu ortak alanlara güveniyor (Id, IsActive, vs.).
  * Soft delete, Id atama gibi tekrar eden işleri merkezileştiriyor.
* **Interface’ler (`IRepository<T>`, `IStudentRepository`, ...):**

  * Üst katmanları (Program.cs gibi) implementation’dan koparmak için var → gevşek bağlılık (loose coupling).
  * Yarın veri kaynağını değiştirsen UI kodunu değiştirmezsin.
  * Ayrıca ileride tipe özel metotlar eklemek için bir hook sağlıyor.
* **Program.cs’deki static fonksiyonlar:**

  * Her biri bir “use case” (öğretmen ekle, ders aç, not gir, ortalama göster).
  * Konsoldan input alıyor, repository’lerle konuşuyor, servislerden hesap istiyor.
  * `static` olmaları sadece pratiklik: instance oluşturmadan çağırabilelim diye.
* **Service katmanı (`StudentAcademicService`):**

  * İş kuralını modelden ayırıyor (ortalama hesaplama, geçti mi?).
  * Bu kuralı merkezileştiriyor ki tekrar etme / yanlış hesaplama olmasın.

Sen bu projeye bakarak rahat rahat yeni bir “küçük yönetim sistemi” kurabilirsin (örneğin Kütüphane Yönetimi, Sipariş Yönetimi, Personel Yönetimi). Yapacağın şey bu yapıyı kopyalayıp model isimlerini değiştirip kuralları yazmak.

		 
		 */
    }
}
