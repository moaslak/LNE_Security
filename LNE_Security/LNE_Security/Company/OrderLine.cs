using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class OrderLine
    {

        public Product Product
        {
            get => default;
            set
            {
            }
        }

        public UInt16 Quantity
        {
            get => default;
            set
            {
            }
        }

        public double Price
        {
            get => default;
            set
            {
            }
        }

        public double CalculateLinePrice()
        {
            throw new System.NotImplementedException();
        }
    }
}