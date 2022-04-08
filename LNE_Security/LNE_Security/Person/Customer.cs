using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace LNE_Security;

public class Customer : Person
{
    private Person _person { get; set; }

    public UInt16 CID { get; set; }
    

    public override Person DeletePerson(ContactInfo contactInfo, Database database, Address address)
    {
        //database = new Database();
        contactInfo = new ContactInfo();
        address = new Address();
        _person.DeletePerson(contactInfo, database, address);
        return null;
    }

    public override Person GetPerson(ContactInfo contactInfo)
    {
        contactInfo = new ContactInfo();
        return _person;
    }

    public override Person UpdatePerson(ContactInfo contactInfo, Database database, Address address)
    {
        contactInfo = new ContactInfo();
        //database = new Database();
        address = new Address();
        return _person;
    }

    public string CreateFullName(string FirstName, string LastName)
    {
        return FirstName + " " + LastName;
    }
}
