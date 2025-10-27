using SinavAlistirma.DataAccess;
using SinavAlistirma.Models;

namespace SinavAlistirma
{
    internal class Program
    {
        //IBaseEntity, BaseEntity,Author, Post Category, Comment 
        static void Main(string[] args)
        {
            ICustomerDal customerDal = new CustomerDal();
            for (int i = 0; i < 3; i++)
            {
                Customer c1 = new Customer();
                Console.WriteLine("Ad :");
                c1.FirstName = Console.ReadLine() ?? "";
                Console.WriteLine("Soyad");
                c1.LastName = Console.ReadLine() ?? "";
                Console.WriteLine("Doğum tarihi");
                c1.BirthDate = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("Adres: ");
                c1.Address = Console.ReadLine() ?? "";
                Console.WriteLine("E mail:");
                c1.Email = Console.ReadLine() ?? "";
                Console.WriteLine("Username");
                c1.Username = Console.ReadLine() ?? "";
                Console.WriteLine("Password:");
                c1.Password = Console.ReadLine() ?? "";
                Console.WriteLine("Phone number:");
                c1.PhoneNumber = Console.ReadLine() ?? "";

                customerDal.Create(c1);
            }

            Console.WriteLine("Müsteriler...");
            List<Customer> customerS = customerDal.GetAll();
            foreach (Customer customer in customerS)
                Console.WriteLine(customer);

            Console.WriteLine("_____________________________");

            Console.WriteLine("Uzerinde işlem yapmak istenen id");
            Guid guid = Guid.Parse(Console.ReadLine() ?? "");
            Console.WriteLine("Bu veri üzerinden hangi işlemi yapmak istersiniz");
            Console.WriteLine("1-Güncelleme\n2-Silme");
            int secim = Convert.ToInt32(Console.ReadLine());
            switch (secim)
            {
                case 1:
                    Customer? customerToUpdate = customerDal.Get(guid);
                    Console.WriteLine("Ad :");
                    customerToUpdate.FirstName = Console.ReadLine() ?? "";
                    Console.WriteLine("Soyad");
                    customerToUpdate.LastName = Console.ReadLine() ?? "";
                    Console.WriteLine("Doğum tarihi");
                    customerToUpdate.BirthDate = Convert.ToDateTime(Console.ReadLine());
                    Console.WriteLine("Adres: ");
                    customerToUpdate.Address = Console.ReadLine() ?? "";
                    Console.WriteLine("E mail:");
                    customerToUpdate.Email = Console.ReadLine() ?? "";
                    Console.WriteLine("Username");
                    customerToUpdate.Username = Console.ReadLine() ?? "";
                    Console.WriteLine("Password:");
                    customerToUpdate.Password = Console.ReadLine() ?? "";
                    Console.WriteLine("Phone number:");
                    customerToUpdate.PhoneNumber = Console.ReadLine() ?? "";
                    customerDal.Update(customerToUpdate);
                    break;
                case 2:
                    customerDal.HardDelete(guid);
                    break;

                default:
                    Console.WriteLine("Bu secim yanlıs");
                    break;
            }
            Console.Clear();
            Console.WriteLine("Güncel Müşteriler...");
            Console.WriteLine();
        }
    }
}