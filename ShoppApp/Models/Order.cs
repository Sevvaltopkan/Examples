using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.Models
{
    public class Order : BaseEntity
    {
        private readonly List<Product> _products;

        public Order() : base()
        {
            OrderDate = DateTime.Now;
            _products = new List<Product>();
        }

        public DateTime OrderDate { get; private set; }

        // IReadOnlyList ile dışarıya sadece okunabilir liste gösteriyoruz
        public IReadOnlyList<Product> Products => _products.AsReadOnly();

        public void AddProduct(Product product)
        {
            if (product == null) return;
            _products.Add(product);
        }

        public void RemoveProduct(Product product)
        {
            if (product == null) return;
            _products.Remove(product);
        }

        public double CalculateTotal()
        {
            // Tüm ürünlerin fiyatlarını topla
            return _products.Sum(p => p.Price);
        }

        public override string ToString()
        {
            // Bu ToString() DisplayOrderSummary gibi davranacak.
            var sb = new StringBuilder();

            sb.AppendLine($"Order [{Id}] Date: {OrderDate}");
            sb.AppendLine("Products:");

            if (_products.Count == 0)
            {
                sb.AppendLine("- (no products)");
            }
            else
            {
                foreach (var p in _products)
                {
                    sb.AppendLine($"  - {p.Name} | {p.Price}₺");
                }
            }

            sb.AppendLine($"TOTAL: {CalculateTotal()}₺");

            return sb.ToString();
        }
    }
}
