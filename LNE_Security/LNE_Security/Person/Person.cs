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
            }
        }

        public Database Database
        {
            get => default;
            set
            {
            }
        }

        public Address Address
        {
            get => default;
            set
            {
            }
        }

        public UInt16 ID
        {
            get => default;
            set
            {
            }
        }

        public Enum Type
        {
            get => default;
            set
            {
            }
        }

        public virtual Person NewPerson()
        {
            throw new System.NotImplementedException();
        }

        public virtual Person DeletePerson()
        {
            throw new System.NotImplementedException();
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