using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class SalesOrder
    {
        public List<OrderLine> OrderLines
        {
            get => default;
            set
            {
            }
        }

        public UInt32 OrderID
        {
            get => default;
            set
            {
            }
        }

        public double TotalPrice
        {
            get => default;
            set
            {
            }
        }

        public double CalculateVATS()
        {
            throw new System.NotImplementedException();
        }

        public double CalculateTotalPrice()
        {
            throw new System.NotImplementedException();
        }
    }
}