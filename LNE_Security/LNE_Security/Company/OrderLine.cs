using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security;

public class OrderLine
{

    public double Quantity { get; set; }

    public Product Product = new Product();

    public double CalculateLinePrice(Product product)
    {
        if(product == null || product.SalesPrice < 0)
            return 0;
        else
            return product.SalesPrice * Quantity;
    }
}
