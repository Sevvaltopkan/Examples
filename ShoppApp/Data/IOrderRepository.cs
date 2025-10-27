using System;
using System.Collections.Generic;
using ShopApp.Models;

namespace ShopApp.Data
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetOrdersAboveTotal(double minTotal);
        IEnumerable<Order> GetOrdersByDate(DateTime day);
    }
}
