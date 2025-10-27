using System;
using ShopApp.Models;
using ShopApp.Data;
using System.Linq;

namespace ShopApp
{
    public class Program
    {
        static IProductRepository productRepo = new ProductRepository();
        static IOrderRepository orderRepo = new OrderRepository();

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("===== SHOP SYSTEM =====");
                Console.WriteLine("1 - Yeni Ürün Ekle");
                Console.WriteLine("2 - Ürünleri Listele");
                Console.WriteLine("3 - Yeni Sipariş Oluştur");
                Console.WriteLine("4 - Siparişe Ürün Ekle");
                Console.WriteLine("5 - Tüm Siparişleri Listele");
                Console.WriteLine("0 - Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine();
                Console.WriteLine();

                switch (secim)
                {
                    case "1":
                        UrunEkle();
                        break;
                    case "2":
                        UrunleriListele();
                        break;
                    case "3":
                        SiparisOlustur();
                        break;
                    case "4":
                        SipariseUrunEkle();
                        break;
                    case "5":
                        SiparisleriListele();
                        break;
                    case "0":
                        Console.WriteLine("Program kapatılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim!");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine("Devam etmek için Enter'a basın...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        static void UrunEkle()
        {
            Console.Write("Ürün adı: ");
            string name = Console.ReadLine();

            Console.Write("Fiyat: ");
            double.TryParse(Console.ReadLine(), out double price);

            Console.Write("Stok: ");
            int.TryParse(Console.ReadLine(), out int stock);

            Product p = new Product(name, price, stock);
            productRepo.Add(p);

            Console.WriteLine("Ürün eklendi:");
            Console.WriteLine(p.ToString());
        }

        static void UrunleriListele()
        {
            Console.WriteLine("=== ÜRÜNLER ===");
            foreach (var p in productRepo.GetAll())
            {
                Console.WriteLine(p.ToString());
            }
        }

        static void SiparisOlustur()
        {
            Order o = new Order();
            orderRepo.Add(o);

            Console.WriteLine("Sipariş oluşturuldu:");
            Console.WriteLine(o.ToString());
        }

        static void SipariseUrunEkle()
        {
            // Sipariş seç
            Console.WriteLine("Hangi siparişe ürün eklemek istiyorsun? (Order Id gir)");
            foreach (var o in orderRepo.GetAll())
            {
                Console.WriteLine($"{o.Id} | Toplam: {o.CalculateTotal()}₺ | {o.OrderDate}");
            }
            Console.Write("Order Id: ");
            string orderIdStr = Console.ReadLine();
            Guid.TryParse(orderIdStr, out Guid orderId);

            var order = orderRepo.GetById(orderId);
            if (order == null)
            {
                Console.WriteLine("Sipariş bulunamadı!");
                return;
            }

            // Ürün seç
            Console.WriteLine("Hangi ürünü eklemek istiyorsun? (Product Id gir)");
            foreach (var p in productRepo.GetAll())
            {
                Console.WriteLine($"{p.Id} | {p.Name} | {p.Price}₺ | Stok:{p.Stock}");
            }
            Console.Write("Product Id: ");
            string productIdStr = Console.ReadLine();
            Guid.TryParse(productIdStr, out Guid productId);

            var product = productRepo.GetById(productId);
            if (product == null)
            {
                Console.WriteLine("Ürün bulunamadı!");
                return;
            }

            order.AddProduct(product);

            Console.WriteLine("Ürün siparişe eklendi.");
            Console.WriteLine(order.ToString());
        }

        static void SiparisleriListele()
        {
            Console.WriteLine("=== SİPARİŞLER ===");

            foreach (var o in orderRepo.GetAll())
            {
                Console.WriteLine(o.ToString());
            }
        }
    }
}
