using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class OrderLine
    {

        public Product? Product { get; set; }

        public UInt16 Quantity { get; set; }

        public double Price { get; set; }   
        //public double CalculateLinePrice()
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}