using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security;

public abstract class Person
{
    //Address address1 = new Address();
    //ContactInfo contactInfo1 = new ContactInfo();
    //public ContactInfo? ContactInfo = new ContactInfo();

    public Database Database
    {
        get => default;
        set
        {
            Database = value;
        }
    }

    public ContactInfo ContactInfo = new ContactInfo();
    public UInt16 ContactInfoID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string FullName { get; set; }

    public Address Address { get; set; }
    public string FullAddress { get; set; }

    public UInt16 ID { get; set; }

    public enum Types { Customer, Employee }
    public Types type  { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public UInt16 CompanyID { get; set; }
    
    public string CreateFullName(string FirstName, string LastName)
    {
        return FirstName + " " + LastName;
    }


}
