using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public abstract class Person
    {
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
            ContactInfo = contactInfom;
            Database = database;
            Address = address;
            return null;
        }

        // overload
        public virtual Person DeletePerson()
        {
            throw new NotImplementedException();
        }

        public virtual Person UpdatePerson()
        {
            throw new System.NotImplementedException();
        }

        public virtual Person GetPerson()
        {
            throw new System.NotImplementedException();
        }
    }
}