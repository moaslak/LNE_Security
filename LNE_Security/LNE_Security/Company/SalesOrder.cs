using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security;

public class SalesOrder
{
    const double VATS = 1.25;

    public List<OrderLine> OrderLines = new List<OrderLine>();

    public UInt32 OrderID { get; set; }

    public double TotalPrice { get; set; }

    public DateTime OrderTime { get; set; }
    public DateTime CompletionTime { get; set; }

    public OrderLine? orderLine { get; set; }

    /// <summary>
    /// Adds the VATS to the total price
    /// </summary>
    /// <param name="VATS"></param>
    /// <param name="TotalPrice"></param>
    /// <returns>double</returns>
    public double CalculateVATS(double VATS, double TotalPrice)
    {
        return TotalPrice * VATS;
    }

    /// <summary>
    /// Calculates the total price from ALL the orderlines.
    /// </summary>
    /// <param name="orderLines"></param>
    /// <returns>double</returns>
    public double CalculateTotalPrice(List<OrderLine> orderLines)
    {
        if(orderLines == null)
            return 0;
        
        double totalPrice = 0;
        foreach(OrderLine orderLine in orderLines)
        {
            if (orderLine.Product.SalesPrice < 0)
                return 0;
            else
                totalPrice = totalPrice + orderLine.Product.SalesPrice * orderLine.Quantity;
        }
        return totalPrice;
    }        
}
