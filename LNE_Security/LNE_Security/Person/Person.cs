using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public abstract class Person
    {
        Address address1 = new Address();
        ContactInfo contactInfo1 = new ContactInfo();
        public ContactInfo ContactInfo
        {
            get => default;
            set
            {
                ContactInfo = value;
            }
        }

        public Database Database
        {
            get => default;
            set
            {
                Database = value;
            }
        }

        public Address Address
        {
            get => default;
            set
            {
                Address = value;
            }
        }

        public UInt16 ID
        {
            get => default;
            set
            {
                ID = value;
            }
        }

        public Enum Type
        {
            get => default;
            set
            {
                Type = value;
            }
        }

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

        public virtual Person DeletePerson(ContactInfo contactInfom,
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
        public virtual Person UpdatePerson(ContactInfo contactInfo,
            Database database, Address address)

        {
            ContactInfo = contactInfo;
            Database = database;
            Address = address;
            return this;

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
}