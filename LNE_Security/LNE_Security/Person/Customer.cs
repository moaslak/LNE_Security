using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace LNE_Security;

public class Customer : Person
{
    private Person _person { get; set; }

    public UInt16 CID { get; set; }
    public UInt16 CompanyID { get; set; }  

    public DateTime? newestOrder { get;set; }
}
