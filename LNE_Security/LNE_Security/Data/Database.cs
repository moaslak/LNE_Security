using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNE_Security
{
    public partial class Company
    {
        public static Company Instance { get; private set; }

        static Company()
        {
            Instance = new Company();
        }
    }
}
