using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class Employee : Person
    {
        public override Person NewPerson()
        {
            throw new System.NotImplementedException();
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