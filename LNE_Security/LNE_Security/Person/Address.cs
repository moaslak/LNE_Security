using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security;

public class Address
{
    public UInt16 AddressID { get; set; }
    public string StreetName { get; set; }
    public string HouseNumber { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }

    public string AddressLine(Person person)
    {
        return StreetName + " " + HouseNumber + ", " + ZipCode + " " + City + ", " + Country; 
    }

    public string commaSeperatedAddress(Person person)
    {
        string seperatedAddress = StreetName + ", " + HouseNumber + ", " + ZipCode + ", " + City + ", " + Country;
        return seperatedAddress;
    }
}
