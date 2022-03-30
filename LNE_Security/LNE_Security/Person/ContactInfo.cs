using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security;

public class ContactInfo
{
    Address address = new Address();
    public UInt16 ID { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FullName 
    {
        get
        {
            return FirstName + " " + LastName + " ";
        }
        set
        {
            FirstName = value;
            LastName = value;
        }
    }
    public string FullAddress 
    {
        get
        {

            return address.StreetName + "" + address.HouseNumber + "" + address.City + "" + address.ZipCode + "" + address.Country;
        }
        set
        {
            string zipCode = address.ZipCode.ToString();
            address.StreetName = value;
            address.HouseNumber = value;
            address.City = value;
            zipCode = value;
            address.Country = value;
        }
    }
}
