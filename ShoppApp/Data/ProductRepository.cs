using System.Collections.Generic;
using System.Linq;
using ShopApp.Models;

namespace ShopApp.Data
{
    public class ProductRepository : InMemoryRepository<Product>, IProductRepository
    {
        public IEnumerable<Product> SearchByName(string text)
        {
            text = text.ToLower();
            return _items.Where(p => p.Name.ToLower().Contains(text));
        }

        public IEnumerable<Product> GetInStockOnly()
        {
            return _items.Where(p => p.Stock > 0);
        }
    }
}
