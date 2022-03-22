using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class Address
    {
        public UInt16 ID { get; set; } //TODO: Er denne nødvendig?
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public UInt16 ZipCode { get; set; }
        public string Country { get; set; }
    }
}