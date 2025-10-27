using System;

namespace ShopApp.Models
{
    public class Product : BaseEntity
    {
        private string _name;
        private double _price;
        private int _stock;

        public Product(string name, double price, int stock) : base()
        {
            Name = name;
            Price = price;
            Stock = stock;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _name = "Unnamed Product";
                else
                    _name = value.Trim();
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                // Fiyat eksi olamaz, en az 0 olsun
                if (value < 0)
                    _price = 0;
                else
                    _price = value;
            }
        }

        public int Stock
        {
            get { return _stock; }
            set
            {
                // Stok eksi olamaz, eksi gelirse 0 yap
                if (value < 0)
                    _stock = 0;
                else
                    _stock = value;
            }
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} | Price: {Price} | Stock: {Stock} | Created: {CreatedDate}";
        }
    }
}
