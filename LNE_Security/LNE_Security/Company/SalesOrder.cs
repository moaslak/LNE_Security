using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class SalesOrder
    {
        const double VATS = 1.25;

        public List<OrderLine> OrderLines = new List<OrderLine>();

        public UInt32 OrderID { get; set; }

        public double TotalPrice { get; set; }

        public DateTime OrderTime { get; set; }
        public DateTime CompletionTime { get; set; }

        public OrderLine? orderLine { get; set; }
        public double CalculateVATS(double VATS, double TotalPrice)
        {
            return TotalPrice * VATS;
        }

        public double CalculateTotalPrice(List<OrderLine> orderLines)
        {
            double totalPrice = 0;
            foreach(OrderLine orderLine in orderLines)
            {
                totalPrice = totalPrice + orderLine.Price * orderLine.Quantity;
            }
            return totalPrice;
        }        
    }
}