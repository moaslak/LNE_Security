using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LNE_Security
{
    public partial class Database
    {
        public static Database Instance { get; private set; }

        static Database()
        {
            Instance = new Database();
        }
    }
}
