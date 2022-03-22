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
    }
}