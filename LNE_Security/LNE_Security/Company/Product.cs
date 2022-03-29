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

        // TODO: Navngivning af priser er misvisende
        public double SalesPrice { get; set; } // Salgspris
        public double CostPrice { get; set; } // Indkøbspris
        public double CompanyPrice { get; set; } // hvad er denne?
        public double AmountInStorage { get; set; }
        public UInt32 ID { get; set; }

        // TODO: Location af produkt?
        // TODO: Enhed?
        public string? Description { get; set; }

        /// <summary>
        /// Calculates and returns the profik from the sales and coste prices. If the return is larger then 100 %, then there is a gain, if less then 100 %, then there is a loss. 
        /// If either of the inputs are less then 0, the method will return 0
        /// </summary>
        /// <param name="SalesPrice"></param>
        /// <param name="CostPrice"></param>
        /// <returns>double</returns>
        public double CalculateProfitPercent(double SalesPrice, double CostPrice)
        {
            if(CostPrice <= 0 || SalesPrice <= 0) return 0;

            return (SalesPrice/ CostPrice)*100;    
        }

        public double CalculateProfit(double SalesPrice, double CostPrice)
        {
            return SalesPrice - CostPrice; 
        }

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