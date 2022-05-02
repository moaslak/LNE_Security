using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LNE_Security;

public class Employee : Person
{
    public UInt16 EID { get; set; }
    
    public string UserName { get; set; }
    public string Password { get; set; }
}
