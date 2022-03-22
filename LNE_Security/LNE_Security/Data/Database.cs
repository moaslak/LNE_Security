using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security
{
    public class Database
    {
        public static Company Instance { get; private set; }

        static Company()
        {
            Instance = new Company();
        }
    }
}