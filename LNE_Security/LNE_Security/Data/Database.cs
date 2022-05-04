using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using LNE_Security;

namespace LNE_Security;

public partial class Database : Product
{
    public static Database Instance { get; private set; }

    static Database()
    {
        Instance = new Database();
    }
       
    Address address = new Address();
    ContactInfo contactInfo = new ContactInfo();
}
