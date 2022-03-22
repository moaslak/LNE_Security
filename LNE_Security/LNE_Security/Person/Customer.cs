using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class Customer : Person
    {
        public override Person NewPerson(ContactInfo contactInfo, Database database, Address address)
        {
            
            contactInfo = new ContactInfo();
            address = new Address();
            database = new Database();
            return this; 
        }

        public override Person DeletePerson()
        {
            throw new System.NotImplementedException();
        }

        public override Person GetPerson()
        {
            throw new System.NotImplementedException();
        }

        public override Person UpdatePerson()
        {
            throw new System.NotImplementedException();
        }
    }
}