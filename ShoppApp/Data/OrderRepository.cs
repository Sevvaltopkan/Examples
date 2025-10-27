using System;
using System.Collections.Generic;
using System.Linq;
using ShopApp.Models;

namespace ShopApp.Data
{
    public class OrderRepository : InMemoryRepository<Order>, IOrderRepository
    {
        public IEnumerable<Order> GetOrdersAboveTotal(double minTotal)
        {
            return _items.Where(o => o.CalculateTotal() >= minTotal);
        }

        public IEnumerable<Order> GetOrdersByDate(DateTime day)
        {
            // Sadece gün karşılaştırması yapalım (saat önemli değil)
            return _items.Where(o =>
                o.CreatedDate.Date == day.Date
            );
        }
    }
}
