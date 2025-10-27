using System;
using ChatApp.Models;
using ChatApp.Data;

namespace ChatApp
{
    public class Program
    {
        static IUserRepository userRepo = new UserRepository();
        static IMessageRepository messageRepo = new MessageRepository();

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("===== MESAJLAŞMA SİSTEMİ =====");
                Console.WriteLine("1 - Kullanıcı Oluştur");
                Console.WriteLine("2 - Kullanıcıları Listele");
                Console.WriteLine("3 - Mesaj Gönder");
                Console.WriteLine("4 - Mesajları Listele");
                Console.WriteLine("5 - Belirli Kullanıcının Mesajlarını Listele");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine();
                Console.WriteLine();

                switch (secim)
                {
                    case "1":
                        KullaniciOlustur();
                        break;
                    case "2":
                        KullaniciListele();
                        break;
                    case "3":
                        MesajGonder();
                        break;
                    case "4":
                        TumMesajlariListele();
                        break;
                    case "5":
                        KullaniciMesajlariniListele();
                        break;
                    case "0":
                        Console.WriteLine("Program kapatılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        break;
                }

                Console.WriteLine();

            }
        }

        static void KullaniciOlustur()
        {
            Console.Write("First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            User u = new User(username, password, email, firstName, lastName);
            userRepo.Add(u);

            Console.WriteLine();
            Console.WriteLine("Kullanıcı oluşturuldu:");
            Console.WriteLine(u.ToString());
        }

        static void KullaniciListele()
        {
            var all = userRepo.GetAll();
            Console.WriteLine("=== KULLANICI LİSTESİ ===");

            foreach (var u in all)
            {
                Console.WriteLine(u.ToString());
            }
        }

        static void MesajGonder()
        {
            var allUsers = userRepo.GetAll();

            // en az iki user var mı
            int count = 0;
            foreach (var _ in allUsers) count++;
            if (count < 2)
            {
                Console.WriteLine("Mesaj göndermek için en az 2 kullanıcı olmalı.");
                return;
            }

            Console.WriteLine("Gönderen seç (SenderId olarak bu Guid'i kullan):");
            foreach (var u in userRepo.GetAll())
            {
                Console.WriteLine($"{u.Id} -> {u.FirstName} {u.LastName} ({u.Username})");
            }
            Console.Write("Sender Id: ");
            string senderInput = Console.ReadLine();

            Console.WriteLine("Alıcı seç (ReceiverId olarak bu Guid'i kullan):");
            foreach (var u in userRepo.GetAll())
            {
                Console.WriteLine($"{u.Id} -> {u.FirstName} {u.LastName} ({u.Username})");
            }
            Console.Write("Receiver Id: ");
            string receiverInput = Console.ReadLine();

            Console.Write("Mesaj içeriği: ");
            string content = Console.ReadLine();

            Guid.TryParse(senderInput, out Guid senderGuid);
            Guid.TryParse(receiverInput, out Guid receiverGuid);

            Message m = new Message(senderGuid, receiverGuid, content);
            messageRepo.Add(m);

            Console.WriteLine();
            Console.WriteLine("Mesaj oluşturuldu:");
            Console.WriteLine(m.ToString());
        }

        static void TumMesajlariListele()
        {
            var all = messageRepo.GetAll();

            Console.WriteLine("=== TÜM MESAJLAR ===");
            foreach (var msg in all)
            {
                Console.WriteLine(msg.ToString());
            }
        }

        static void KullaniciMesajlariniListele()
        {
            Console.Write("Mesajlarını görmek istediğin kullanıcının Id'si: ");
            string idStr = Console.ReadLine();

            if (!Guid.TryParse(idStr, out Guid userGuid))
            {
                Console.WriteLine("Geçersiz Guid.");
                return;
            }

            var list = messageRepo.GetMessagesByUser(userGuid);

            Console.WriteLine("=== KULLANICI MESAJLARI ===");
            foreach (var msg in list)
            {
                Console.WriteLine(msg.ToString());
            }
        }
    }
}
