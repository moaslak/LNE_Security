using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security;

public class ContactInfo
{
    public Address Address = new Address();
    public UInt16 ContactInfoID { get; set; }

    public UInt16 PersonId { get; set; }
    public UInt16 AddressId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber {get; set; }
    public string FullName 
    {
        get
        {
            return FirstName + " " + LastName;
        }
        set
        {
            FirstName = value;
            LastName = value;
        }
    }
    /// <summary>
    /// Set the full address for contact info. Concats properties.
    /// </summary>
    public string FullAddress 
    {
        get
        {

            return Address.StreetName + "," + Address.HouseNumber + "," + Address.City + "," + Address.ZipCode + "," + Address.Country;
        }
        set
        {
            string zipCode = Address.ZipCode.ToString();
            Address.StreetName = value;
            Address.HouseNumber = value;
            Address.City = value;
            zipCode = value;
            Address.Country = value;
        }
    }



}
