using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNE_Security;

partial class Database
{
    public int ID { get; set; }
    public string CompanyName { get; set; }
    public string RoadName { get; set; }
    public int HouseNumber { get; set; }
    public int ZipCode { get; set; }
    public string Country { get; set; }
    public Decimal Currency { get; set; }

}
