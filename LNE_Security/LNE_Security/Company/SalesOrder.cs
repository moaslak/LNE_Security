﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class SalesOrder
    {
        public List<OrderLine>? OrderLines { get; set; }

        public UInt32 OrderID { get; set; }

        public double TotalPrice { get; set; }

        public DateTime OrderTime { get; set; }
        public DateTime CompletionTime { get; set; }

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