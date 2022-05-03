using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security;

public class OrderLine
{
    public UInt16 OLID { get; set; } //orderLine ID
    public double Quantity { get; set; }

    public Product Product = new Product();
    public UInt32 PID { get; set; }
    public UInt32 OrderID { get; set; }
    public enum States { Created, Confirmed, Packed, Closed, Canceled, Incomplete, Error };
    public States State { get; set; }

    public string pickedBy { get; set; }

    public double CalculateLinePrice(Product product)
    {
        if(product == null || product.SalesPrice < 0)
            return 0;
        else
            return product.SalesPrice * Quantity;
    }
}
