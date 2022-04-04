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
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string FullName { get; set; }

    public Address Address { get; set; }

    public UInt16 ID { get; set; } // TODO: Lav id generator

    public enum Types { Customer, Employee }
    public Types type  { get; set; }

    public string Email { get; set; }

    public List<string> PhoneNumbers { get; set; }

    /*public Person(ContactInfo contactInfo)
    {
        this.FirstName = contactInfo.FirstName;
        this.LastName = contactInfo.LastName;
        this.FullName = contactInfo.FullName;
        this.Address = contactInfo.Address;
        this.PhoneNumbers = contactInfo.PhoneNumber;
        this.Email = contactInfo.Email;
    }*/
    public virtual Person NewPerson(
        ContactInfo contactInfo, Database database,
        Address address)
    { 
        ContactInfo = contactInfo;
        Database = database;
        Address = address;
        ID++;
        return this;
    }

    // overload
    public virtual Person NewPerson()
    {
        throw new NotImplementedException();
    }

    public virtual Person DeletePerson(ContactInfo contactInfo,
        Database database, Address address)
    {
        ContactInfo = contactInfo;
        Database = database;
        Address = address;
        return null;
    }

    // overload
    public virtual Person DeletePerson()
    {
        throw new NotImplementedException();
    }

    // overload

    public virtual Person UpdatePerson()
    {
        throw new NotImplementedException();
    }
    public virtual Person UpdatePerson(ContactInfo contactInfo,
        Database database, Address address)

    {
        ContactInfo = contactInfo;
        Database = database;
        Address = address;
        return this;

    }

    // overload
    public virtual Person GetPerson()
    {
        throw new NotImplementedException();
    }

    public virtual Person GetPerson(ContactInfo contactInfo)
    {
        ContactInfo = contactInfo;
        return this;
    }

    public void CompbineNames()
    {
        ContactInfo contactInfo = new();
        Console.Write($"Indtast fornavn: ");
        var fornavn = Convert.ToString(Console.ReadLine());
        Console.Write($"Indtast efternavn: ");
        var efternavn = Convert.ToString(Console.ReadLine());
        Console.WriteLine($"{fornavn + efternavn}");
    }

    

}
