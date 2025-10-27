using System.Collections.Generic;
using ShopApp.Models;

namespace ShopApp.Data
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> SearchByName(string text);
        IEnumerable<Product> GetInStockOnly();
    }
}
