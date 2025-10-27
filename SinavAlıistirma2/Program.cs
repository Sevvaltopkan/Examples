using System;
using System.Collections.Generic;
using SinavAlıistirma2.Models;

namespace SinavAlıistirma2
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Uygulama çalıştığı sürece tutulacak veriler
            List<Members> membersList = new List<Members>();
            List<Category> categoryList = new List<Category>();
            List<Post> postList = new List<Post>();
            List<Comment> commentList = new List<Comment>();

            // Demo amaçlı başlangıç kategorisi oluşturalım ki post açarken kullanabilelim
            categoryList.Add(new Category
            {
                Id = Guid.NewGuid(),
                Name = "Genel",
                Description = "Genel konular",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsActive = true
            });

            while (true)
            {
                Console.WriteLine("******** BLOG SİSTEMİ ********");
                Console.WriteLine("1 - Üye Kaydı Oluştur");
                Console.WriteLine("2 - Post Oluştur");
                Console.WriteLine("3 - Postları Listele");
                Console.WriteLine("4 - Yorum Yap");
                Console.WriteLine("5 - Yorumları Listele");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçim: ");

                string giris = Console.ReadLine();
                Console.WriteLine();

                if (giris == "0")
                {
                    Console.WriteLine("Program kapatılıyor...");
                    break;
                }

                switch (giris)
                {
                    case "1":
                        // Üye kaydı
                        Members newMember = new Members();
                        newMember.Id = Guid.NewGuid();
                        newMember.CreatedDate = DateTime.Now;
                        newMember.UpdatedDate = DateTime.Now;
                        newMember.IsActive = true;

                        Console.Write("Kullanıcı adı: ");
                        newMember.UserName = Console.ReadLine();

                        Console.Write("Email: ");
                        newMember.Email = Console.ReadLine();

                        Console.Write("Şifre: ");
                        newMember.Password = Console.ReadLine();

                        Console.Write("Telefon: ");
                        newMember.PhoneNumber = Console.ReadLine();

                        membersList.Add(newMember);

                        Console.WriteLine("➡ Üyelik oluşturuldu. Üye Id: " + newMember.Id);
                        Console.WriteLine();
                        break;

                    case "2":
                        // Post oluşturma
                        if (membersList.Count == 0)
                        {
                            Console.WriteLine("Önce üye kaydı oluşturmalısınız (seçenek 1).");
                            Console.WriteLine();
                            break;
                        }

                        Console.WriteLine("Post eklemek için hangi üye ile giriş yapıyorsunuz?");
                        for (int i = 0; i < membersList.Count; i++)
                        {
                            Console.WriteLine($"{i + 1} - {membersList[i].UserName} ({membersList[i].Id})");
                        }
                        Console.Write("Seç: ");
                        int memberSecim;
                        if (!int.TryParse(Console.ReadLine(), out memberSecim) ||
                            memberSecim < 1 ||
                            memberSecim > membersList.Count)
                        {
                            Console.WriteLine("Geçersiz seçim.\n");
                            break;
                        }
                        Members aktifUye = membersList[memberSecim - 1];

                        // Kategori seçtir
                        Console.WriteLine("\nKategori seçiniz:");
                        for (int i = 0; i < categoryList.Count; i++)
                        {
                            Console.WriteLine($"{i + 1} - {categoryList[i].Name} ({categoryList[i].Id})");
                        }
                        Console.Write("Kategori: ");
                        int catSecim;
                        if (!int.TryParse(Console.ReadLine(), out catSecim) ||
                            catSecim < 1 ||
                            catSecim > categoryList.Count)
                        {
                            Console.WriteLine("Geçersiz kategori.\n");
                            break;
                        }
                        Category secilenKategori = categoryList[catSecim - 1];

                        // Yeni post bilgilerini al
                        Post newPost = new Post();
                        newPost.Id = Guid.NewGuid();
                        newPost.CreatedDate = DateTime.Now;
                        newPost.UpdatedDate = DateTime.Now;
                        newPost.IsActive = true;

                        newPost.CategoryId = secilenKategori.Id;
                        newPost.Category = secilenKategori;

                        Console.Write("Post Başlığı: ");
                        newPost.Title = Console.ReadLine();

                        Console.Write("Post İçeriği: ");
                        newPost.Content = Console.ReadLine();

                        postList.Add(newPost);

                        // kategori içindeki postlar listesi tutuluyorsa oraya da ekleyelim
                        if (secilenKategori.Posts == null)
                        {
                            secilenKategori.Posts = new List<Post>();
                        }
                        secilenKategori.Posts.Add(newPost);

                        // İsteğe bağlı: üyenin yazar bilgisi
                        if (aktifUye.Author == null)
                        {
                            aktifUye.Author = new Author
                            {
                                Id = Guid.NewGuid(),
                                CreatedDate = DateTime.Now,
                                UpdatedDate = DateTime.Now,
                                IsActive = true,
                                FirstName = aktifUye.UserName, // basit atama
                                LastName = "",
                                Members = aktifUye
                            };
                        }

                        Console.WriteLine("\n➡ Post oluşturuldu.");
                        Console.WriteLine("Post Id: " + newPost.Id);
                        Console.WriteLine("Yazar: " + aktifUye.UserName);
                        Console.WriteLine("Kategori: " + secilenKategori.Name);
                        Console.WriteLine();
                        break;

                    case "3":
                        // Postları listele
                        if (postList.Count == 0)
                        {
                            Console.WriteLine("Henüz post yok.\n");
                            break;
                        }

                        Console.WriteLine("=== TÜM POSTLAR ===");
                        foreach (var p in postList)
                        {
                            Console.WriteLine("Id: " + p.Id);
                            Console.WriteLine("Başlık: " + p.Title);
                            Console.WriteLine("İçerik: " + p.Content);
                            Console.WriteLine("Kategori: " + (p.Category != null ? p.Category.Name : "-"));
                            Console.WriteLine("Oluşturulma: " + p.CreatedDate);
                            Console.WriteLine("-------------------------");
                        }
                        Console.WriteLine();
                        break;

                    case "4":
                        // Yorum yap
                        if (membersList.Count == 0 || postList.Count == 0)
                        {
                            Console.WriteLine("Yorum yapmak için en az 1 üye ve 1 post olmalı.\n");
                            break;
                        }

                        Console.WriteLine("Yorum yapacak üyeyi seçin:");
                        for (int i = 0; i < membersList.Count; i++)
                        {
                            Console.WriteLine($"{i + 1} - {membersList[i].UserName} ({membersList[i].Id})");
                        }
                        Console.Write("Seç: ");
                        int yorumYapanIndex;
                        if (!int.TryParse(Console.ReadLine(), out yorumYapanIndex) ||
                            yorumYapanIndex < 1 ||
                            yorumYapanIndex > membersList.Count)
                        {
                            Console.WriteLine("Geçersiz seçim.\n");
                            break;
                        }
                        Members yorumcu = membersList[yorumYapanIndex - 1];

                        Console.WriteLine("\nYorum yapılacak postu seçin:");
                        for (int i = 0; i < postList.Count; i++)
                        {
                            Console.WriteLine($"{i + 1} - {postList[i].Title} ({postList[i].Id})");
                        }
                        Console.Write("Seç: ");
                        int yorumPostIndex;
                        if (!int.TryParse(Console.ReadLine(), out yorumPostIndex) ||
                            yorumPostIndex < 1 ||
                            yorumPostIndex > postList.Count)
                        {
                            Console.WriteLine("Geçersiz seçim.\n");
                            break;
                        }
                        Post yorumYapilacakPost = postList[yorumPostIndex - 1];

                        Console.Write("Yorum içeriği: ");
                        string yorumText = Console.ReadLine();

                        Comment yeniYorum = new Comment();
                        yeniYorum.Id = Guid.NewGuid();
                        yeniYorum.CreatedDate = DateTime.Now;
                        yeniYorum.UpdatedDate = DateTime.Now;
                        yeniYorum.IsActive = true;

                        yeniYorum.PostId = yorumYapilacakPost.Id;
                        yeniYorum.MemberId = yorumcu.Id;
                        yeniYorum.Post = yorumYapilacakPost;
                        yeniYorum.Member = yorumcu;
                        yeniYorum.Content = yorumText;

                        commentList.Add(yeniYorum);

                        Console.WriteLine("➡ Yorum eklendi.\n");
                        break;

                    case "5":
                        // Yorumları listele
                        if (commentList.Count == 0)
                        {
                            Console.WriteLine("Henüz yorum yok.\n");
                            break;
                        }

                        Console.WriteLine("=== YORUMLAR ===");
                        foreach (var c in commentList)
                        {
                            Console.WriteLine("Yorum Id: " + c.Id);
                            Console.WriteLine("Post Id: " + c.PostId);
                            Console.WriteLine("Üye: " + (c.Member != null ? c.Member.UserName : "-"));
                            Console.WriteLine("Yorum: " + c.Content);
                            Console.WriteLine("Tarih: " + c.CreatedDate);
                            Console.WriteLine("-------------------------");
                        }
                        Console.WriteLine();
                        break;

                    default:
                        Console.WriteLine("Geçersiz seçim.\n");
                        break;
                }
            }
        }
    }
}
