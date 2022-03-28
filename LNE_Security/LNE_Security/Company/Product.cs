using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public enum UnitPrice
    {
        Hour,
        Meter
    }
    public class Product
    {
        public int ProductNumber { get; set; }
        public string? ProductName { get; set; }

        public float SalesPrice { get; set; }
        public float CompanyPrice { get; set; }
        public float AmountInStorage { get; set; }
        public UInt32 ID { get; set; }

        public string? Description { get; set; }

        public void CalculateProfit()
        {
            Console.Write($"Indtast:{UnitPrice.Hour} {UnitPrice.Meter}");
            var unitPrice = Convert.ToDouble(Console.ReadLine());
            Console.Write("Vælg mængden: ");
            var amount = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine(unitPrice * amount);
            var percent = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine((percent / SalesPrice) * 100);
        }

        public List<Product>? products = new List<Product>();


    }
}